using backend.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using ZstdSharp.Unsafe;

namespace backend.Controllers
{
    [Route("api/flyers")]
    [ApiController]
    public class DatabaseController : ControllerBase
    {
        private readonly ExternalFlyerService _flyerService;
        private readonly MongoDbService _dbService;
        private readonly BlobService _blobService;

        public DatabaseController(ExternalFlyerService flyerService, MongoDbService mongoDbService, BlobService blobService)
        {
            _flyerService = flyerService;
            _dbService = mongoDbService;
            _blobService = blobService;
        }

        [HttpPost("import")]
        public async Task<IActionResult> ImportFlyers([FromQuery] string supermarketId)
        {
            var flyers = await _flyerService.FetchFromNodeApi(supermarketId);
            if(flyers == null)
            {
                return BadRequest("No flyers received");
            }
            var serviceType = flyers.FirstOrDefault().ServiceType;
            var actualDate = flyers.FirstOrDefault().ActualDate;
            if (serviceType == "saveToDb") {
                if(await _dbService.FlyersExistAsync(actualDate, supermarketId))
                {
                    return Conflict("Flyers for this supermarket and date already exist ");
                }
                await _dbService.SaveFlyersAsync(flyers, supermarketId);
            }
            else if(serviceType == "saveToCloudFlare")
            {
                if(await _dbService.FlyersExistAsync(actualDate, supermarketId))
                {
                    return Conflict("Flyers for this supermarket already exist in the cloud!");
                }
                await _blobService.UploadBlobIntoCloud(flyers);
               
                List<string> cloudURLs = await _blobService.FetchCloudFlareURLAsync(supermarketId, actualDate);
                foreach(var url in cloudURLs)
                {
                    var filterPageIndex = Regex.Match(url, @"/(\d+)\.jpe?g");
                    if (filterPageIndex.Success)
                    {
                        var pageIndex = filterPageIndex.Groups[1].Value;
                        if(int.Parse(pageIndex) == flyers.FirstOrDefault().PageIndex)
                        {
                            flyers.FirstOrDefault().ImageURL = url;
                        }
                    }
                }
                await _dbService.SaveFlyersAsync(flyers, supermarketId);
                
                return Ok("Flyers imported and save to CloudFlare");
            }

                //Validation
                //var actualDate = flyers.FirstOrDefault().ActualDate;
                //if (await _dbService.FlyersExistAsync(actualDate, supermarketId))
                //{
                //    return Conflict("Flyers for this supermarket and date already exist ");
                //}

                //await _dbService.SaveFlyersAsync(flyers, supermarketId);

                return Ok("Flyers imported and saved to MongoDb");
        }

        [HttpGet("supermarketId")]
        public async Task<IActionResult> GetFlyers([FromQuery] string supermarketId)
        {
            var flyers = await _dbService.GetFlyersAsync(supermarketId);
            if (flyers == null)
            {
                return BadRequest();
            }
            return Ok(flyers);
        }


        [HttpGet("pageIndex")]
        public async Task<ActionResult> GetPages(int pageIndex, string supermarketID)
        {
            var flyers = await _dbService.GetPageAsnyc(pageIndex, supermarketID);
            if (flyers == null)
            {
                return NotFound();
            }

            return Ok(flyers);
        }
        [HttpGet("ActualDate")]
        public async Task<ActionResult> GetWeeklyFlyerPages([FromQuery] string actualDate)
        {
            var flyers = await _dbService.GetFlyersByActualDateAsync(actualDate);
            if(flyers == null)
            {
                return BadRequest();
            }
            return Ok(flyers);
        }
        
    }
}
