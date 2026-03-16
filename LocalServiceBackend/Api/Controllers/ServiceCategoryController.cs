using Application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Trial;

namespace Api.Controllers
{
    public class UpdateRequest { public string Name { get; set; } public string Icon { get; set; } }
    public class CategoryResponse { public int ServiceCategoryId { get; set; } public string Name { get; set; } public string Icon { get; set; } }

    [Route("api/[controller]")]
    [ApiController]
    public class ServiceCategoryController : ControllerBase
    {
        private readonly IServiceCategoryService _service;

        public ServiceCategoryController(IServiceCategoryService service)
        {
            _service = service;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(string name, string icon)
        {
            var category = await _service.CreateCategoryAsync(name, icon);
            return Ok("Category added");
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _service.GetAllCategoriesAsync();
            var result = categories.Select(c => new CategoryResponse
            {
                ServiceCategoryId = c.ServiceCategoryId,
                Name = c.Name,
                Icon = c.Icon
            });
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await _service.GetCategoryByIdAsync(id);

            if (category == null)
                return NotFound($"Category with ID {id} not found.");
            var response= new CategoryResponse { ServiceCategoryId = id , Name=category.Name , Icon = category.Icon};

            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateRequest updateRequest)
        {
            var category = new ServiceCategory { ServiceCategoryId = id,Name = updateRequest.Name, Icon = updateRequest.Icon };
            try
            {
                var updatedCategory = await _service.UpdateCategoryAsync(category);

                if (updatedCategory == null)
                    return NotFound($"Category with ID {id} not found.");

                return Ok(updatedCategory);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteCategoryAsync(id);

            if (!result)
                return NotFound($"Category with ID {id} not found.");

            return NoContent();
        }
    }
}
