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
            System.Console.WriteLine(System.Text.Json.JsonSerializer.Serialize( taskDto));
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

            
            var newWeeklyScrum = new WeeklyScrum
            {
                CreatedAt = DateTime.Now,
                Information = taskDto.WeeklyScrum,  // Información del Scrum (si la envían)
                TaskId = task.Id,
                Task = task,  // Asociamos la tarea recién creada
                DeveloperId = taskDto.DeveloperId ?? 0 // Aseguramos que el developer se asocie
            };

            _context.WeeklyScrum.Add(newWeeklyScrum);
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
         [HttpGet("GetTasksWithDeveloperNameBySprintId/{sprintId}")]
        public async Task<ActionResult<List<TaskWithDeveloperNameDto>>> GetTasksWithDeveloperNameBySprintId(int sprintId)
        {
           var tasks = await _context.TaskEntity
            .Where(t => t.Sprint != null && t.Sprint.Id == sprintId)
            .Include(t => t.Developer) // 👈 Necesario para obtener el nombre del Developer
            .Select(t => new TaskWithDeveloperNameDto
            {
                Id = t.Id,
                Description = t.Description,
                State = t.State,
                DeveloperId = t.Developer!.Id,
                DeveloperName = t.Developer != null ? t.Developer.Name : "Sin Asignar"
            })
            .ToListAsync();

            if (tasks.Count == 0)
            {
                return NoContent();
            }

            return Ok(tasks);
        }

        [HttpGet("GetTasksByDeveloperId/{id}")]
        public async Task<ActionResult<List<Models.Task>>> GetTasksById(int id){
            var developer = await _context.Developer.Where(developer=>id == developer.Id).FirstOrDefaultAsync();
            if(developer==null){
                System.Console.WriteLine("no existe developer con ese id");
                return NoContent();
            }
            var tasks = await _context.TaskEntity.Where(task => id==developer.Id).ToListAsync();
            return Ok(tasks);
        }

        [HttpPost("AddScrumWeeklyToTask")]
        public async Task<ActionResult<bool>> AddScrumWeeklyToTask([FromBody] ScrumRequest request)
        {
            if(request.Content=="" || request.Content==null){
                System.Console.WriteLine("sin texto para guardar");
                return NoContent();
            }
            if(request.TaskId==0){
                return BadRequest("id para tarea no valido");
            }
            var developers = await _context.Developer.FirstOrDefaultAsync(developer=>developer.Id==request.DeveloperId);
            if(developers==null){
                System.Console.WriteLine("no existen usuarios con ese id");
                return BadRequest("no existen usuarios con ese id");
            }
            var tasks = await _context.TaskEntity.FirstOrDefaultAsync(task=>task.Id==request.TaskId);
            if(tasks==null){
                System.Console.WriteLine("no existen tareas con ese id");
                return BadRequest("no existen tareas con ese id");
            }
            var weeklyScrums = await _context.WeeklyScrum
                .Where(scrum => scrum.TaskId == request.TaskId && scrum.DeveloperId == request.DeveloperId)
                .ToListAsync();

            var now = DateTime.UtcNow;
            var startOfWeek = now.Date.AddDays(-(int)now.DayOfWeek); 
            var endOfWeek = startOfWeek.AddDays(7);

            var alreadyHasScrumThisWeek = weeklyScrums.Any(scrum =>
                scrum.CreatedAt >= startOfWeek && scrum.CreatedAt < endOfWeek
            );

            if (alreadyHasScrumThisWeek)
            {
                System.Console.WriteLine("Ya existe un Scrum semanal registrado esta semana para esta tarea.");
                return BadRequest("Ya existe un Scrum semanal registrado esta semana para esta tarea.");
            }

            // Si no hay Scrum esta semana, lo agregamos
            var newScrum = new WeeklyScrum
            {
                DeveloperId = request.DeveloperId,
                TaskId = request.TaskId,
                Information = request.Content,
                CreatedAt = now
            };

            System.Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(newScrum));
            var transaction = _context.Database.BeginTransaction();
            try{
                await _context.WeeklyScrum.AddAsync(newScrum);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }catch(Exception error){
                System.Console.WriteLine(error.Message.ToString());
                await transaction.RollbackAsync();
            }
            return Ok(true);
        }


        [HttpPatch("UpdateTaksState/{userId}")]
        public async Task<ActionResult<bool>> UpdateTasksState(int userId, [FromBody] List<Models.Task> tasks )
        {
            System.Console.WriteLine($"userId recibido: {userId}");

            // var developerExists = await _context.Developer.AnyAsync(d => d.Id == userId);
            // if (!developerExists)
            // {
            //     return BadRequest("Developer no encontrado");
            // }
            System.Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(tasks));
            var originalTasks = await _context.TaskEntity
                                          .Where(t => tasks.Select(tsk => tsk.Id).Contains(t.Id))//t.Developer!.Id == userId
                                          .AsNoTracking()
                                          .ToListAsync();
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var changedTasks = new List<Models.Task>();
                _context.TaskEntity.UpdateRange(tasks);
                
                foreach (var task in tasks)
                {
                    var originalTask = originalTasks.FirstOrDefault(t => t.Id == task.Id);
                    if (originalTask != null && originalTask.State!=task.State) //&& !originalTask.Equals(task) Comparar solo si los datos son diferentes
                    {
                        System.Console.WriteLine("si hay dstintos");
                        changedTasks.Add(task);
                    }
                }
                var user = await _context.User.FirstOrDefaultAsync(x=>x.Id == userId);
                var changeDetails = new List<ChangeDetails>();
                // if(developerExists){
                    foreach (var changedTask in changedTasks)
                    {
                        var originalTask = originalTasks.FirstOrDefault(t => t.Id == changedTask.Id);
                        System.Console.WriteLine(changedTask.Name);
                        var change = new ChangeDetails
                            {
                                
                                Sprints = null,
                                SprintNumber = null,
                                TaskId = changedTask.Id,
                                TaskInformation = changedTask.Name+" se actualizo de estado "+originalTask!.State+" a "+changedTask.State,
                                UserData = "Actualizacion de orden por parte de "+user?.Name,
                            };

                        changeDetails.Add(change);
                    }
                // }
                if(changeDetails.Count!=0)
                {
                    await _context.ChangeDetails.AddRangeAsync(changeDetails);
                }
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }catch(Exception error){
                System.Console.WriteLine(error.ToString());
                await transaction.RollbackAsync();
                return BadRequest();
            }
            return Ok();
        }
        [HttpPatch("UpdateTaksOrder/{userId}")]
        public async Task<ActionResult<bool>> UpdateTasksOrder(int userId, [FromBody] List<Models.Task> tasks)
        {
            Console.WriteLine($"✅ userId recibido: {userId}");
            Console.WriteLine($"📦 Tareas recibidas:\n{System.Text.Json.JsonSerializer.Serialize(tasks)}");

            // Verificamos si el userId corresponde a un developer válido
            // var developer = await _context.User.FirstOrDefaultAsync(u => u.Id == userId);
            // if (developer == null)
            // {
            //     return BadRequest("El usuario no es un developer válido.");
            // }

            // Obtenemos las tareas originales (antes del cambio)
            var taskIds = tasks.Select(tsk => tsk.Id).ToList();
            var sprint = await _context.Sprint
                .Where(s => s.Tasks!.Any(t => taskIds.Contains(t.Id)))  // Verifica si alguna tarea del Sprint tiene el Id en taskIds
                .FirstOrDefaultAsync();

            var originalTasks = await _context.TaskEntity
                .Where(t => taskIds.Contains(t.Id))
                .Include(t => t.Developer)
                .Include(t => t.Sprint)
                .AsNoTracking()
                .ToListAsync();

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var changedTasks = new List<Models.Task>();
                _context.TaskEntity.UpdateRange(tasks); // Marca tareas para actualizar

                // Detectar cambios de orden
                foreach (var task in tasks)
                {
                    var originalTask = originalTasks.FirstOrDefault(t => t.Id == task.Id);
                    if (originalTask != null && originalTask.Order != task.Order)
                    {
                        Console.WriteLine($"🔁 Cambio detectado en la tarea {task.Name}: {originalTask.Order} ➡ {task.Order}");
                        changedTasks.Add(task);
                    }
                }
                var user = await _context.User.FirstOrDefaultAsync(x=>x.Id==userId);

                // Preparar registros de cambio
                var changeDetails = new List<ChangeDetails>();
                foreach (var changedTask in changedTasks)
                {
                    var originalTask = originalTasks.FirstOrDefault(t => t.Id == changedTask.Id);
                    if (originalTask != null && originalTask.Order != changedTask.Order)
                    {
                        Console.WriteLine($"Cambio detectado en la tarea {changedTask.Name}: {originalTask.Order} ➡ {changedTask.Order}");
                            var change = new ChangeDetails
                            {
                                Sprints = null,
                                SprintNumber = null,
                                TaskId = changedTask.Id,
                                TaskInformation = changedTask.Name+" se actualizo de estado "+originalTask!.Order+" a "+changedTask.Order,
                                UserData = "Actualizacion de orden por parte de "+user?.Name,
                                // DeveloperId = userId
                            };
                            changeDetails.Add(change);
                    }

                }
                
                if (changeDetails.Count != 0) // Solo agregar si hay cambios
                {
                    Console.WriteLine("Se registrarán cambios en ChangeDetails");
                    await _context.ChangeDetails.AddRangeAsync(changeDetails);
                }
                else
                {
                    Console.WriteLine("No se detectaron cambios para registrar.");
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Ok(true);
            }
            catch (Exception error)
            {
                Console.WriteLine($"🔥 Error al actualizar tareas: {error}");
                await transaction.RollbackAsync();
                return BadRequest("Error al actualizar el orden de las tareas");
            }
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
            ProductBacklog? backlogEntity = null;
            if (payload.Backlog != null)
            {
                backlogEntity = await _context.ProductBacklog.FirstOrDefaultAsync(x => x.Id == payload.Backlog.BacklogId);
                if (backlogEntity == null)
                    return BadRequest("No se encontró el ProductBacklog con el Id proporcionado.");
            }

            Models.Sprint? sprintEntity = null;
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