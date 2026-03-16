using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Trial;

namespace Infrastructure
{
    public class JobRepository
    {
        public async Task<Job> InsertJobAsync(Job job)
        {
            using (var context = new AppDBContext())
            {
                var newJob = new Job
                {
                    CustomerId= job.CustomerId,
                    ServiceCategoryId= job.ServiceCategoryId,
                    Description = job.Description,
                    Location = job.Location,
                    IsEmergency = job.IsEmergency,
                    Status = job.Status,
                    CreatedAt=DateTime.Now,
                    AcceptedAt=null,
                    CompletedAt=null,
                };
                await context.Jobs.AddAsync(newJob);
                context.SaveChanges();
                return newJob;
            }
        }

        public async Task DeleteJobAsync(int JobId)
        {
            using (var context = new AppDBContext())
            {
                var job = context.Jobs.FirstOrDefault(j => j.JobId == JobId);
                if (job != null)
                {
                    context.Jobs.Remove(job);
                    context.SaveChanges();
                }
            }
        }

        public async Task UpdateJobAsync(Job Job)
        {
            using (var context = new AppDBContext())
            {
                var job = context.Jobs.FirstOrDefault(j => j.JobId == Job.JobId);
                if (job != null)
                {
                    job.CustomerId = Job.CustomerId;
                    job.ServiceCategoryId = Job.ServiceCategoryId;
                    job.Description = Job.Description;
                    job.Location = Job.Location;
                    job.IsEmergency = Job.IsEmergency;
                    job.Status = Job.Status;
                    job.AcceptedAt = Job.AcceptedAt;
                    job.CompletedAt = Job.CompletedAt;
                    context.SaveChanges();
                }
            }
        }

        public async Task<Job?> GetByIdAsync(int jobId)
        {
            using (var context = new AppDBContext()) 
            {
                return await context.Jobs.FirstOrDefaultAsync(j=> j.JobId == jobId);
            }
        }

        public async Task CancelAsync(int jobId)
        {
            using (var context = new AppDBContext())
            {
                var job = await context.Jobs.FirstOrDefaultAsync(j => j.JobId == jobId);
                if (job == null)
                    return;
                job.Status = "Cancelled";
                await context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Job>> GetJobsByCustomerAsync(int customerId)
        {
            using (var context = new AppDBContext())
            {
                var jobs = await context.Jobs.Where(j=>j.CustomerId == customerId).ToListAsync();
                return jobs;
            }
        }

        public async Task<IEnumerable<Job>> GetActiveJobsByCustomerAsync(int customerId)
        {
            using (var context = new AppDBContext())
            {
                var jobs= await context.Jobs
            .Where(j => j.CustomerId == customerId &&
                       (j.Status == "Open" || j.Status == "Accepted"))
            .ToListAsync();
                return jobs;
            }
        }

        public async Task<IEnumerable<Job>> GetOpenJobsByCategoryAsync(int serviceCategoryId)
        {
            using (var context = new AppDBContext())
            {
                return await context.Jobs
            .Where(j => j.ServiceCategoryId == serviceCategoryId &&
                        j.Status == "Open")
            .ToListAsync();
            }
        }

        public async Task<IEnumerable<Job>> GetProviderJobsAsync(int providerId)
        {
            using (var context = new AppDBContext())
            {
                
                var jobs = await (from j in context.Jobs join ja in context.JobAssignments 
                                  on j.JobId equals ja.JobId where ja.ProviderId == providerId select j)
                    .Distinct().ToListAsync();
                return jobs;

                /*return await context.Jobs
            .Where(j => j.IsEmergency== true && j.Status == "Open")
            .OrderByDescending(j => j.CreatedAt)
            .ToListAsync();*/
            }
        }

        public async Task MarkAsAcceptedAsync(int jobId)
        {
            using (var context = new AppDBContext())
            {
                var job = await context.Jobs.FirstOrDefaultAsync(j => j.JobId == jobId);
                if (job == null)
                    return;
                job.Status = "Accepted";
                job.AcceptedAt = DateTime.Now;
                await context.SaveChangesAsync();
            }
        }

        public async Task MarkAsCompletedAsync(int jobId)
        {
            using (var context = new AppDBContext())
            {
                var job = await context.Jobs.FirstOrDefaultAsync(j => j.JobId == jobId);
                if (job == null)
                    return;
                job.Status = "Completed";
                job.CompletedAt = DateTime.Now;
                await context.SaveChangesAsync();
            }
        }


    }
}
