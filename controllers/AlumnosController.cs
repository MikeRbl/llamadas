using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace MiProyecto8.Controllers
{
    public class Alumno
    {
        public string Nombre { get; set; }
        public int Edad { get; set; }
    }

    [Route("api/alumnos")]
    [ApiController]
    public class AlumnosController : ControllerBase
    {
        private static List<Alumno> alumnos = new() { "Pedro", "Juan", "Maria" };
        [HttpGet]
        public IActionResult GetAlumno()
        {
            var alumno = new AlumnoM
            {
                Id = 1,
                Nombre = "Juan",
                Correo = "juan@mail.com",
                password = "secret123"
            };
            var dto = new AlumnoDTO(alumno)
            {
                Nombre = alumno.Nombre,
                Correo = alumno.Correo
            };
            return Ok(dto);
        }

        [HttpGet ("jose")]

        // GET: api/alumnos
        [HttpGet]
        public IEnumerable<Alumno> Get() => alumnos;

        // GET: api/alumnos/0
        [HttpGet("{id}")]
        public ActionResult<Alumno> GetById(int id)
        {
            if (id < 0 || id >= alumnos.Count)
            {
                return NotFound("Alumno no encontrado en ese índice.");
            }
            return Ok(alumnos[id]);
        }

        // POST: api/alumnos
        [HttpPost]
        public IActionResult Post([FromBody] Alumno nuevoAlumno)
        {
            if (nuevoAlumno == null)
            {
                return BadRequest("Datos del alumno no pueden ser nulos.");
            }
            
            alumnos.Add(nuevoAlumno); 
            return CreatedAtAction(nameof(GetById), new { id = alumnos.Count - 1 }, nuevoAlumno);
        }

        // PUT: api/alumnos/0
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Alumno alumnoActualizado)
        {
            if (id < 0 || id >= alumnos.Count)
            {
                return NotFound("Alumno no encontrado .");
            }
            alumnos[id] = alumnoActualizado;
            
            return Ok(alumnos[id]);
        }

        // GET
        [HttpGet("info")]
        public IActionResult Info([FromHeader(Name = "Amor ")] string agente)
        {
            return Ok($"User-Agent: {agente}");          
        }
        // DELETE: api/alumnos/0
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (id < 0 || id >= alumnos.Count)
            {
                return NotFound("Alumno no encontrado para eliminar en ese índice.");
            }

            alumnos.RemoveAt(id);
            return Ok("Alumno eliminado exitosamente.");
        }
    }
}
