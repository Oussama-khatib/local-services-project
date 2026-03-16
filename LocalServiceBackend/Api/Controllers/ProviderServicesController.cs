using Application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Trial;

namespace Api.Controllers
{
    public class CreateProviderServiceDto
    {
        public int CategoryId { get; set; }
        public decimal PriceMin { get; set; }
        public decimal PriceMax { get; set; }
        public string Description { get; set; }
    }

    public class UpdateProviderServiceDto
    {
        public decimal PriceMin { get; set; }
        public decimal PriceMax { get; set; }
        public string Description { get; set; }
    }

    public class ProviderServiceResponseDto
    {
        public int ServiceId { get; set; }
        public int ProviderId { get; set; }
        public int ServiceCategoryId { get; set; }
        public decimal? PriceMin { get; set; }
        public decimal? PriceMax { get; set; }
        public string Description { get; set; }
    }

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProviderServicesController : ControllerBase
    {
        private readonly IProviderServiceService _providerServiceService;
        private readonly IServiceProviderService _providerService;

        public ProviderServicesController(
            IProviderServiceService providerServiceService,
            IServiceProviderService providerService)
        {
            _providerServiceService = providerServiceService;
            _providerService = providerService;
        }

        [Authorize(Roles = "Service Provider")]
        [HttpPost("{providerId}")]
        public async Task<IActionResult> AddService(
        int providerId,
        CreateProviderServiceDto dto)
        {
            var provider = await _providerService.GetProviderByIdAsync(providerId);

            if (provider == null)
                return NotFound("Provider not found");

            int currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            // Ensure provider owns this profile
            if (provider.UserId != currentUserId)
                return Forbid();

            try
            {
                var service = await _providerServiceService.AddServiceToProviderAsync(
                    providerId,
                    dto.CategoryId,
                    dto.PriceMin,
                    dto.PriceMax,
                    dto.Description);

                return Ok(MapToDto(service));
                
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var service = await _providerServiceService.GetProviderServiceByIdAsync(id);

            if (service == null)
                return NotFound();

            var provider = await _providerService.GetProviderByIdAsync(service.ProviderId);

            var role = User.FindFirstValue(ClaimTypes.Role);
            var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            if (role != "Admin" && provider!.UserId != currentUserId)
                return Forbid();

            return Ok(MapToDto(service));
           
        }

        [HttpGet("provider/{providerId}")]
        public async Task<IActionResult> GetByProvider(int providerId)
        {
            var provider = await _providerService.GetProviderByIdAsync(providerId);

            if (provider == null)
                return NotFound();

            var role = User.FindFirstValue(ClaimTypes.Role);
            var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            //if (role != "Admin" && provider.UserId != currentUserId)
            //return Forbid();

            var services = await _providerServiceService.GetServicesByProviderAsync(providerId);

            var categoryService = new ServiceCategoryService();
            List<ServiceCategory> categories = new List<ServiceCategory>();
            foreach (var service in services)
            {
                var category = await categoryService.GetCategoryByIdAsync(service.CategoryId);
                if (category == null) continue;
                categories.Add(category);
            }

            return Ok(services.Select(s => new
            {
                s.ServiceId,
                s.CategoryId,
                CategoryName = categories.FirstOrDefault(c => c.ServiceCategoryId == s.CategoryId)?.Name,
                s.PriceMin,
                s.PriceMax
            }));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateProviderServiceDto dto)
        {
            var existing = await _providerServiceService.GetProviderServiceByIdAsync(id);

            if (existing == null)
                return NotFound();

            var provider = await _providerService.GetProviderByIdAsync(existing.ProviderId);

            var role = User.FindFirstValue(ClaimTypes.Role);
            var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            if (role != "admin" && provider!.UserId != currentUserId)
                return Forbid();

            existing.PriceMin = dto.PriceMin;
            existing.PriceMax = dto.PriceMax;
            existing.Description = dto.Description;
            try
            {
                var updated = await _providerServiceService.UpdateProviderServiceAsync(existing);

                return Ok(MapToDto(updated!));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _providerServiceService.GetProviderServiceByIdAsync(id);

            if (existing == null)
                return NotFound();

            var provider = await _providerService.GetProviderByIdAsync(existing.ProviderId);

            var role = User.FindFirstValue(ClaimTypes.Role);
            var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            if (role != "Admin" && provider!.UserId != currentUserId)
                return Forbid();

            await _providerServiceService.RemoveProviderServiceAsync(id);

            return Ok("Service removed successfully");
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search(
        [FromQuery] int categoryId,
        [FromQuery] string location)
        {
            try
            {
                var providers = await _providerServiceService.SearchProvidersAsync(categoryId, location);

                return Ok(providers);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private ProviderServiceResponseDto MapToDto(ProviderService service)
        {
            return new ProviderServiceResponseDto
            {
                ServiceId = service.ServiceId,
                ProviderId = service.ProviderId,
                ServiceCategoryId = service.CategoryId,
                PriceMin = service.PriceMin,
                PriceMax = service.PriceMax,
                Description = service.Description
            };
        }
    }
}
