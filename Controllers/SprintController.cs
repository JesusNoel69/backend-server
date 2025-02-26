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
    }
}