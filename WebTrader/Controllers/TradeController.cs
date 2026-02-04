using Microsoft.AspNetCore.Mvc;
using WebTrader.Interfaces;

namespace WebTrader.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class TradeController : Controller
    {
        private readonly ITradeRepository _tradeRepository;

        public TradeController(ITradeRepository tradeRepository)
        {
            this._tradeRepository = tradeRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(string))]
        public IActionResult CalculateTrades(string action, float amount)
        {
            var result = _tradeRepository.CalculateTrade(action, amount);
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }
    }
}
