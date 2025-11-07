using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace MyApp.Namespace
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigController : ControllerBase
    {
        private readonly IConfiguration _config;

        public ConfigController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var urlUsuarios = _config["ExternalApis:Usuarios"];
            var urlPosts = _config["ExternalApis:Posts"];

            return Ok(new { urlUsuarios, urlPosts });
        }
    }
}
