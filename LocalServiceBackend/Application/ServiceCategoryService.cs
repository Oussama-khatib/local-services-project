using Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trial;

namespace Application
{
    public class ServiceCategoryService : IServiceCategoryService
    {
        public async Task<ServiceCategory> CreateCategoryAsync(string name, string icon)
        {
            ServiceCategoryRepository categoryRepo = new ServiceCategoryRepository();
            var category = new ServiceCategory
            {
                Name = name.Trim(),
                Icon = icon
            };
            await categoryRepo.InsertServiceCategoryAsync(category);
            return category;
        }

        public async Task<bool> DeleteCategoryAsync(int categoryId)
        {
            ServiceCategoryRepository categoryRepo = new ServiceCategoryRepository();
            var category = await categoryRepo.GetByIdAsync(categoryId);
            if (category == null) return false;

            return true;//not implemented
        }

        public async Task<ServiceCategory?> UpdateCategoryAsync(ServiceCategory category)
        {
            ServiceCategoryRepository categoryRepo = new ServiceCategoryRepository();
            var existing = await categoryRepo.GetByIdAsync(category.ServiceCategoryId);
            if (existing == null) return null;

            if (string.IsNullOrWhiteSpace(category.Name))
                throw new Exception("Category name cannot be empty");

            existing.Name = category.Name.Trim();
            existing.Icon = category.Icon;
            await categoryRepo.UpdateServiceCategoryAsync(existing);
            return existing;
        }

        public async Task<IEnumerable<ServiceCategory>> GetAllCategoriesAsync()
        {
            ServiceCategoryRepository categoryRepo = new ServiceCategoryRepository();
            return await categoryRepo.GetAll();
        }

        public async Task<ServiceCategory?> GetCategoryByIdAsync(int categoryId)
        {
            ServiceCategoryRepository categoryRepo = new ServiceCategoryRepository();
            return await categoryRepo.GetByIdAsync(categoryId);
        }
    }
}
