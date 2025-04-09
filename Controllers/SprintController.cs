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

       [HttpPost("GetScrumsWeeklyByTaskIds")]
        public async Task<ActionResult<WeeklyScrum[]>> GetScrumsByTaskIds([FromBody] List<int> taskIds)
        {
            var scrums = await _context.WeeklyScrum
                                    .Where(scrum => taskIds.Contains(scrum.TaskId))
                                    .ToListAsync();

            return Ok(scrums);
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

        [HttpPost("AddSprint/{projectId}")]
        public async Task<IActionResult> AddSprint([FromBody] Sprint sprint, int projectId)
        {
            // Recuperar el ProductBacklog (con sus tareas) y el proyecto existentes
            var productBacklog = await _context.ProductBacklog
                .Include(pb => pb.Tasks)
                .FirstOrDefaultAsync(pb => pb.ProjectId == projectId);
            var project = await _context.Project.FindAsync(projectId);

            if (productBacklog == null || project == null)
            {
                return BadRequest("Sin product backlog o proyecto");
            }

            // Asociar el sprint al proyecto existente
            sprint.Project = project;

            // Extraer los Ids de las tareas que se desean mover desde el sprint
            var taskIdsToMove = sprint.Tasks?.Select(t => t.Id).ToList() ?? new List<int>();

            // Evitar que el sprint intente adjuntar las instancias de Task que ya están siendo rastreadas
            sprint.Tasks = null;

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Insertar el sprint y guardar para que se asigne su Id
                await _context.Sprint.AddAsync(sprint);
                await _context.SaveChangesAsync();

                // Si se especificaron tareas para mover...
                if (taskIdsToMove.Any())
                {
                    // Buscar en el backlog (ya rastreado) las tareas cuyos Ids coincidan
                    var tasksToUpdate = productBacklog.Tasks
                        .Where(t => taskIdsToMove.Contains(t.Id))
                        .ToList();

                    foreach (var trackedTask in tasksToUpdate)
                    {
                        // Remover la tarea de la colección del backlog
                        productBacklog.Tasks.Remove(trackedTask);
                        // Asignar la relación con el sprint: si tienes la propiedad SprintId, se puede asignar directamente
                        _context.Entry(trackedTask).Property("SprintId").CurrentValue = sprint.Id;
                        // Desasociar la tarea del backlog
                        trackedTask.ProductBacklog = null;
                        // Si tienes ProductBacklogId, asigna null:
                        // _context.Entry(trackedTask).Property("ProductBacklogId").CurrentValue = null;
                        _context.TaskEntity.Update(trackedTask);
                    }
                    await _context.SaveChangesAsync();
                }

                await transaction.CommitAsync();
            }
            catch (Exception error)
            {
                System.Console.WriteLine(error.Message);
                await transaction.RollbackAsync();
                throw;
            }
            return Ok(sprint);
        }
        
       
    }
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