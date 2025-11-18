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
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _context; // Inyectamos el contexto de base de datos

        // Constructor: Recibimos configuración y el contexto de la BD
        public AuthController(IConfiguration configuration, AppDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            // 1. Buscar al ALUMNO por su Correo y Password en la base de datos
            var alumno = await _context.Alumnos
                                .FirstOrDefaultAsync(a => a.Correo == loginRequest.Correo 
                                                       && a.Password == loginRequest.Password);

            // 2. Validar si el alumno existe
            if (alumno == null)
            {
                return Unauthorized(new { message = "Correo o contraseña incorrectos" });
            }

            // 3. Generar los Claims (Datos dentro del token)
            // Aquí guardamos información útil del alumno dentro del token para no tener que consultar la BD a cada rato
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, alumno.Correo),          // Guardamos el correo como identificador
                new Claim("Nombre", alumno.Nombre),                 // Guardamos el nombre real (claim personalizado)
                new Claim("IdAlumno", alumno.Id.ToString()), // Muy útil: Guardamos el ID del alumno para usarlo luego
                new Claim(ClaimTypes.Role, "Alumno"),               // Asignamos el rol "Alumno"
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            // 4. Firmar y crear el Token
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                Expires = DateTime.UtcNow.AddHours(4), // Duración del token
                SigningCredentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256),
                Subject = new ClaimsIdentity(authClaims)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new
            {
                token = tokenHandler.WriteToken(token),
                expiration = token.ValidTo,
                alumno = alumno.Nombre // Opcional: devolver el nombre para mostrarlo en el frontend
            });
        }
    }
}
