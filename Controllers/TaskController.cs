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
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;

namespace BackEnd_Server.Controllers
{
    [Route("[controller]")]
    public class TaskController : Controller
    {
        private readonly ILogger<TaskController> _logger;
        private readonly AplicationDbContext _context;

        public TaskController(ILogger<TaskController> logger, AplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        [HttpGet("GetTaskById")]
        public async Task<ActionResult<Models.Task>> GetTaskBySprintId(int sprintId){
            var task= await _context.TaskEntity
                            .Where(
                                x => x.Sprint!=null 
                                && x.Sprint.Id == sprintId
                            ).FirstOrDefaultAsync();
             if (task == null)
            {
                return NotFound();
            }

            return Ok(task);
        }
        
        // [HttpGet("GetTaskByProductBacklogId")]
        // public async Task<ActionResult<Models.Task>> GetTaskByProductBacklogId(int productBacklogId){
        //     var task= await _context.ProductBacklog
        //                 .Where(
        //                     && x.Task == productBacklogId
        //                 ).FirstOrDefaultAsync();
        //      if (task == null)
        //     {
        //         return NotFound();
        //     }

        //     return Ok(task);
        // }

        [HttpGet("GetProgressvalue")]
        public async Task<double> ProgressValue(int sprintId){
            var tasks = await _context.TaskEntity
                                .Where(
                                    task=> task.Sprint!=null 
                                    && task.Sprint.Id == sprintId
                                ).ToListAsync();
            double total =0;
            if(tasks.Count == 0){
                return 0;
            }
            foreach (var task in tasks)
            {
                /*states:
                1 por hacer 0%
                2 en progreso 30%
                3 revision 75%
                4 terminado 100%
                */
                if (task.State == 2) {
                    total += 0.3;
                } else if (task.State == 3) {
                    total += 0.75;
                } else {
                    total += 1;
                }
            }
            return total / tasks.Count * 100;
        }
        
       [HttpPost("AddTaskToProductBacklog")]
        public async Task<ActionResult<ProductBacklog>> InsertTaskInProductBacklog([FromBody] TaskCreateDto taskDto)
        {
            // Verificar que se envíe un ProductBacklogId válido
            if (taskDto == null || taskDto.ProductBacklogId <= 0)
            {
                return BadRequest("Datos de la tarea o backlog inválidos.");
            }

            // Recuperar el ProductBacklog correspondiente
            var productBacklog = await _context.ProductBacklog
                .Include(pb => pb.Tasks)
                .FirstOrDefaultAsync(pb => pb.Id == taskDto.ProductBacklogId);

            if (productBacklog == null)
            {
                return NotFound("No se encontró el Product Backlog especificado.");
            }

            int taskOrder=1;
            if(productBacklog.Tasks!=null && productBacklog.Tasks.Count>0)
            {
                foreach (var item in productBacklog.Tasks)
                {
                    if(item.Order>taskOrder){
                        taskOrder=item.Order;
                    }
                }
                taskOrder++;
            }
            var developer = await _context.Developer.FirstOrDefaultAsync(dev => dev.Id==taskDto.DeveloperId);
            // Mapear el DTO a la entidad Task
            var task = new Models.Task
            {
                Name = taskDto.Name,
                Description = taskDto.Description,
                State = taskDto.State,
                Order = taskOrder,
                ProductBacklog = productBacklog,
                WeeklyScrums = null,
                Developer = developer,
            };

            _context.TaskEntity.Add(task);
            await _context.SaveChangesAsync();

            // Recargar el Product Backlog con la tarea recién agregada
            var updatedBacklog = await _context.ProductBacklog
                .Include(pb => pb.Tasks)
                .FirstOrDefaultAsync(pb => pb.Id == productBacklog.Id);

            return Ok(updatedBacklog);
        }

        [HttpGet("GetTasksBySprintId/{sprintId}")]
        public async Task<ActionResult<List<Models.Task>>> GetTasksBySprintId(int sprintId)
        {
            var tasks = await _context.TaskEntity
                            .Where(t => t.Sprint!=null && t.Sprint.Id == sprintId)
                            .ToListAsync();
            if (tasks == null || tasks.Count == 0)
            {
                return NoContent();
            }
            return Ok(tasks);
        }

        [HttpPatch("UpdateTaksState")]
        public async Task<ActionResult<bool>> UpdateTasksState([FromBody] List<Models.Task> tasks )
        {
            System.Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(tasks));
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {

                _context.TaskEntity.UpdateRange(tasks);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }catch(Exception error){
                System.Console.WriteLine(error.ToString());
                await transaction.RollbackAsync();
                return BadRequest();
            }
            return Ok();
        }

        [HttpPatch("UpdateTaksOrder")]
        public async Task<ActionResult<bool>> UpdateTasksOrder([FromBody] List<Models.Task> tasks )
        {
            System.Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(tasks));
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {

                _context.TaskEntity.UpdateRange(tasks);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }catch(Exception error){
                System.Console.WriteLine(error.ToString());
                await transaction.RollbackAsync();
                return BadRequest();
            }
            return Ok();
        }
        
        [HttpPatch("UpdateTasksSprint")]
        public async Task<ActionResult<bool>> UpdateTasksSprint([FromBody] UpdateTasksSprintDTO payload)
        {
            if (payload == null)
                return BadRequest("Payload is null");

            // Extraer los IDs de las tareas a actualizar
            var backlogTaskIds = payload.Backlog?.Tasks?.Select(t => t.Id) ?? Enumerable.Empty<int>();
            var sprintTaskIds = payload.Sprint?.Tasks?.Select(t => t.Id) ?? Enumerable.Empty<int>();
            var allTaskIds = backlogTaskIds.Union(sprintTaskIds).Distinct().ToList();

            // Obtener las tareas desde la base de datos (estas serán las únicas instancias rastreadas)
            var tasksToUpdate = await _context.TaskEntity
                .Where(t => allTaskIds.Contains(t.Id))
                .ToListAsync();

            // Obtener las instancias únicas de backlog y sprint
            ProductBacklog backlogEntity = null;
            if (payload.Backlog != null)
            {
                backlogEntity = await _context.ProductBacklog.FirstOrDefaultAsync(x => x.Id == payload.Backlog.BacklogId);
                if (backlogEntity == null)
                    return BadRequest("No se encontró el ProductBacklog con el Id proporcionado.");
            }

            Models.Sprint sprintEntity = null;
            if (payload.Sprint != null)
            {
                sprintEntity = await _context.Sprint.FirstOrDefaultAsync(x => x.Id == payload.Sprint.SprintId);
                if (sprintEntity == null)
                    return BadRequest("No se encontró el Sprint con el Id proporcionado.");
            }

            // Actualizar cada tarea según su presencia en las listas del payload
            foreach (var task in tasksToUpdate)
            {
                if (payload.Backlog?.Tasks?.Any(t => t.Id == task.Id) == true)
                {
                    task.ProductBacklog = backlogEntity;
                    task.Sprint = null;
                }
                else if (payload.Sprint?.Tasks?.Any(t => t.Id == task.Id) == true)
                {
                    task.Sprint = sprintEntity;
                    task.ProductBacklog = null;
                }
            }
            System.Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(tasksToUpdate));
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception error)
            {
                Console.WriteLine(error.ToString());
                await transaction.RollbackAsync();
                return BadRequest();
            }
            return Ok(true);
        }
    }


}