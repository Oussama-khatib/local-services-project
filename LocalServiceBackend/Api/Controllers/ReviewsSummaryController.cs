using Application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api.Controllers
{
    public class SummaryResponse { public int id {  get; set; } public int providerId { get; set; } public string? summary { get; set; } }
    
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReviewsSummaryController : ControllerBase
    {
        private readonly IReviewsSummaryService _service;

        public ReviewsSummaryController(IReviewsSummaryService service)
        {
            _service = service;
        }

        [HttpGet("my-summary")]
        [Authorize(Roles = "Service Provider")]
        public async Task<IActionResult> GetMySummary()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            ServiceProviderService service = new ServiceProviderService();
            var provider = await service.GetProviderByUserIdAsync(userId);

            var summary = await _service.GetSummaryByProviderAsync(provider.ProviderId);

            if (summary == null)
                return NotFound("Summary not found.");

            var result = new SummaryResponse
            {
                id = summary.ReviewsSummaryId,
                providerId = summary.ProviderId,
                summary = summary.SummaryText
            };

            return Ok(result);
        }

        [HttpGet("summary")]
        [Authorize]
        public async Task<IActionResult> GetProviderSummary(int providerId)
        {
            try
            {
                var summary = await _service.GetSummaryByProviderAsync(providerId);
                if (summary == null)
                {
                    var res = new SummaryResponse
                    {
                        summary = ""
                    };
                    return Ok(res);
                }
                var result = new SummaryResponse
                {
                    id = summary.ReviewsSummaryId,
                    providerId = summary.ProviderId,
                    summary = summary.SummaryText
                };
                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("generate")]
        [Authorize(Roles = "Service Provider")]
        public async Task<IActionResult> GenerateOrUpdate()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            ServiceProviderService service = new ServiceProviderService();
            var provider = await service.GetProviderByUserIdAsync(userId);
            try
            {
                var summary = await _service.GenerateOrUpdateSummaryAsync(provider.ProviderId);
                var result = new SummaryResponse
                {
                    id = summary.ReviewsSummaryId,
                    providerId = summary.ProviderId,
                    summary = summary.SummaryText
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("delete")]
        [Authorize(Roles = "Service Provider")]
        public async Task<IActionResult> Delete()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            ServiceProviderService service = new ServiceProviderService();
            var provider = await service.GetProviderByUserIdAsync(userId);

            await _service.DeleteSummaryAsync(provider.ProviderId);

            return NoContent();
        }
    }
}
