using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MiProyecto8;
using MyApp.Namespace;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MyApp.Namespace
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController
    {
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _context;
        public AuthController(IConfiguration configuration, AppDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest loginRequest)
        {
            // 1. Validar credenciales contra la base de datos
            var alumno = _context.Alumnos.FirstOrDefault(a => a.Correo == loginRequest.Correo && a.Password == loginRequest.Password);

            if (alumno != null)
            {
                // 2. CREAR LOS CLAIMS (DATOS DEL USUARIO)
                // Los claims ahora se basan en los datos del alumno encontrado en la BD
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, alumno.Correo),
                    new Claim("Nombre", alumno.Nombre),
                    new Claim("IdAlumno", alumno.Id.ToString()),
                    new Claim(ClaimTypes.Role, "Estudiante"),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                // 3. GENERAR EL TOKEN (Igual que antes)
                // Asegúrate de que la clave JWT no sea nula.
                // El '!' al final indica al compilador que confías en que no será nulo en tiempo de ejecución.

                // 3. GENERAR EL TOKEN (Igual que antes)
                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Issuer = _configuration["Jwt:Issuer"],
                    Audience = _configuration["Jwt:Audience"],
                    Expires = DateTime.UtcNow.AddHours(1),
                    SigningCredentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256),
                    Subject = new ClaimsIdentity(authClaims)
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);

                return Ok(new { token = tokenHandler.WriteToken(token) });
            }

            // Si las credenciales no son válidas o el alumno no se encuentra, retornar un error de no autorizado
            return Unauthorized("Credenciales inválidas.");
        }
    }
}
