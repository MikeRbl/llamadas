using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MiProyecto8.Controllers; // O el namespace donde tengas AppDbContext
using MyApp.Namespace;    // El namespace donde está LoginRequest

namespace MiProyecto8.Controllers // Recomiendo usar el namespace de tu proyecto
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase // Heredar de ControllerBase es buena práctica
    {
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest loginRequest)
        {
            // Lógica de validación de credenciales
            // **REEMPLAZA ESTA LÓGICA POR LA TUYA** (p. ej., consultar la base de datos)
            if (loginRequest.Username != "usuario" || loginRequest.Password != "password")
            {
                return Unauthorized(new { message = "Credenciales inválidas" });
            }

            // Generación del Token JWT
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, loginRequest.Username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                // Puedes agregar más claims, como roles o ID de usuario
                new Claim(ClaimTypes.Role, "Admin")
            };

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                Expires = DateTime.UtcNow.AddHours(3), // El token expira en 3 horas
                SigningCredentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256),
                Subject = new ClaimsIdentity(authClaims)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new
            {
                token = tokenHandler.WriteToken(token),
                expiration = token.ValidTo
            });
        }
    }
}