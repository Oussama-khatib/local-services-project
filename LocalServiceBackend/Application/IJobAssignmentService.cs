using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trial;

namespace Application
{
    public interface IJobAssignmentService
    {
        Task<JobAssignment> AssignProviderToJobAsync(int jobId, int providerId);
        Task<JobAssignment?> GetAssignmentByJobAsync(int jobId);
        Task<IEnumerable<JobAssignment>?> GetAssignmentsByProviderAsync(int providerId);
    }
}
