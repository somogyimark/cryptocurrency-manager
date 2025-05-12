using cryptocurrency_manager.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace cryptocurrency_manager.Controllers
{
    [ApiController]
    public class TradeController : ControllerBase
    {
        private readonly ITradeService _tradeService;
        public TradeController(ITradeService tradeService)
        {
            _tradeService = tradeService;
        }

        [HttpPost]
        [Route("POST/api/trade/buy")]
        public async Task<IActionResult> Buy(int cryptoId, decimal amount)
        {
            var userId = int.Parse(User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);

            var result = await _tradeService.BuyCryptoAsync(userId, cryptoId, amount);
            return Ok(result);
        }

        [HttpPost]
        [Route("POST/api/trade/sell")]
        public async Task<IActionResult> Sell(int cryptoId, decimal amount)
        {
            var userId = int.Parse(User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);
            var result = await _tradeService.SellCryptoAsync(userId, cryptoId, amount);
            return Ok(result);
        }

        [HttpGet]
        [Route("GET /api/transactions/{userId}")]
        public async Task<IActionResult> GetTradeHistory(int userId)
        {
            var result = await _tradeService.GetUserTransactionsAsync(userId);
            return Ok(result);
        }

        [HttpGet]
        [Route("GET /api/mytransactions")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetMyTradeHistory()
        {
            var userId = int.Parse(User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);
            var result = await _tradeService.GetUserTransactionsAsync(userId);
            return Ok(result);
        }

        [HttpGet]
        [Route("GET/api/transactions/details/{transactionId}")]
        public async Task<IActionResult> GetTransactionDetailsAsync(int transactionId)
        {
            var result = await _tradeService.GetTransactionDetailsAsync(transactionId);
            return Ok(result);
        }
    }
}
