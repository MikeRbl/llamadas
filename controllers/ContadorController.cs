using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiProyecto8;

namespace MyApp.Namespace
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContadorController : ControllerBase
    {
        private readonly IContadorService _contador1;
        private readonly IContadorService _contador2;
        private readonly ILlamada _servicioLlamada;

        public ContadorController(IContadorService contador1, IContadorService contador2, ILlamada servicioLlamada)
        {
            _contador1 = contador1;
            _contador2 = contador2;
            _servicioLlamada = servicioLlamada;
        }
        
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new
            {
                PrimerValor = _contador1.ObtenerValor(),
                SegundoValor = _contador2.ObtenerValor()
            });
        }

        [HttpGet("llamadas")]
        public async Task<IActionResult> LlamadasAsync ()
        {
            // Iniciamos las tareas. No usamos 'await' aquí
            // para que se ejecuten en paralelo.
            Task<string> llamada1 = _servicioLlamada.EjecutarLlamadaAsync("Llamada-Rápida", 500);
            Task<string> llamada2 = _servicioLlamada.EjecutarLlamadaAsync("Llamada-Media", 2000);
            Task<string> llamada3 = _servicioLlamada.EjecutarLlamadaAsync("Llamada-Lenta", 3000);

            // Ahora esperamos a que TODAS terminen (como en tu Program.cs)
            await Task.WhenAll(llamada1, llamada2, llamada3);

            // Obtenemos los resultados de las tareas ya completadas
            var resultados = new 
            {
                Respuesta1 = llamada1.Result,
                Respuesta2 = llamada2.Result,
                Respuesta3 = llamada3.Result
            };
            
            // Toda la operación tomará aprox. 3 segundos (la más lenta), no 5.5s
            return Ok(resultados);
        }
        
    }
}