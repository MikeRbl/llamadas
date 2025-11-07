using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using MiProyecto8; // <-- FIX 3: Agregar using para AlumnoM y AlumnoDTO

namespace MiProyecto8.Controllers
{
    // Esta clase Alumno se usa para la lista estática
    public class Alumno
    {
        public string Nombre { get; set; }
        public int Edad { get; set; }
    }

    [Route("api/alumnos")]
    [ApiController]
    public class AlumnosController : ControllerBase
    {
        // FIX 2: Inicializar la lista con objetos Alumno, no con strings.
        private static List<Alumno> alumnos = new()
        {
            new Alumno { Nombre = "Pedro", Edad = 20 },
            new Alumno { Nombre = "Juan", Edad = 21 },
            new Alumno { Nombre = "Maria", Edad = 22 }
        };

        // FIX 1: Se cambió la ruta para evitar ambigüedad con el Get() de abajo.
        [HttpGet("dto-test")] // Originalmente era [HttpGet] y chocaba con el Get()
        public IActionResult GetAlumno()
        {
            var alumno = new AlumnoM
            {
                Id = 1,
                Nombre = "Juan",
                Correo = "juan@mail.com",
                Password = "secret123" // <-- FIX 4: 'Password' debe ir con P mayúscula
            };
            var dto = new AlumnoDTO(alumno)
            {
                Nombre = alumno.Nombre,
                Correo = alumno.Correo
            };
            return Ok(dto);
        }

        // FIX 5: Se eliminó el [HttpGet("jose")] flotante que estaba aquí.

        // GET: api/alumnos
        [HttpGet] // Esta ruta (GET api/alumnos) ahora es única
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
// <-- FIX 6: Se eliminó el '}' extra que estaba aquí.