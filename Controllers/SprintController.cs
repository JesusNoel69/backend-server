using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using BackEnd_Server.Data;
using BackEnd_Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BackEnd_Server.Controllers
{
    [Route("[controller]")]
    public class SprintController : Controller
    {
        private readonly ILogger<SprintController> _logger;
        private readonly AplicationDbContext _context;

        public SprintController(ILogger<SprintController> logger, AplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet("GetSprintsByProjectId")]
        public async Task<ActionResult<IEnumerable<Sprint>>> GetSprints(int projectId){
            var sprints = await _context.Sprint
                                .Where(sprint=>sprint.Project != null 
                                    && sprint.Project.Id==projectId).
                                    ToListAsync();
             if(sprints.Count == 0){
                return NoContent();
            }
            return Ok(sprints);
        }
        [HttpGet("GetSelectedSprint")]
        public async Task<ActionResult<Sprint>> GetSprint(int sprintId, int projectId){
            var sprint = await _context.Sprint
                                .Where(sprint=>sprint.Project != null 
                                && sprint.Project.Id==projectId && sprint.Id==sprintId)
                                .FirstOrDefaultAsync();
            if(sprint == null){
                return NoContent();
            }
            return Ok(sprint);
        }
        [HttpPost("AddSprint")]
        public async Task<IActionResult> AddSprint([FromBody]Sprint sprint){
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                await _context.Sprint.AddAsync(sprint);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception error)
            {
                System.Console.WriteLine(error.Message);
                await transaction.RollbackAsync();
                throw;
            }
            return Ok();
        }

        /*
            [HttpPost("AddSprint")]
            public async Task<IActionResult> AddSprint([FromBody] Sprint sprint)
            {
                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    // Si en el sprint se han incluido tareas, actualizamos cada una para quitar la referencia al ProductBacklog
                    if (sprint.Tasks != null && sprint.Tasks.Any())
                    {
                        foreach (var task in sprint.Tasks)
                        {
                            // Obtenemos la tarea existente desde la base de datos
                            var existingTask = await _context.TaskEntity.FindAsync(task.Id);
                            if (existingTask != null)
                            {
                                // Quitar la asociación con el backlog (o actualizar según la lógica de tu aplicación)
                                existingTask.ProductBacklog = null;
                                // Si usas clave foránea, podrías asignar: existingTask.ProductBacklogId = null;
                                _context.TaskEntity.Update(existingTask);
                            }
                        }
                    }

                    // Agregar el sprint a la base de datos
                    await _context.Sprint.AddAsync(sprint);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception error)
                {
                    System.Console.WriteLine(error.Message);
                    await transaction.RollbackAsync();
                    throw;
                }
                return Ok();
            }
        */
    }
}