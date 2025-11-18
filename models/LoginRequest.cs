using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MyApp.Namespace
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginRequest : ControllerBase
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
    }
}
