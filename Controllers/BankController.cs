using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using server.Services;

namespace server.Controllers
{
    [Route("api/[controller]")]
    public class BankController : ControllerBase
    {
        private readonly IBankService _bankService;

        public BankController(IBankService bankService)
        {
            _bankService = bankService;
        }

        [HttpGet]
        public IActionResult GetBanks()
        {
            var banks = _bankService.GetBanks();
            return Ok(banks);
        }
    }
}
