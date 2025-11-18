using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MiProyecto8.controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParticipantesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ParticipantesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<Participantes>> CrearParticipante([FromBody] Participantes participante)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var convocatoriaExiste = await _context.Convocatorias
                                        .AnyAsync(c => c.ConvocatoriaId == participante.IdConvocatoria);

            if (!convocatoriaExiste)
            {
                return BadRequest(new { mensaje = $"La convocatoria con ID {participante.IdConvocatoria} no existe." });
            }

            _context.Participantes.Add(participante);
            await _context.SaveChangesAsync();

            return Ok(participante);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Participantes>>> GetParticipantes()
        {
            return await _context.Participantes.ToListAsync();
        }
        [HttpGet("buscar")]
public IActionResult Buscar(
    [FromQuery] string termino,
    [FromHeader(Name = "X-Api-Key")] string apiKey
)
{
    return Ok($"Buscando '{termino}' con la ApiKey '{apiKey}'");
}
    }

    
}