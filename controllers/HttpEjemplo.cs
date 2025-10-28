using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MyApp.Namespace
{
    [Route("api/[controller]")]
    [ApiController]
    public class HttpEjemplo : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            using var client = new HttpClient();
            var response = await client.GetAsync("https://jsonplaceholder.typicode.com/users");
            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, "Error al obtener los datos");
            }
            var contenido = await response.Content.ReadAsStringAsync();
            return Ok(contenido);
        }

    }
}
