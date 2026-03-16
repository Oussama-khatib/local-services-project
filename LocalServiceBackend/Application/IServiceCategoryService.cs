using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trial;

namespace Application
{
    public interface IServiceCategoryService
    {
        Task<ServiceCategory> CreateCategoryAsync(string name, string icon);
        Task<bool> DeleteCategoryAsync(int categoryId);
        Task<ServiceCategory?> UpdateCategoryAsync(ServiceCategory category);
        Task<IEnumerable<ServiceCategory>> GetAllCategoriesAsync();
        Task<ServiceCategory?> GetCategoryByIdAsync(int categoryId);
    }
}
