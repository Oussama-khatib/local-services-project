using Application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;

namespace Api.Controllers
{
    public class CreateProviderDto
    {
        public string Biography { get; set; }
        public int YearsOfExperience { get; set; }
        public string Location { get; set; }
    }

    public class UpdateProviderDto
    {
        public string Biography { get; set; }
        public int YearOfExperience { get; set; }
        public string Location { get; set; }
    }

    public class ProviderResponseDto
    {
        public int ProviderId { get; set; }
        public int UserId { get; set; }
        public string Biography { get; set; }
        public int? YearOfExperience { get; set; }
        public string Location { get; set; }
        public int? TrustScore { get; set; }
    }

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ServiceProvidersController : ControllerBase
    {
        private readonly IServiceProviderService _providerService;

        public ServiceProvidersController(IServiceProviderService providerService)
        {
            _providerService = providerService;
        }

        [Authorize(Roles = "Service Provider")]
        [HttpPost]
        public async Task<IActionResult> CreateProfile(CreateProviderDto dto)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            try
            {
                var provider = await _providerService.CreateProviderProfileAsync(
                    userId,
                    dto.Biography,
                    dto.YearsOfExperience,
                    dto.Location);
                var providerResponseDto = new ProviderResponseDto
                {
                    ProviderId = provider.ProviderId,
                    UserId = provider.UserId,
                    Biography = provider.Biography,
                    YearOfExperience = provider.YearOfExperience,
                    Location = provider.Location,
                    TrustScore = provider.TrustScore
                };

                return Ok(providerResponseDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet()]
        public async Task<IActionResult> GetProviders()
        {
            var providers = await _providerService.GetAllProvidersAsync();

            var userService= new UserService();
            var providersWithUserDetails = new List<object>();
            foreach (var provider in providers)
            {
                var user = await userService.GetUserByIdAsync(provider.UserId);

                // Combine the info
                providersWithUserDetails.Add(new
                {
                    providerId = provider.ProviderId,
                    biography = provider.Biography,
                    yearOfExperience = provider.YearOfExperience,
                    location = provider.Location,
                    trustScore = provider.TrustScore,
                    fullName = user.FullName,
                    phoneNumber = user.PhoneNumber
                });
            }
            return Ok(providersWithUserDetails);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var provider = await _providerService.GetProviderByIdAsync(id);

            if (provider == null)
                return NotFound();

            var role = User.FindFirstValue(ClaimTypes.Role);
            var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            if (role != "Admin" && provider.UserId != currentUserId)
                return Forbid();
            var providerResponseDto = new ProviderResponseDto
            {
                ProviderId = provider.ProviderId,
                UserId = provider.UserId,
                Biography = provider.Biography,
                YearOfExperience = provider.YearOfExperience,
                Location = provider.Location,
                TrustScore = provider.TrustScore
            };
            return Ok(providerResponseDto);
        }

        [HttpGet("by-user/{userId}")]
        public async Task<IActionResult> GetByUserId(int userId)
        {
            var provider = await _providerService.GetProviderByUserIdAsync(userId);

            if (provider == null)
                return NotFound();

            var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var role = User.FindFirstValue(ClaimTypes.Role);

            if (role != "Admin" && currentUserId != userId)
                return Forbid();

            var providerResponseDto = new ProviderResponseDto
            {
                ProviderId = provider.ProviderId,
                UserId = provider.UserId,
                Biography = provider.Biography,
                YearOfExperience = provider.YearOfExperience,
                Location = provider.Location,
                TrustScore = provider.TrustScore
            };
            return Ok(providerResponseDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateProviderDto dto)
        {
            var existing = await _providerService.GetProviderByIdAsync(id);

            if (existing == null)
                return NotFound();

            var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var role = User.FindFirstValue(ClaimTypes.Role);

            if (role != "Admin" && existing.UserId != currentUserId)
                return Forbid();

            existing.Biography = dto.Biography;
            existing.YearOfExperience = dto.YearOfExperience;
            existing.Location = dto.Location;

            var updated = await _providerService.UpdateProviderAsync(existing);
            var providerResponseDto = new ProviderResponseDto
            {
                ProviderId = updated.ProviderId,
                UserId = updated.UserId,
                Biography = updated.Biography,
                YearOfExperience = updated.YearOfExperience,
                Location = updated.Location,
                TrustScore = updated.TrustScore
            };
            return Ok(providerResponseDto);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _providerService.DeleteProviderAsync(id);

            if (!result)
                return NotFound();

            return Ok("Provider deleted successfully");
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}/activate")]
        public async Task<IActionResult> Activate(int id)
        {
            try
            {
                await _providerService.ActivateProviderAsync(id);
                return Ok("Provider activated");
            }
            catch (Exception ex) 
            { 
               return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}/deactivate")]
        public async Task<IActionResult> Deactivate(int id)
        {
            try
            {
                await _providerService.DeactivateProviderAsync(id);
                return Ok("Provider deactivated");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("category/{categoryId}")]
        public async Task<IActionResult> GetByCategory(int categoryId)
        {
            try
            {
                var providers = await _providerService.GetProvidersByCategoryAsync(categoryId);
               
                return Ok(providers?.Select(MapToDto));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("location/{location}")]
        public async Task<IActionResult> GetByLocation(string location)
        {
            try
            {
                var providers = await _providerService.GetProvidersByLocationAsync(location);
                return Ok(providers?.Select(MapToDto));
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }  
        }

        [HttpGet("top-rated")]
        public async Task<IActionResult> GetTopRated([FromQuery] int limit = 5)
        {
            var providers = await _providerService.GetTopRatedProvidersAsync(limit);

            return Ok(providers.Select(MapToDto));
        }

        private ProviderResponseDto MapToDto(Trial.ServiceProvider provider)
        {      
            return new ProviderResponseDto
            {
                ProviderId = provider.ProviderId,
                UserId = provider.UserId,
                Biography = provider.Biography,
                YearOfExperience = provider.YearOfExperience,
                Location = provider.Location,
                TrustScore = provider.TrustScore
            };
        }

    }

}
