using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using BackEnd_Server.Data;
using BackEnd_Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BackEnd_Server.Controllers
{
    [Route("[controller]")]
    public class ProjectController : ControllerBase
    {
        private readonly ILogger<ProjectController> _logger;
        private readonly AplicationDbContext _context;

        public ProjectController(ILogger<ProjectController> logger, AplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }
                
        [HttpGet("GetProjects")]
        public async Task<ActionResult<IEnumerable<Project>>> GetProjects()
        {
            List<Project> projects = await _context.Project.ToListAsync();
            if(projects.Count == 0){
                return NoContent();
            }
            return Ok(projects);
        }
        
        [HttpPost("InsertProject")]
        public async Task<ActionResult<Project>> InsertProject([FromBody] Project project)
        {
            Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(project));

            // Aseguramos que la lista TeamProjects no sea nula
            project.TeamProjects ??= [];
            
            // Para cada relación, asignamos la referencia al proyecto
            foreach (var tp in project.TeamProjects)
            {
                tp.Project = project;
            }
            if(project.ProductBacklog != null)
            {
                project.ProductBacklog.Project = project;
                 _context.ProductBacklog.Add(project.ProductBacklog);
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                await _context.Project.AddAsync(project);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return Ok(project);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                System.Console.WriteLine(ex.Message);
                return StatusCode(500, new { message = "Error al insertar el proyecto", error = ex.Message });
            }
        }

        
        [HttpGet("GetProjectById/{id}")]
        public async Task<ActionResult<Project>> GetProjectById(int id)
        {
            var project = await _context.Project
                                .Include(p => p.Sprints)
                                .FirstOrDefaultAsync(p => p.Id == id);
            if (project == null)
            {
                return NotFound();
            }
            return Ok(project);
        }

       [HttpGet("GetProductBacklogById/{projectId}")]
        public async Task<ActionResult<ProductBacklog>> GetProductBacklogById(int projectId)
        {
            System.Console.WriteLine(projectId);
            var backlog = await _context.ProductBacklog.FirstOrDefaultAsync(pb => pb.ProjectId == projectId);
            if (backlog == null)
            {
                return NotFound();
            }

            // Asignar las tareas al backlog, incluso si la lista queda vacía
            backlog.Tasks = await _context.TaskEntity
                .Where(task => task.ProductBacklog != null && task.ProductBacklog.Id == backlog.Id)
                .ToListAsync();
            
            // En lugar de retornar NotFound si no hay tareas, retornamos el backlog (con Tasks vacía)
            return Ok(backlog);
        }


    }
}