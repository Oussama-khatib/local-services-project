using Application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TransactionController : Controller
    {
        private readonly ITransactionService _service;
        private readonly IWalletService _walletService;

        public TransactionController(
            ITransactionService service,
            IWalletService walletService)
        {
            _service = service;
            _walletService = walletService;
        }

        [HttpGet("my-transactions")]
        public async Task<IActionResult> GetMyTransactions()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var wallet = await _walletService.GetWalletByUserIdAsync(userId);

            if (wallet == null)
                return NotFound("Wallet not found.");

            var transactions = await _service.GetTransactionsByWalletAsync(wallet.WalletId);

            return Ok(transactions);
        }

        [HttpGet("job/{jobId}")]
        public async Task<IActionResult> GetByJob(int jobId)
        {
            try
            {
                var transactions = await _service.GetTransactionsByJobAsync(jobId);

                if (transactions == null)
                    return NotFound();

                return Ok(transactions);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("type/{type}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetByType(string type)
        {
            var transactions = await _service.GetTransactionsByTypeAsync(type);

            if (transactions == null)
                return NotFound();

            return Ok(transactions);
        }

        [HttpGet("platform-earnings")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetPlatformEarnings()
        {
            var earnings = await _service.GetPlatformEarningsAsync();

            return Ok(new { PlatformEarnings = earnings });
        }
    }
}
