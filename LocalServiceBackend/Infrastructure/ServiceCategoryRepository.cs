using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trial;

namespace Infrastructure
{
    public class ServiceCategoryRepository
    {
        public async Task InsertServiceCategoryAsync(ServiceCategory serviceCategory)
        {
            using (var context = new AppDBContext())
            {
                var newServiceCategory = new ServiceCategory
                {
                    Name = serviceCategory.Name,
                    Icon=serviceCategory.Icon,
                };
                await context.ServiceCategories.AddAsync(newServiceCategory);
                context.SaveChanges();
            }
        }

        public async Task DeleteServiceCategoryAsync(int ServiceCategoryId)
        {
            using (var context = new AppDBContext())
            {
                var serviceCategory = context.ServiceCategories.FirstOrDefault(sc => sc.ServiceCategoryId == ServiceCategoryId);
                if (serviceCategory != null)
                {
                    context.ServiceCategories.Remove(serviceCategory);
                    context.SaveChanges();
                }
            }
        }

        public async Task UpdateServiceCategoryAsync(ServiceCategory ServiceCategory)
        {
            using (var context = new AppDBContext())
            {
                var serviceCategory = context.ServiceCategories.FirstOrDefault(sc => sc.ServiceCategoryId == ServiceCategory.ServiceCategoryId);
                if (serviceCategory != null)
                {
                    serviceCategory.Name= ServiceCategory.Name;
                    serviceCategory.Icon= ServiceCategory.Icon;
                    context.SaveChanges();
                }
            }
        }

        public async Task<IEnumerable<ServiceCategory>> GetAll()
        {
            using (var context = new AppDBContext())
            {
                var serviceCategories = await context.ServiceCategories.ToListAsync();
                return serviceCategories;
            }
        }

        public async Task<ServiceCategory?> GetByIdAsync(int serviceCategoryId)
        {
            using (var context = new AppDBContext())
            {
                return await context.ServiceCategories
                             .FirstOrDefaultAsync(c => c.ServiceCategoryId == serviceCategoryId);
            }
        }
    }
    

}
