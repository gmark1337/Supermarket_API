using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("Supermarkets")]
    public class SupermarketController : Controller
    {
        private readonly ISupermarketService _service;
        private readonly ILogger<SupermarketService> _logger;

        public SupermarketController(ISupermarketService service, ILogger<SupermarketService> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpPost]
        public IActionResult Add([FromBody] Supermarkets supermarket)
        {
            var existingmarket = _service.Get(supermarket.Id);

            if (existingmarket != null)
            {
                return Conflict();
            }

            _service.Add(supermarket);
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var existingmarket = _service.Get(id);

            if (existingmarket is null)
            {
                return NotFound();
            }

            _service.Delete(id);
            return Ok();
        }


        [HttpGet]
        public ActionResult<List<Supermarkets>> Get()
        {
            var client = _service.Get();
            return Ok(client);
            
        }

        [HttpGet("{id}")]
        public ActionResult<Supermarkets> Get(int id)
        {
            var client = _service.Get(id);

            if (client is null)
            {
                return NotFound();
            }

            return Ok(client);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Supermarkets supermarket)
        {
            if (id != supermarket.Id)
            {
                return BadRequest();
            }

            var oldMarket = _service.Get(id);

            if(oldMarket is null)
            {
                return NotFound();
            }

            _service.Update(supermarket);
            return Ok();
        }
    }
}
