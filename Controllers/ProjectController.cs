using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using BackEnd_Server.Data;
using BackEnd_Server.DTOs;
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
public async Task<ActionResult<IEnumerable<Project>>> GetProjects([FromQuery] int userId, [FromQuery] bool role)
{
    Team? team;
    TeamProject? teamProject;

    if (role)
    {
        // Para ProductOwner
        team = await _context.Team.FirstOrDefaultAsync(x => x.ProductOwnerId == userId);
        System.Console.WriteLine(team);
        if (team == null)
        {
            return BadRequest("Proyectos no encontrados para el team.");
        }
        teamProject = await _context.TeamProject.FirstOrDefaultAsync(x => x.TeamId == team.Id);
        System.Console.WriteLine(teamProject);
        if (teamProject == null)
        {
            return BadRequest("TeamProject nulo.");
        }
    }
    else
    {
        // Incluimos la propiedad Team para el desarrollador
        var developer = await _context.Developer
                                      .Include(d => d.Team)
                                      .FirstOrDefaultAsync(x => x.Id == userId);
        if (developer == null)
        {
            return BadRequest("Usuario sin proyectos nulo.");
        }
        if (developer.Team == null)
        {
            return BadRequest("El desarrollador no tiene equipo asignado.");
        }
        team = developer.Team;
        teamProject = await _context.TeamProject.FirstOrDefaultAsync(x => x.TeamId == team.Id);
        if (teamProject == null)
        {
            return BadRequest("TeamProject nulo.");
        }
    }

    List<Project> projects = await _context.Project
                                            .Where(x => x.Id == teamProject.ProjectId)
                                            .ToListAsync();
    if (projects.Count == 0)
    {
        return NoContent();
    }
    return Ok(projects);
}

        
        [HttpPost("InsertProject")]
        public async Task<ActionResult<Project>> InsertProject([FromBody] ProjectRequestDTO projectRequest)
        {
            // System.Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(projectRequest));
            var project = projectRequest.Project;
            var userId = projectRequest.UserId;
            if(project==null){
                System.Console.WriteLine("nada que crear, proyecto vacio");
                return Ok("Nada que crear");
            }
    
            var user = await _context.ProductOwner.FindAsync(userId);
            if (user == null || !user.Rol)  
            {
                System.Console.WriteLine("no el product Owner");
                return BadRequest(new { message = "El usuario no es un ProductOwner" });
            }
    
            project.TeamProjects ??= [];
            int order =1;
            foreach (var sprint in project.Sprints??[])
            {
                foreach (var task in sprint.Tasks??[])
                {
                    task.Order=order;
                    order++;
                }
                order=1;
            }
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

        [HttpGet("GetWeeklyScrumsProductBacklogByProjectId/{id}")]
        public async Task<ActionResult<Project>> GetWeeklyScrumsProductBacklogByProjectId(int id)
        {
            var sprints = await _context.Sprint.Where(sprint=>sprint.Project!.Id == id).ToListAsync();
            var tasksId = await _context.TaskEntity.Where(task => sprints.Select(x=>x.Id).Contains(task.Sprint!.Id)).Select(task=>task.Id).ToListAsync();
            System.Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(tasksId));

            var changes = await _context.ChangeDetails.Where(change=>tasksId.Contains(change.Id)).ToListAsync();

            

            // if (project == null)
            // {
            //     return NotFound();
            // }


            return Ok();
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