using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiProyecto8;

namespace MyApp.Namespace
{
    [Route("api/[controller]")]
    [ApiController]
    public class SaludoController : ControllerBase
    {
        private readonly ISaludoService _saludoService;
        private readonly ISaludoService _saludoFormal;
        private readonly ISaludoService _saludoInformal;

        public SaludoController(
        ISaludoService saludoService,
        [FromKeyedServices("formal")] ISaludoService formal,
        [FromKeyedServices("informal")] ISaludoService informal)
        {
            _saludoService = saludoService;
            _saludoFormal = formal;
            _saludoInformal = informal;
        }

        [HttpGet("{nombre}")]
        public IActionResult GetSaludo(string nombre)
        {
            var mensaje = _saludoService.Saludar(nombre);
            return Ok(mensaje);
        }
        [HttpGet("formal/{nombre}")]
        public IActionResult GetFormal(string nombre)
        {
            var mensaje = _saludoFormal.Saludar(nombre);
            return Ok(mensaje);
        }
        [HttpGet("informal/{nombre}")]
        public IActionResult GetInformal(string nombre)
        {
            var mensaje = _saludoInformal.Saludar(nombre);
            return Ok(mensaje);
        }
    }
}
