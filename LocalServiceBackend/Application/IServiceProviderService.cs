using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trial;

namespace Application
{
    public interface IServiceProviderService
    {
        Task<ServiceProvider> CreateProviderProfileAsync(int userId, string bio, int yearsExp, string location);
        Task<bool> DeleteProviderAsync(int providerId);
        Task<ServiceProvider?> UpdateProviderAsync(ServiceProvider provider);
        Task<ServiceProvider?> GetProviderByIdAsync(int providerId);
        Task<ServiceProvider?> GetProviderByUserIdAsync(int userId);
        Task ActivateProviderAsync(int providerId);
        Task DeactivateProviderAsync(int providerId);
        Task<IEnumerable<ServiceProvider>?> GetProvidersByCategoryAsync(int categoryId);
        Task<IEnumerable<ServiceProvider>?> GetProvidersByLocationAsync(string location);
        Task<IEnumerable<ServiceProvider>> GetTopRatedProvidersAsync(int limit);
        Task UpdateTrustScoreAsync(int providerId, int newScore);
        Task<IEnumerable<ServiceProvider>> GetAllProvidersAsync();
    }
}
