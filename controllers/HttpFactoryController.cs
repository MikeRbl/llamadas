using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MyApp.Namespace
{
    [Route("api/[controller]")]
    [ApiController]
    public class HttpFactoryController : ControllerBase
    {
        private readonly HttpClient _httpclient;
        public HttpFactoryController(IHttpClientFactory httpClientFactory)
        {
            _httpclient = httpClientFactory.CreateClient("jsonplaceholder");
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var response = await _httpclient.GetAsync("users");
            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, "Error al obtener los datos");
            }
            var contenido = await response.Content.ReadAsStringAsync();
            return Ok(contenido);
        }        
    }
}
