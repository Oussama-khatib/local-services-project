using Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trial;

namespace Application
{
    public class ProviderServiceService : IProviderServiceService
    {
        public async Task<ProviderService> AddServiceToProviderAsync(int providerId,int categoryId,decimal priceMin,decimal priceMax,string description)
        {
            ServiceProviderRepository providerRepo = new ServiceProviderRepository();
            var provider = await providerRepo.GetByIdAsync(providerId);
            if (provider == null)
                throw new Exception("Provider not found");

            if (priceMin < 0 || priceMax < 0 || priceMin > priceMax)
                throw new Exception("Invalid price range");

            ServiceCategoryRepository categoryRepo = new ServiceCategoryRepository();
            var category= await categoryRepo.GetByIdAsync(categoryId);
            if (category == null)
                throw new Exception("Category not found");

            var service = new ProviderService
            {
                ProviderId = providerId,
                CategoryId = categoryId,
                PriceMin = priceMin,
                PriceMax = priceMax,
                Description = description
            };
            ProviderServiceRepository providerServiceRepo = new ProviderServiceRepository();
            await providerServiceRepo.InsertProviderServiceAsync(service);
            return service;
        }

        public async Task<bool> RemoveProviderServiceAsync(int providerServiceId)
        {
            ProviderServiceRepository providerServiceRepo = new ProviderServiceRepository();
            var existing = await providerServiceRepo.GetByIdAsync(providerServiceId);
            if (existing == null) return false;

            await providerServiceRepo.DeleteProviderServiceAsync(providerServiceId);
            return true;
        }

        public async Task<ProviderService?> UpdateProviderServiceAsync(ProviderService providerService)
        {
            ProviderServiceRepository providerServiceRepo = new ProviderServiceRepository();
            var existing = await providerServiceRepo.GetByIdAsync(providerService.ServiceId);
            if (existing == null) return null;

            if (providerService.PriceMin < 0 || providerService.PriceMax < 0 || providerService.PriceMin > providerService.PriceMax)
                throw new Exception("Invalid price range");
            existing.PriceMin = providerService.PriceMin;
            existing.PriceMax = providerService.PriceMax;
            existing.Description = providerService.Description;

            await providerServiceRepo.UpdateProviderServiceAsync(existing);
            return existing;
        }

        public async Task<ProviderService?> GetProviderServiceByIdAsync(int serviceId)
        {
            ProviderServiceRepository providerServiceRepo = new ProviderServiceRepository();
            return await providerServiceRepo.GetByIdAsync(serviceId);
        }

        public async Task<IEnumerable<ProviderService>?> GetServicesByProviderAsync(int providerId)
        {
            ProviderServiceRepository providerServiceRepo = new ProviderServiceRepository();
            return await providerServiceRepo.GetByProviderAsync(providerId);
        }

        public async Task<IEnumerable<ServiceProvider>> SearchProvidersAsync(int categoryId, string location)
        {
            ProviderServiceRepository providerServiceRepo = new ProviderServiceRepository();
            ServiceCategoryRepository categoryRepo = new ServiceCategoryRepository();
            var category = await categoryRepo.GetByIdAsync(categoryId);
            if (category == null)
                throw new Exception("Category not found");
            return await providerServiceRepo.GetProvidersOfferingServiceAsync(categoryId, location);
        }

    }
}
