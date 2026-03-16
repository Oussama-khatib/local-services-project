using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trial;

namespace Application
{
    public interface IProviderServiceService
    {
        Task<ProviderService> AddServiceToProviderAsync(int providerId, int categoryId, decimal priceMin, decimal priceMax, string description);
        Task<bool> RemoveProviderServiceAsync(int providerServiceId);
        Task<ProviderService?> UpdateProviderServiceAsync(ProviderService providerService);
        Task<ProviderService?> GetProviderServiceByIdAsync(int serviceId);
        Task<IEnumerable<ProviderService>?> GetServicesByProviderAsync(int providerId);
        Task<IEnumerable<ServiceProvider>> SearchProvidersAsync(int categoryId, string location);
    }
}
