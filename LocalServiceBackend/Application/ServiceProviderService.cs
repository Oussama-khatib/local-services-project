using Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Trial;

namespace Application
{
    public class ServiceProviderService : IServiceProviderService
    {
        public async Task<ServiceProvider> CreateProviderProfileAsync(int userId, string bio, int yearsExp, string location)
        {
            ServiceProviderRepository providerRepo = new ServiceProviderRepository();
            UserRepository userRepo = new UserRepository();
            var user = await userRepo.GetByIdAsync(userId);
            if (user == null)
                throw new Exception("User not found");
            if (user.Role != "Service Provider")
                throw new Exception("User is not a service provider");
            var existing = await providerRepo.GetByUserIdAsync(userId);
            if (existing != null)
                throw new Exception("Provider profile already exists");
            var provider = new ServiceProvider
            {
                UserId = userId,
                Biography = bio,
                YearOfExperience = yearsExp,
                Location = location,
                TrustScore = 0
            };
            await providerRepo.InsertServiceProviderAsync(provider);
            return provider;
        }

        public async Task<bool> DeleteProviderAsync(int providerId)
        {
            ServiceProviderRepository providerRepo = new ServiceProviderRepository();
            var provider = await providerRepo.GetByIdAsync(providerId);
            if (provider == null) return false;
            UserRepository userRepo = new UserRepository();
            await userRepo.DeactivateAsync(provider.UserId);
            return true;

        }

        public async Task<ServiceProvider?> UpdateProviderAsync(ServiceProvider provider)
        {
            ServiceProviderRepository providerRepo = new ServiceProviderRepository();
            var existing = await providerRepo.GetByIdAsync(provider.ProviderId);
            if (existing == null) return null;
            existing.Biography = provider.Biography;
            existing.YearOfExperience = provider.YearOfExperience;
            existing.Location = provider.Location;
            await providerRepo.UpdateServiceProviderAsync(existing);
            return existing;
        }

        public async Task<ServiceProvider?> GetProviderByIdAsync(int providerId)
        {
            ServiceProviderRepository providerRepo = new ServiceProviderRepository();
            return await providerRepo.GetByIdAsync(providerId);
        }

        public async Task<ServiceProvider?> GetProviderByUserIdAsync(int userId)
        {
            ServiceProviderRepository providerRepo = new ServiceProviderRepository();
            return await providerRepo.GetByUserIdAsync(userId);
        }

        public async Task ActivateProviderAsync(int providerId)
        {

            ServiceProviderRepository providerRepo = new ServiceProviderRepository();
            var provider = await providerRepo.GetByIdAsync(providerId);
            if (provider == null)
                throw new Exception("Provider not found");

            await providerRepo.ActivateAsync(providerId);
        }

        public async Task DeactivateProviderAsync(int providerId)
        {
            ServiceProviderRepository providerRepo = new ServiceProviderRepository();
            var provider = await providerRepo.GetByIdAsync(providerId);
            if (provider == null)
                throw new Exception("Provider not found");

            await providerRepo.DeactivateAsync(providerId);
        }

        public async Task<IEnumerable<ServiceProvider>?> GetProvidersByCategoryAsync(int categoryId)
        {
            ServiceProviderRepository providerRepo = new ServiceProviderRepository();
            ServiceCategoryRepository categoryRepo = new ServiceCategoryRepository();
            var category= await categoryRepo.GetByIdAsync(categoryId);
            
            if (category == null)
                throw new Exception("Category not found");
            return await providerRepo.GetProvidersByCategoryAsync(categoryId);
        }

        public async Task<IEnumerable<ServiceProvider>?> GetProvidersByLocationAsync(string location)
        {
            ServiceProviderRepository providerRepo = new ServiceProviderRepository();
            return await providerRepo.GetProvidersByLocationAsync(location);
        }

        public async Task<IEnumerable<ServiceProvider>> GetTopRatedProvidersAsync(int limit)
        {
            ServiceProviderRepository providerRepo = new ServiceProviderRepository();
            return await providerRepo.GetTopRatedProvidersAsync(limit);
        }

        public async Task UpdateTrustScoreAsync(int providerId, int newScore)
        {
            ServiceProviderRepository providerRepo = new ServiceProviderRepository();
            var provider = await providerRepo.GetByIdAsync(providerId);
            if (provider == null)
                throw new Exception("Provider not found");
            if (newScore < 0 || newScore > 100)
                throw new Exception("Trust score must be between 0 and 100");
            await providerRepo.UpdateTrustScoreAsync(providerId, newScore);

        }

        public async Task<IEnumerable<ServiceProvider>> GetAllProvidersAsync()
        {
            ServiceProviderRepository providerRepo = new ServiceProviderRepository();
            var providers = await providerRepo.GetAll();
            return providers;
        }

    }
}
