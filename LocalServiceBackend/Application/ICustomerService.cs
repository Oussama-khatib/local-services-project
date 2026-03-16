using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trial;

namespace Application
{
    public interface ICustomerService
    {
        Task<Customer> CreateCustomerProfileAsync(int userId, string defaultLocation);
        Task<bool> DeleteCustomerAsync(int customerId);
        Task<Customer?> UpdateCustomerAsync(Customer customer);
        Task<Customer?> GetCustomerByIdAsync(int customerId);
        Task<Customer?> GetCustomerByUserIdAsync(int userId);
        Task IncrementJobsPostedAsync(int customerId);
        Task UpdateTrustScoreAsync(int customerId, string newScore);
    }
}
