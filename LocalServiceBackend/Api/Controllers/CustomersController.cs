using Microsoft.AspNetCore.Mvc;
using Application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Trial;


namespace Api.Controllers
{
    public class CreateCustomerDto
    {
        public string DefaultLocation { get; set; }
    }

    public class UpdateCustomerDto
    {
        public string DefaultLocation { get; set; }
    }
    public class CustomerResponseDto
    {
        public int CustomerId { get; set; }
        public int UserId { get; set; }
        public string DefaultLocation { get; set; }
        public string TotalJobPosted { get; set; }
        public string TrustScore { get; set; }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }
        [Authorize(Roles = "Customer")]
        [HttpPost]
        public async Task<IActionResult> CreateProfile(CreateCustomerDto dto)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            try
            {
                var customer = await _customerService.CreateCustomerProfileAsync(userId, dto.DefaultLocation);
                return Ok(new CustomerResponseDto
                {
                    CustomerId = customer.CustomerId,
                    UserId = customer.UserId,
                    DefaultLocation = customer.DefaultLocation,
                    TotalJobPosted = customer.TotalJobPosted,
                    TrustScore = customer.TrustScore
                });
            }
            catch (Exception e) {
                return BadRequest("Customer creation failed because: " + e.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var customer = await _customerService.GetCustomerByIdAsync(id);

            if (customer == null)
                return NotFound();

            var userRole = User.FindFirstValue(ClaimTypes.Role);
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            // If not admin, must own the profile
            if (userRole != "Admin" && customer.UserId != userId)
                return Forbid();

            return Ok(new CustomerResponseDto
            {
                CustomerId = customer.CustomerId,
                UserId = customer.UserId,
                DefaultLocation = customer.DefaultLocation,
                TotalJobPosted = customer.TotalJobPosted,
                TrustScore = customer.TrustScore
            });
        }

        [HttpGet("by-user/{userId}")]
        public async Task<IActionResult> GetByUserId(int userId)
        {
            var customer = await _customerService.GetCustomerByUserIdAsync(userId);

            if (customer == null)
                return NotFound();

            var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var role = User.FindFirstValue(ClaimTypes.Role);

            if (role != "Admin" && currentUserId != userId)
                return Forbid();

            return Ok(new CustomerResponseDto
            {
                CustomerId = customer.CustomerId,
                UserId = customer.UserId,
                DefaultLocation = customer.DefaultLocation,
                TotalJobPosted = customer.TotalJobPosted,
                TrustScore = customer.TrustScore
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateCustomerDto dto)
        {
            var existing = await _customerService.GetCustomerByIdAsync(id);

            if (existing == null)
                return NotFound();

            var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var role = User.FindFirstValue(ClaimTypes.Role);

            if (role != "Admin" && existing.UserId != currentUserId)
                return Forbid();

            existing.DefaultLocation = dto.DefaultLocation;

            var updated = await _customerService.UpdateCustomerAsync(existing);

            return Ok(new CustomerResponseDto
            {
                CustomerId = updated!.CustomerId,
                UserId = updated.UserId,
                DefaultLocation = updated.DefaultLocation,
                TotalJobPosted = updated.TotalJobPosted,
                TrustScore = updated.TrustScore
            });
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _customerService.DeleteCustomerAsync(id);

            if (!result)
                return NotFound();

            return Ok("Customer deleted successfully");
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}/trust-score")]
        public async Task<IActionResult> UpdateTrustScore(int id, [FromBody] string newScore)
        {
            await _customerService.UpdateTrustScoreAsync(id, newScore);
            return Ok("Trust score updated");
        }

    }
}
