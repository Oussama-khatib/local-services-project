using Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trial;

namespace Application
{
    public class JobService : IJobService
    {
        public async Task<Job> CreateJobAsync(int customerId, int categoryId, string description, string location, bool isEmergency)
        {
            CustomerRepository customerRepo = new CustomerRepository();
            var customer = await customerRepo.GetByIdAsync(customerId);
            if (customer == null)
                throw new Exception("Customer not found");

            ServiceCategoryRepository categoryRepo = new ServiceCategoryRepository();
            var category= categoryRepo.GetByIdAsync(categoryId);
            if (category == null)
                throw new Exception("Category not found");

            JobRepository jobRepo = new JobRepository();
            var job = new Job
            {
                CustomerId = customerId,
                ServiceCategoryId = categoryId,
                Description = description,
                Location = location,
                IsEmergency = isEmergency,
                Status = "Open",
                CreatedAt = DateTime.Now
            };
            job = await jobRepo.InsertJobAsync(job);

            CustomerService customerService = new CustomerService();
            await customerService.IncrementJobsPostedAsync(customerId);

            return job;
        }

        public async Task<bool> DeleteJobAsync(int jobId)
        {
            JobRepository jobRepo = new JobRepository();
            var job = await jobRepo.GetByIdAsync(jobId);
            if (job == null) return false;

            if (job.Status != "Open")
                throw new Exception("Only open jobs can be deleted");
            await jobRepo.DeleteJobAsync(jobId);
            JobAssignmentRepository jobAssignmentRepo = new JobAssignmentRepository();
            await jobAssignmentRepo.DeleteAsync(jobId);
            return true;
        }

        public async Task<Job?> GetJobByIdAsync(int jobId)
        {
            JobRepository jobRepo = new JobRepository();
            return await jobRepo.GetByIdAsync(jobId);
        }

        public async Task<IEnumerable<Job>> GetJobsByCustomerAsync(int customerId)
        {
            JobRepository jobRepo = new JobRepository();
            return await jobRepo.GetJobsByCustomerAsync(customerId);
        }

        public async Task<IEnumerable<Job>> GetActiveJobsByCustomerAsync(int customerId)
        {
            JobRepository jobRepo = new JobRepository();
            return await jobRepo.GetActiveJobsByCustomerAsync(customerId);
        }

        public async Task<IEnumerable<Job>> GetOpenJobsByCategoryAsync(int categoryId)
        {
            JobRepository jobRepo = new JobRepository();
            return await jobRepo.GetOpenJobsByCategoryAsync(categoryId);
        }

        public async Task<IEnumerable<Job>> GetProviderJobsAsync(int providerId)
        {
            JobRepository jobRepo = new JobRepository();
            return await jobRepo.GetProviderJobsAsync(providerId);
        }

        public async Task CancelJobAsync(int jobId)
        {
            JobRepository jobRepo = new JobRepository();
            var job = await jobRepo.GetByIdAsync(jobId);
            if (job == null)
                throw new Exception("Job not found");

            if (job.Status == "Completed")
                throw new Exception("Completed job cannot be cancelled");

            await jobRepo.CancelAsync(jobId);
        }

        public async Task AcceptJobAsync(int jobId, int providerId)
        {
            JobRepository jobRepo = new JobRepository();
            var job = await jobRepo.GetByIdAsync(jobId);
            if (job == null)
                throw new Exception("Job not found");

            if (job.Status != "Open")
                throw new Exception("Job is not available");

            ServiceProviderRepository providerRepo = new ServiceProviderRepository();
            var provider = await providerRepo.GetByIdAsync(providerId);
            if (provider == null)
                throw new Exception("Provider not found");

            JobAssignmentRepository assignmentRepo = new JobAssignmentRepository();
            await assignmentRepo.AcceptAssignmentAsync(jobId, providerId);

            await jobRepo.MarkAsAcceptedAsync(jobId);

        }

        public async Task CompleteJobAsync(int jobId, decimal price)
        {
            
            
            JobRepository jobRepo = new JobRepository();
            var job = await jobRepo.GetByIdAsync(jobId);
            if (job == null)
                throw new Exception("Job not found");

            if (job.Status != "Accepted")
                throw new Exception("Only accepted jobs can be completed");

            if (price <= 0)
                throw new Exception("Invalid job amount");
            //get the customer
            CustomerService customerService = new CustomerService();
            var customer= await customerService.GetCustomerByIdAsync(job.CustomerId);

            //get the provider
            JobAssignmentService jobAssignmentService = new JobAssignmentService();
            var jobAssignment = await jobAssignmentService.GetAssignmentByJobAsync(jobId);
            ServiceProviderService serviceProviderService = new ServiceProviderService();
            var serviceProvider = await serviceProviderService.GetProviderByIdAsync(jobAssignment.ProviderId);

            WalletService walletService = new WalletService();
            var customerWallet = await walletService.GetWalletByUserIdAsync(customer.UserId);
            var providerWallet = await walletService.GetWalletByUserIdAsync(serviceProvider.UserId);
            var platformWallet = await walletService.GetWalletByUserIdAsync(1);


            if (customerWallet == null || providerWallet == null)
                throw new Exception("Wallet not found");

            decimal commissionRate = 0.05m;
            decimal commission = price * commissionRate;
            decimal providerAmount = price - commission;

            // Move money
            await walletService.TransferAsync(customerWallet.WalletId, providerWallet.WalletId, providerAmount);
            await walletService.TransferAsync(customerWallet.WalletId, platformWallet.WalletId, commission);

            // Transactions
            TransactionService transactionService = new TransactionService();
            await transactionService.RecordTransactionAsync(customerWallet.WalletId, providerWallet.WalletId, jobId, providerAmount, "JobPayment");
            await transactionService.RecordTransactionAsync(customerWallet.WalletId, platformWallet.WalletId, jobId, commission, "Commission");

            await jobRepo.MarkAsCompletedAsync(jobId);

        }
    }
}
