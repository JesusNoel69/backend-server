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

            // Mapear el DTO a la entidad Task
            var task = new Models.Task
            {
                Name = taskDto.Name,
                Description = taskDto.Description,
                State = taskDto.State,
                Order = taskDto.Order,
                ProductBacklog = productBacklog,
                WeeklyScrums = null,
                Developer = null // Aquí puedes asignar el Developer buscándolo por taskDto.DeveloperId si lo requieres
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



    }
}