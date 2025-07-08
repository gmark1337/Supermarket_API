using backend.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace backend.Controllers
{
    [Route("api/flyers")]
    [ApiController]
    public class DatabaseController : ControllerBase
    {
        private readonly ExternalFlyerService _flyerService;
        private readonly MongoDbService _dbService;

        public DatabaseController(ExternalFlyerService flyerService, MongoDbService mongoDbService)
        {
            _flyerService = flyerService;
            _dbService = mongoDbService;
        }

        [HttpPost("import")]
        public async Task<IActionResult> ImportFlyers([FromQuery] string supermarketId)
        {
            var flyers = await _flyerService.FetchFromNodeApi(supermarketId);
            if(flyers == null)
            {
                return BadRequest("No flyers received");
            }
            //Validation
            var actualDate = flyers.FirstOrDefault().ActualDate;
            if (await _dbService.FlyersExistAsync(actualDate))
            {
                return Conflict("Flyers for this supermarket and date already exist ");
            }

            await _dbService.SaveFlyersAsync(flyers, supermarketId);

            return Ok("Flyers imported and save to MongoDb");
        }

        [HttpGet("{supermarketId}")]
        public async Task<IActionResult> GetFlyers(string supermarketId)
        {
            var flyers = await _dbService.GetFlyersAsync(supermarketId);
            return Ok(flyers);
        }

        
    }
}
