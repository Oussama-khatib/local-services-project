using Application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Trial;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WalletController : ControllerBase
    {
        private readonly IWalletService _service;

        public WalletController(IWalletService service)
        {
            _service = service;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateWallet()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            try
            {
                var wallet = await _service.CreateWalletAsync(userId);

                return Ok(wallet);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("my-wallet")]
        public async Task<IActionResult> GetMyWallet()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            try
            {
                var wallet = await _service.GetWalletByUserIdAsync(userId);

                if (wallet == null)
                    return NotFound("Wallet not found.");

                return Ok(wallet);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("userName/{walletId}")]
        public async Task<IActionResult> GetUserName(int walletId)
        {
            try
            {
                var userName = await _service.GetUserNameByWalletId(walletId);
                return Ok(userName);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("deposit")]
        public async Task<IActionResult> Deposit(decimal amount)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var wallet = await _service.GetWalletByUserIdAsync(userId);

            if (wallet == null)
                return NotFound("Wallet not found.");
            try
            {
                await _service.DepositAsync(wallet.WalletId, amount);

                return Ok("Deposit successful.");
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
    }
}
