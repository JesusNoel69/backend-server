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
        public async Task<ActionResult<Project>> InsertProject([FromBody]Project project)
        {
            // Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(project));
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _context.Project.Add(project);
                
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return Ok();//CreatedAtAction(nameof(GetProjectById), new { id = project.Id }, project);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                System.Console.WriteLine(ex.Message);
                return StatusCode(500, new { message = "Error al insertar el proyecto", error = ex.Message });
            }
        }
      
        [HttpGet("{id}")]
        public async Task<ActionResult<Project>> GetProjectById(int id)
        {
            var project = await _context.Project.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            return Ok(project);
        }

    }
}