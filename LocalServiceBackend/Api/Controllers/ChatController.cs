using Application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api.Controllers
{
    public class ChatRequest
    {
        public string Message { get; set; }
    }

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ChatController : ControllerBase
    {
        private readonly IAiService _AiService;

        public ChatController(IAiService aiService)
        {
            _AiService = aiService;
        }

        [HttpPost]
        public async Task<IActionResult> Chat([FromBody] ChatRequest request)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var role = User.FindFirst(ClaimTypes.Role)!.Value;
            try
            {
                var response = await _AiService.ProcessQuestionAsync(userId, role, request.Message);

                return Ok(new { message = response });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
