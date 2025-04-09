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
    public class ChangeDetailsController : Controller
    {
        
        private readonly ILogger<ProjectController> _logger;
        private readonly AplicationDbContext _context;

        public ChangeDetailsController(ILogger<ProjectController> logger, AplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        [HttpGet("GetChangeDetailsByProjectId/{projectId}")]
        public async Task<ActionResult<ChangeDetails>> GetChangeDetailsByProjectId(int projectId){

            var tasksIds = new List<int>();
            var sprintTasksIds = await _context.Sprint
            .Where(s => s.Project!.Id == projectId)
            .SelectMany(s => s.Tasks!)
            .Select(t => t.Id)
            .ToListAsync();

            var productBacklogTasksIds = await _context.ProductBacklog
            .Where(s => s.Project!.Id == projectId)
            .SelectMany(s => s.Tasks!)
            .Select(t => t.Id)
            .ToListAsync();
            if(sprintTasksIds.Count!=0){
                tasksIds.AddRange(sprintTasksIds);
            }    
            if(productBacklogTasksIds.Count!=0){
                tasksIds.AddRange(productBacklogTasksIds);
            }

            // System.Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(tasksIds));
            var tasks = await _context.ChangeDetails.Where(x=>tasksIds.Contains((int)x.TaskId!)).ToListAsync();
            // System.Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(tasks));
            // if(taskIds.Count==0){
            //     return NoContent();
            // }
            
            return Ok(tasks);
        }
    }
}