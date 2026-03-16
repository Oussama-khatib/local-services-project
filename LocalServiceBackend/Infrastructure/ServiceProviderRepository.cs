using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trial;

namespace Infrastructure
{
    public class ServiceProviderRepository
    {
        public async Task InsertServiceProviderAsync(ServiceProvider serviceProvider)
        {
            using (var context = new AppDBContext())
            {
                var newServiceProvider = new ServiceProvider
                {
                    UserId = serviceProvider.UserId,
                    Biography= serviceProvider.Biography,
                    YearOfExperience= serviceProvider.YearOfExperience,
                    Location=serviceProvider.Location,
                    TrustScore = serviceProvider.TrustScore,
                };
                await context.ServiceProviders.AddAsync(newServiceProvider);
                context.SaveChanges();
            }
        }

        public async Task DeleteServiceProviderAsync(int ServiceProviderId)
        {
            using (var context = new AppDBContext())
            {
                var serviceProvider = context.ServiceProviders.FirstOrDefault(sp => sp.ProviderId == ServiceProviderId);
                if (serviceProvider != null)
                {
                    context.ServiceProviders.Remove(serviceProvider);
                    context.SaveChanges();
                }
            }
        }

        public async Task UpdateServiceProviderAsync(ServiceProvider ServiceProvider)
        {
            using (var context = new AppDBContext())
            {
                var serviceProvider = context.ServiceProviders.FirstOrDefault(sp => sp.ProviderId == ServiceProvider.ProviderId);
                if (serviceProvider != null)
                {
                    serviceProvider.UserId = ServiceProvider.UserId;
                    serviceProvider.Biography = ServiceProvider.Biography;
                    serviceProvider.YearOfExperience = ServiceProvider.YearOfExperience;
                    serviceProvider.Location = ServiceProvider.Location;
                    serviceProvider.TrustScore = ServiceProvider.TrustScore;
                    context.SaveChanges();
                }
            }
        }

        public async Task<ServiceProvider?> GetByIdAsync(int providerId)
        {
            using (var context = new AppDBContext())
            {
                return await context.ServiceProviders
            .FirstOrDefaultAsync(p => p.ProviderId == providerId);
            }
        }

        public async Task<ServiceProvider?> GetByUserIdAsync(int userId)
        {
            using (var context = new AppDBContext())
            {
                return await context.ServiceProviders
            .FirstOrDefaultAsync(p => p.UserId == userId);
            }
        }

        public async Task DeactivateAsync(int providerId)
        {
            using (var context = new AppDBContext())
            {
                var provider = await GetByIdAsync(providerId);
                if (provider == null)
                    return;

                UserRepository userRepository = new UserRepository();
                await userRepository.DeactivateAsync(provider.UserId);
            }
        }

        public async Task ActivateAsync(int providerId)
        {
            using (var context = new AppDBContext())
            {
                var provider = await GetByIdAsync(providerId);
                if (provider == null)
                    return;
                UserRepository userRepository = new UserRepository();
                await userRepository.ActivateAsync(provider.UserId);
            }
        }

        public async Task<IEnumerable<ServiceProvider>> GetProvidersByCategoryAsync(int serviceCategoryId)
        {
            using (var context = new AppDBContext())
            {
                var serviceProviders = await 
                    (from sp in context.ServiceProviders join ps in 
                     context.ProviderServices on sp.ProviderId equals ps.ProviderId where 
                     ps.CategoryId == serviceCategoryId select sp).Distinct().ToListAsync();
                return serviceProviders;
            }
        }

        public async Task<IEnumerable<ServiceProvider>> GetProvidersByLocationAsync(string location)
        {
            using (var context = new AppDBContext())
            {
                return await context.ServiceProviders
            .Where(p => p.Location == location)
            .ToListAsync();
            }
        }

        public async Task<IEnumerable<ServiceProvider>> GetTopRatedProvidersAsync(int limit)
        {
            using (var context = new AppDBContext())
            {
                return await context.ServiceProviders
            .OrderByDescending(p => p.TrustScore)
            .Take(limit)
            .ToListAsync();
            }
        }

        public async Task UpdateTrustScoreAsync(int providerId, int newScore)
        {
            using (var context = new AppDBContext())
            {
                var provider = await GetByIdAsync(providerId);
                if (provider == null)
                    return;

                provider.TrustScore = newScore;
                await context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<ServiceProvider>> GetAll()
        {
            using (var context = new AppDBContext())
            {
                var providers = await context.ServiceProviders.ToListAsync();
                return providers;
            }
        }
    }
}
