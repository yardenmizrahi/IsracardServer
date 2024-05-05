using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using server.Services;

namespace server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardController : ControllerBase
    {
        private readonly ICardService _cardService;

        public CardController(ICardService cardService)
        {
            _cardService = cardService;
        }

        [HttpGet]
        public IActionResult GetCards([FromQuery] CardFilter filter)
        {
            try
            {
                var cards = _cardService.GetCards(filter);
                return Ok(cards);
            }
            catch (ServiceException ex)
            {
                // Log the exception
                Console.WriteLine($"An error occurred while retrieving cards: {ex}");

                // Return a 500 Internal Server Error with an error message
                return StatusCode(500, "An error occurred while retrieving cards. Please try again later.");
            }
        }

        [HttpPost("IncreaseCreditLimit")]
        public IActionResult IncreaseCreditLimit([FromBody] IncreaseCreditLimitRequest request)
        {
            Console.WriteLine($"Received request body: {JsonConvert.SerializeObject(request)}");

            var result = _cardService.IncreaseCreditLimit(request);
            if (result.Success)
            {
                Console.WriteLine($"Received request body: {result.Success}");
                return Ok(result.Message);
            }
            return BadRequest(result.Message);
        }
    }
}
