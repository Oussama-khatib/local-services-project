using Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trial;

namespace Application
{
    public class JobAssignmentService : IJobAssignmentService
    {
        public async Task<JobAssignment> AssignProviderToJobAsync(int jobId, int providerId)
        {
            JobRepository jobRepo = new JobRepository();
            var job = await jobRepo.GetByIdAsync(jobId);
            if (job == null)
                throw new Exception("Job not found");

            if (job.Status != "Open")
                throw new Exception("Job is not available for assignment");

            ServiceProviderRepository providerRepo = new ServiceProviderRepository();
            var provider = await providerRepo.GetByIdAsync(providerId);
            if (provider == null)
                throw new Exception("Provider not found");

            JobAssignmentRepository assignmentRepo = new JobAssignmentRepository();
            var alreadyAssigned = await assignmentRepo.CheckIfProviderAlreadyAssignedAsync(jobId, providerId);
            if (alreadyAssigned)
                throw new Exception("Provider already assigned to this job");

            await assignmentRepo.CreateAssignmentAsync(jobId, providerId);
            return await assignmentRepo.GetByJobAsync(jobId) ?? throw new Exception("Assignment failed");
        }

        public async Task<JobAssignment?> GetAssignmentByJobAsync(int jobId)
        {
            JobAssignmentRepository assignmentRepo = new JobAssignmentRepository();
            return await assignmentRepo.GetByJobAsync(jobId);
        }

        public async Task<IEnumerable<JobAssignment>?> GetAssignmentsByProviderAsync(int providerId)
        {
            JobAssignmentRepository assignmentRepo = new JobAssignmentRepository();
            return await assignmentRepo.GetByProviderAsync(providerId);
        }
    }
}
