using AutoMapper;
using cryptocurrency_manager.DataContext.Dtos;
using cryptocurrency_manager.Services;
using DataContext.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace cryptocurrency_manager.Controllers
{
    [ApiController]
    public class WalletController : ControllerBase
    {
        private readonly IWalletService _walletService;
        public WalletController(IWalletService walletService)
        {
            _walletService = walletService;
        }

        [HttpGet]
        [Route("GET/api/wallet/{userId}")]
        public async Task<IActionResult> GetWalletByUserId(int userId)
        {
            var wallets = await _walletService.GetWalletByUserIdAsync(userId);
            if (wallets == null)
            {
                return NotFound();
            }
            return Ok(wallets);
        }

        [HttpGet]
        [Route("GET/api/mywallet")]
        public async Task<IActionResult> GetMyWallet()
        {
            var userId = int.Parse(User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);
            var wallets = await _walletService.GetWalletByUserIdAsync(userId);
            if (wallets == null)
            {
                return NotFound();
            }
            return Ok(wallets);
        }

        [HttpPut]
        [Route("PUT/api/wallet/{userId}")]
        public async Task<IActionResult> UpdateWallet(int userId, [FromBody] WalletUpdateDto walletUpdateDto)
        {
            if (walletUpdateDto == null)
            {
                return BadRequest("Invalid wallet data.");
            }
            var updatedWallet = await _walletService.UpdateWalletAsync(userId, walletUpdateDto);
            if (updatedWallet == null)
            {
                return NotFound();
            }
            return Ok(updatedWallet);
        }

        [HttpPut]
        [Route("PUT/api/mywallet")]
        public async Task<IActionResult> UpdateMyWallet([FromBody] WalletUpdateDto walletUpdateDto)
        {
            var userId = int.Parse(User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);
            if (walletUpdateDto == null)
            {
                return BadRequest("Invalid wallet data.");
            }
            var updatedWallet = await _walletService.UpdateWalletAsync(userId, walletUpdateDto);
            if (updatedWallet == null)
            {
                return NotFound();
            }
            return Ok(updatedWallet);
        }

        [HttpDelete]
        [Route("DELETE/api/wallet/{userId}")]
        public async Task<IActionResult> DeleteWallet(int userId)
        {
            var result = await _walletService.DeleteWalletAsync(userId);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete]
        [Route("DELETE/api/mywallet")]
        public async Task<IActionResult> DeleteMyWallet()
        {
            var userId = int.Parse(User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);
            var result = await _walletService.DeleteWalletAsync(userId);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

    }
}
