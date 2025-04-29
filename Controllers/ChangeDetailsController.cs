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
        public async Task<ActionResult<List<ChangeDetailWithTaskNameDto>>> 
            GetChangeDetailsByProjectId(int projectId)
        {
            // 1) Recuperar todos los TaskIds del proyecto
            var sprintTaskIds = await _context.Sprint
                .Where(s => s.Project!.Id == projectId)
                .SelectMany(s => s.Tasks!)
                .Select(t => t.Id)
                .ToListAsync();

            var backlogTaskIds = await _context.ProductBacklog
                .Where(pb => pb.Project!.Id == projectId)
                .SelectMany(pb => pb.Tasks!)
                .Select(t => t.Id)
                .ToListAsync();

            var allTaskIds = sprintTaskIds
                .Concat(backlogTaskIds)
                .Distinct()
                .ToList();

            if (!allTaskIds.Any())
                return NoContent();

            // 2) Proyectar ChangeDetails + TaskName
            var result = await _context.ChangeDetails
                .Where(cd => cd.TaskId != null && allTaskIds.Contains(cd.TaskId.Value))
                .Join(
                    _context.TaskEntity,
                    cd => cd.TaskId,
                    t  => t.Id,
                    (cd, t) => new ChangeDetailWithTaskNameDto
                    {
                        Id              = cd.Id,
                        SprintNumber    = cd.SprintNumber,
                        UserData        = cd.UserData,
                        TaskInformation = cd.TaskInformation,
                        TaskId          = t.Id,
                        TaskName        = t.Name!
                    }
                )
                .ToListAsync();
                // System.Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(result));
            return Ok(result);
        }

    }
}