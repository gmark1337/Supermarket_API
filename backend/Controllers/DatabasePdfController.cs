using backend.Data;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/pdfFlyer")]
    public class DatabasePdfController : Controller
    {
        private readonly ExternalPdfService _externalPdfService;
        private readonly MongoDbService _pdfCollectionService;
        public DatabasePdfController(ExternalPdfService externalPdfService, MongoDbService pdfCollectionService)
        {
            _externalPdfService = externalPdfService;
            _pdfCollectionService = pdfCollectionService;
        }



        [HttpPost("import")]
        public async Task<IActionResult>ImportPdf([FromQuery] string supermarketId)
        { 
            var pdf = await _externalPdfService.FetchPdfFromNodeApi(supermarketId);
            if(pdf == null)
            {
                return BadRequest("No Pdf received");
            }

            var actualDate = pdf.ActualDate;
            if(await _pdfCollectionService.FlyerPdfExistAsync(supermarketId, actualDate))
            {
                return Conflict("Pdf for this supermarket already exist in the database!");
            }

            await _pdfCollectionService.SaveFlyerPdfAsync(pdf, supermarketId);
            return Ok("Flyers imported and save to MongoDb");
        }

        [HttpGet("supermarketId")]
        public async Task<IActionResult> GetPdfFlyers([FromQuery]string supermarketId)
        {
            var flyers = await _pdfCollectionService.GetFlyerPdfAsync(supermarketId);
            if(flyers == null)
            {
                return BadRequest();
            }

            return Ok(flyers);
        }


        [HttpGet("ActualDate")]
        public async Task<IActionResult> GetPdfFlyersByActualDate([FromQuery]string actualDate)
        {
            var flyers = await _pdfCollectionService.GetFlyerPdfbyActualDateASync(actualDate);
            if (flyers == null)
            {
                return BadRequest();
            }

            return Ok(flyers);
        }

    }
}
