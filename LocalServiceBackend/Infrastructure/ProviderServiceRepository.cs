using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trial;

namespace Infrastructure
{
    public class ProviderServiceRepository
    {
        public async Task InsertProviderServiceAsync(ProviderService providerService)
        {
            using (var context = new AppDBContext())
            {
                var newProviderService = new ProviderService
                {
                    ProviderId = providerService.ProviderId,
                    CategoryId = providerService.CategoryId,
                    PriceMin = providerService.PriceMin,
                    PriceMax = providerService.PriceMax,
                    Description = providerService.Description,
                };
                await context.ProviderServices.AddAsync(newProviderService);
                context.SaveChanges();
            }
        }

        public async Task DeleteProviderServiceAsync(int ProviderServiceId)
        {
            using (var context = new AppDBContext())
            {
                var providerService = context.ProviderServices.FirstOrDefault(ps => ps.ServiceId == ProviderServiceId);
                if (providerService != null)
                {
                    context.ProviderServices.Remove(providerService);
                    context.SaveChanges();
                }
            }
        }

        public async Task UpdateProviderServiceAsync(ProviderService ProviderService)
        {
            using (var context = new AppDBContext())
            {
                var providerService = context.ProviderServices.FirstOrDefault(ps => ps.ServiceId == ProviderService.ServiceId);
                if (providerService != null)
                {
                    
                    providerService.ProviderId=ProviderService.ProviderId;
                    providerService.CategoryId=ProviderService.CategoryId;
                    providerService.PriceMin = ProviderService.PriceMin;
                    providerService.PriceMax = ProviderService.PriceMax;
                    providerService.Description = ProviderService.Description;
                    context.SaveChanges();
                }
            }
        }

        public async Task<ProviderService?> GetByIdAsync(int serviceId)
        {
            using (var context = new AppDBContext())
            {
                //return context.ProviderServices.FirstOrDefault(ps => ps.ServiceId == serviceId);
                return context.ProviderServices.Where(ps => ps.ServiceId == serviceId).First();
            }
        }

        public async Task<IEnumerable<ProviderService>> GetByProviderAsync(int providerId)
        {
            using (var context = new AppDBContext())
            {
                return await context.ProviderServices.Where(ps => ps.ProviderId == providerId).ToListAsync();
            }
        }

        public async Task<IEnumerable<ServiceProvider>> GetProvidersOfferingServiceAsync(int serviceCategoryId, string location)
        {
            using (var context = new AppDBContext())
            {
                return await context.ProviderServices.Where(ps=>ps.CategoryId==serviceCategoryId).Join(context.ServiceProviders, ps => ps.ProviderId, sp => sp.ProviderId,
                    (ps, sp) => sp).Where(sp=>sp.Location==location).ToListAsync();
            }
        }
    }
}
