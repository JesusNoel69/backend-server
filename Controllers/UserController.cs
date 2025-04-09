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
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly AplicationDbContext _context;
        public UserController(ILogger<UserController> logger, AplicationDbContext context)
        {
            _context = context;
            _logger = logger;
        }

        [HttpPost("AddDeveloper")]
        public async Task<ActionResult<Developer>> AddDeveloper([FromBody] Developer developer)
        {
            if (developer == null)
            {
                return BadRequest("El objeto Developer es nulo.");
            }
            //developer.Team = null; // O deja null si aún no está asignado a un equipo.
            
            var transaction = await _context.Database.BeginTransactionAsync();
            try{
                Console.WriteLine("developer: "+System.Text.Json.JsonSerializer.Serialize(developer));
                _context.Attach(developer.Team);
                // _context.Attach(developer.Team!);

                await _context.Developer.AddAsync(developer);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }catch(Exception error){
                Console.WriteLine(error.ToString());
                await transaction.RollbackAsync();
            }
            return Ok(developer);
        }
        [HttpPost("AddProductOwner")]
        public async Task<ActionResult<ProductOwner>> AddProductOwner([FromBody] ProductOwner productOwner)
        {
            if (productOwner == null)
            {
                return BadRequest("El objeto ProductOwner es nulo.");
            }
            Console.WriteLine("ProductOwner: " + System.Text.Json.JsonSerializer.Serialize(productOwner));

            // Inicia la transacción
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // 1. Agregar el ProductOwner y guardar para obtener su ID generado.
                var productOwnerEntry = _context.ProductOwner.Add(productOwner);
                await _context.SaveChangesAsync();
                System.Console.WriteLine("Es: "+productOwnerEntry.Entity.Id);
                // 2. Crear un nuevo Team y asignarle el ID del ProductOwner.
                Team newTeam = new Team
                {
                    Name = "equipo " +productOwner.Name ,
                    Code = "",
                    ProductOwnerId = productOwnerEntry.Entity.Id
                };
                _context.Team.Add(newTeam);
                await _context.SaveChangesAsync();

                // 3. Asignar el nuevo Team al ProductOwner.
                productOwner.Team = newTeam;
                await _context.SaveChangesAsync();

                // 4. Commit de la transacción
                await transaction.CommitAsync();

                return Ok(productOwner);
            }
            catch (Exception error)
            {
                Console.WriteLine(error.ToString());
                await transaction.RollbackAsync();
                return StatusCode(500, "Error al agregar el ProductOwner");
            }
        }


        [HttpPost("AddTeam")]
        public async Task<ActionResult<ProductOwner>> AddTeam([FromBody] Team team)
        {
            if (team == null)
            {
                return BadRequest("El objeto ProductOwner es nulo.");
            }            
            Console.WriteLine("team: "+System.Text.Json.JsonSerializer.Serialize(team));
            var transaction = await _context.Database.BeginTransactionAsync();
            try{
                _context.Team.Add(team);
                await _context.SaveChangesAsync();
                transaction.Commit();
            }catch(Exception error){
                Console.WriteLine(error.ToString());
                transaction.Rollback();
            }
            return Ok(team);
        }
        [HttpGet("GetProductOwner/{teamId}")]
        public async Task<ActionResult<ProductOwner>> GetProductOwner(int teamId){
            var developerId = await _context.Team.Where(x=>x.Id == teamId).Select(x=>x.ProductOwnerId).FirstOrDefaultAsync();
            Console.WriteLine("developerId: "+developerId);
            
            var developer = await _context.ProductOwner.FirstOrDefaultAsync(x=>x.Id == developerId);

            Console.WriteLine("developer: "+System.Text.Json.JsonSerializer.Serialize(developer));
            if (developer == null){
                return BadRequest("no product owner");
            }
            return developer;
        }
        [HttpGet("GetTeamById/{teamId}")]
        public async Task<ActionResult<Team>> GetTeamById( int teamId){
            var team = await _context.Team.Where(x=>x.Id == teamId).FirstOrDefaultAsync();
            
            if (team == null){
                return BadRequest("no team");
            }
            Console.WriteLine("team: "+System.Text.Json.JsonSerializer.Serialize(team));
            return team;
        }

        [HttpPost("GetDevelopersByTasksIds")]
        public async Task<ActionResult<List<(string DeveloperName, int TaskId)>>> GetDevelopersByTasksIds([FromBody] List<int> Ids)
        {
            // Obtener las tareas asociadas a los IDs proporcionados
            var tasks = await _context.TaskEntity
                .Where(x => Ids.Contains(x.Id))
                .Select(x => new 
                { 
                    TaskId = x.Id, 
                    DeveloperName = x.Developer != null ? x.Developer.Name : "Sin asignar"
                })
                .ToListAsync();

            // Verificar si hay datos
            if (tasks.Count == 0)
            {
                Console.WriteLine("No hay desarrolladores asignados a estas tareas.");
                return NotFound("No se encontraron desarrolladores para las tareas proporcionadas.");
            }

            // Retornar la lista con el nombre del desarrollador y el ID de la tarea
            return Ok(tasks);
        }
        [HttpPost("GetDevelopersByIds")]
        public async Task<ActionResult<List<Developer>>> GetDevelopersByIds([FromBody] List<int> Ids)
        {
            // Obtener las tareas asociadas a los IDs proporcionados
            var developers = await _context.Developer
                .Where(x => Ids.Contains(x.Id))
                .ToListAsync();

            // Verificar si hay datos
            if (developers.Count == 0)
            {
                Console.WriteLine("No hay desarrolladores asignados a estas tareas.");
                return NotFound("No se encontraron desarrolladores para las tareas proporcionadas.");
            }

            // Retornar la lista con el nombre del desarrollador y el ID de la tarea
            return Ok(developers);
        }

        [HttpGet("GetDevelopersByProjectId/{projectId}")]
        public async Task<ActionResult<List<Developer>>> GetDevelopersByProjectId(int projectId)
        {
            var teams = await _context.Team
                .Where(x => x.TeamProjects != null && x.TeamProjects.Any(tp => tp.ProjectId == projectId))
                .Include(x => x.Developers)
                .ToListAsync();

            var developers = teams
                .SelectMany(t => t.Developers ?? Enumerable.Empty<Developer>())
                .Distinct()
                .ToList();
            Console.WriteLine("developers: "+System.Text.Json.JsonSerializer.Serialize(developers));
            if (developers.Count==0){
                return BadRequest("no team");
            }
            return developers;
        }

        [HttpGet("GetTeamsByProductOwnerId/{productOwnerId}")]
        public async Task<ActionResult<List<Team>>> GetTeamByProductOwnerId( int productOwnerId){
            System.Console.WriteLine(productOwnerId);
            var teams = await _context.Team.Where(x=>x.ProductOwnerId == productOwnerId).ToListAsync();
            
            if (teams.Count == 0)
            {
                return BadRequest("no team");
            }
            Console.WriteLine("team: "+System.Text.Json.JsonSerializer.Serialize(teams));
            return teams;
        }

        [HttpGet("GetDeveloperByTeamId/{id}")]
        public async Task<ActionResult<List<Developer>>> GetDeveloperByTeamId(int id){
            System.Console.WriteLine(id);
            var developers = await _context.Developer.Where(x=> x.Team!=null && x.Team.Id == id).ToListAsync();
            
            if (developers.Count == 0)
            {
                return BadRequest("no team");
            }
            Console.WriteLine("team: "+System.Text.Json.JsonSerializer.Serialize(developers));
            return developers;
        }
    }
}