using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trial;

namespace Infrastructure
{
    public class JobAssignmentRepository
    {
        public async Task CreateAssignmentAsync(int jobId, int providerId)
        {
            using (var context = new AppDBContext())
            {
                var assignment = new JobAssignment
                {
                    JobId = jobId,
                    ProviderId = providerId,
                    AcceptedAt = null
                };
                await context.JobAssignments.AddAsync(assignment);
                await context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int jobId)
        {
            using (var context = new AppDBContext())
            {
                var jobAssingnment = context.JobAssignments.FirstOrDefault(js=>js.JobId == jobId);
                if (jobAssingnment != null) 
                     context.JobAssignments.Remove(jobAssingnment);
                await context.SaveChangesAsync();
            }
        }

        public async Task AcceptAssignmentAsync(int jobId, int providerId)
        {
            using (var context = new AppDBContext())
            {
                var jobAssignment = context.JobAssignments.FirstOrDefault(js=>js.JobId == jobId);
                if (jobAssignment != null)
                    jobAssignment.AcceptedAt = DateTime.Now;
                await context.SaveChangesAsync();
            }
        }

        public async Task<JobAssignment?> GetByJobAsync(int jobId)
        {
            using (var context = new AppDBContext())
            {
                return await context.JobAssignments
            .FirstOrDefaultAsync(j => j.JobId == jobId);
            }
        }

        public async Task<IEnumerable<JobAssignment>> GetByProviderAsync(int providerId)
        {
            using (var context = new AppDBContext())
            {
                return await context.JobAssignments
            .Where(a => a.ProviderId == providerId)
            .ToListAsync();
            }
        }

        public async Task<bool> CheckIfProviderAlreadyAssignedAsync(int jobId, int providerId)
        {
            using (var context = new AppDBContext())
            {
                return await context.JobAssignments
            .AnyAsync(a => a.JobId == jobId && a.ProviderId == providerId);
            }
        }
    }
}
