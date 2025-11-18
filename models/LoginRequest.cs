using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MyApp.Namespace
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginRequest
{
    public string Correo { get; set; }
    public string Password { get; set; }
}
}
