using Amazon.Runtime.Internal.Util;
using backend.Data;
using backend.Model;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/feedback")]
    public class FeedbackController : Controller
    {
        private readonly MongoDbService _dbService;
        private readonly ILogger<FeedbackController> _logger;

        public FeedbackController(MongoDbService service, ILogger<FeedbackController> logger)
        {
            _dbService = service;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> ReceivePages([FromBody] Feedback feedback)
        {
            _logger.LogInformation($"User feedback received... \n " +
                $"Username:{feedback.Username} \n" +
                $" Message:{feedback.FeedBackText}");

            if (feedback == null)
            {
                return BadRequest();
            }

            await _dbService.SaveFeedbackAsync(feedback);
            _logger.LogInformation("Feedback added successfully!");

            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult> GetFeedback(string id)
        {
            var filter = await _dbService.GetFeedbackAsync(id);

            if(filter == null)
            {
                return NotFound();
            }
            return Ok(filter);
        }
    }
}
