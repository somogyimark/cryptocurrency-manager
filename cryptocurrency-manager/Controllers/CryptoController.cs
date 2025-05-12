using cryptocurrency_manager.DataContext.Dtos;
using cryptocurrency_manager.Services;
using Microsoft.AspNetCore.Mvc;

namespace cryptocurrency_manager.Controllers
{
    [ApiController]
    public class CryptoController : ControllerBase
    {
        private readonly ICryptoService _cryptoService;
        public CryptoController(ICryptoService cryptoService)
        {
            _cryptoService = cryptoService;
        }
        [HttpGet]
        [Route("GET/api/cryptos")]
        public async Task<IActionResult> GetAllCryptocurrencies()
        {
            var cryptocurrencies = await _cryptoService.GetAllCryptocurrenciesAsync();
            return Ok(cryptocurrencies);
        }


        [HttpGet]
        [Route("GET/api/cryptos/{cryptoid}")]
        public async Task<IActionResult> GetCryptocurrencyById(int cryptoid)
        {
            var cryptocurrency = await _cryptoService.GetCryptocurrencyByIdAsync(cryptoid);
            if (cryptocurrency == null)
            {
                return NotFound();
            }
            return Ok(cryptocurrency);
        }

        [HttpGet]
        [Route("GET/api/crypto/price/history/{cryptoId}")]
        public async Task<IActionResult> GetCryptocurrencyHistory(int cryptoId)
        {
            var history = await _cryptoService.GetCryptocurrencyHistory(cryptoId);
            if (history == null)
            {
                return NotFound();
            }
            return Ok(history);
        }

        [HttpPost]
        [Route("POST/api/cryptos")]

        public async Task<IActionResult> AddCryptocurrency([FromBody] CryptoCreateDto cryptoCreateDto)
        {
            if (cryptoCreateDto == null)
            {
                return BadRequest("Invalid cryptocurrency data.");
            }
            var createdCrypto = await _cryptoService.AddCryptocurrencyAsync(cryptoCreateDto);
            return CreatedAtAction(nameof(GetAllCryptocurrencies), new { id = createdCrypto.Id }, createdCrypto);
        }

        [HttpPut]
        [Route("PUT /api/crypto/price")]
        public async Task<IActionResult> SetCryptocurrencyPrice(int cryptoid, decimal price)
        {
            var result = await _cryptoService.SetCryptocurrencyPrice(cryptoid, price);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpDelete]
        [Route("DELETE/api/cryptos/{cryptoid}")]
        public async Task<IActionResult> DeleteCryptocurrency(int cryptoid)
        {
            var result = await _cryptoService.DeleteCryptocurrencyAsync(cryptoid);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
