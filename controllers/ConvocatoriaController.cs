using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiProyecto8;

namespace MyApp.Namespace
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConvocatoriaController : ControllerBase
    {
        public readonly AppDbContext _context;
        public ConvocatoriaController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult AgregarConvocatoria([FromBody] Convocatoria convocatoria)
        {
            _context.Convocatorias.Add(convocatoria);
            _context.SaveChanges();
            return Ok("Convocatoria Agregada con exito");
        }
        [HttpGet]
        public IActionResult ObtenerConvocatorias()
        {

            return Ok(_context.Convocatorias.ToList());
        }

    }
}
