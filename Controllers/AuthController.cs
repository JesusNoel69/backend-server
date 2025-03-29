using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BackEnd_Server.Data;
using DotNetEnv;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace BackEnd_Server.Controllers
{
    [Route("[controller]")]
    public class AuthController : Controller
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IConfiguration _configuration;
        private AplicationDbContext _context;
        
        public AuthController(IConfiguration configuration, AplicationDbContext context)
        {
            _configuration = configuration;
            _context = context;
            Env.Load();
        }
        
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel login)
        {
            // Validación simple: buscamos el usuario en la base de datos
            var userExists = _context.User
                .Where(x => x.Password == login.Password && x.Account == login.Username)
                .FirstOrDefault();//tolist
            
            // Si no se encontró el usuario, se retorna Unauthorized
            if (userExists == null)//.count==0
                return Unauthorized();
            
            // Obtener la clave secreta desde la configuración
            var keyString = Environment.GetEnvironmentVariable("JWT");//_configuration["JWT"] ?? "";
            var key = Encoding.ASCII.GetBytes(keyString??"");
            
            // Si la clave es vacía, lanza una excepción para evitar crear el token con key length 0
            if (key.Length == 0)
            {
                throw new Exception("La clave JWT está vacía. Asegúrate de que la variable de entorno 'JWT' esté configurada correctamente.");
            }
            
            // Crear el token JWT
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] 
                {
                    new Claim(ClaimTypes.Name, login.Username)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            
            return Ok(new { token = tokenString, user = userExists });
        }
    }
    public class LoginModel
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
    }
}
