using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trial;

namespace Application
{
    public interface IJobService
    {
        Task<Job> CreateJobAsync(int customerId, int categoryId, string description, string location, bool isEmergency);
        Task<bool> DeleteJobAsync(int jobId);
        Task<Job?> GetJobByIdAsync(int jobId);
        Task<IEnumerable<Job>> GetJobsByCustomerAsync(int customerId);
        Task<IEnumerable<Job>> GetActiveJobsByCustomerAsync(int customerId);
        Task<IEnumerable<Job>> GetOpenJobsByCategoryAsync(int categoryId);
        Task<IEnumerable<Job>> GetProviderJobsAsync(int providerId);
        Task CancelJobAsync(int jobId);
        Task AcceptJobAsync(int jobId, int providerId);
        Task CompleteJobAsync(int jobId, decimal price);
    }
}
