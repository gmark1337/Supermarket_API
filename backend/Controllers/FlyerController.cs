using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FlyerController : ControllerBase
    {
        private readonly IFlyerService _flyerService;
        private readonly HttpClient _httpClient;
        private readonly ILogger<FlyerService> _logger;
        

        public FlyerController(IHttpClientFactory httpClientFactory, IFlyerService service, ILogger<FlyerService> logger)
        {
            
            _httpClient = httpClientFactory.CreateClient();
            _flyerService = service;
            _logger = logger;
        }

        [HttpPost("refresh/{supermarketId}")]
        public async Task<IActionResult> RefreshData(int supermarketId)
        {
            var respone = await _httpClient.GetAsync($"http://localhost:3000/api/data?supermarketId={supermarketId}");
            if (!respone.IsSuccessStatusCode)
            {
                return StatusCode((int)respone.StatusCode, "Failed to fetch data from Node.js");
            }


            var content = await respone.Content.ReadAsStringAsync();
            _logger.LogInformation(content);
            
            if(string.IsNullOrEmpty(content))
            {
                return BadRequest("No data received");
            }

            NodeResponse nodeResponse;
            try
            {
                nodeResponse = JsonSerializer.Deserialize<NodeResponse>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch (JsonException e )
            {
                return BadRequest($"Invalid JSON format: {e.Message} ");
            }
            _logger.LogInformation($"Node response: {nodeResponse.data}");

            if (nodeResponse == null || nodeResponse.data == null)
            {
                return BadRequest("Failed to parse data");
            }
            
            List<Flyer> pages = nodeResponse.data.Pages;
            foreach(var flyer in pages) 
            {
                flyer.SupermarketID = supermarketId;
            }
            _flyerService.SetAll(pages);

            return Ok("Data refreshed");
        }

   

        [HttpGet]
        public ActionResult<List<Flyer>> Get()
        {
            var item = _flyerService.Get();
            return item == null ? NotFound() : Ok(item);
        }

    }
}
