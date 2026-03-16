using Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trial;

namespace Application
{
    public class CustomerService : ICustomerService
    {
        public async Task<Customer> CreateCustomerProfileAsync(int userId, string defaultLocation)
        {
            UserRepository userRepo = new UserRepository();
            CustomerRepository customerRepo = new CustomerRepository();
            var user = await userRepo.GetByIdAsync(userId);
            if (user == null)
                throw new Exception("User not found");
            if (user.Role != "Customer")
                throw new Exception("User is not a customer");
            var existing = await customerRepo.GetByUserIdAsync(userId);
            if (existing != null)
                throw new Exception("Customer profile already exists");
            var customer = new Customer
            {
                UserId = userId,
                DefaultLocation = defaultLocation,
                TotalJobPosted = "0",
                TrustScore = "60" 
            };
            await customerRepo.InsertCustomerAsync(customer);
            return customer;
        }

        public async Task<bool> DeleteCustomerAsync(int customerId)
        {
            CustomerRepository customerRepo = new CustomerRepository();
            var customer = await customerRepo.GetByIdAsync(customerId);
            if (customer == null) return false;
            if (int.Parse(customer.TotalJobPosted) > 0)
                throw new Exception("Customer cannot be deleted because jobs exist");

            await customerRepo.DeleteCustomerAsync(customerId);
            return true;
        }

        public async Task<Customer?> UpdateCustomerAsync(Customer customer)
        {
            CustomerRepository customerRepo = new CustomerRepository();
            var existing = await customerRepo.GetByIdAsync(customer.CustomerId);
            if (existing == null) return null;
            existing.DefaultLocation = customer.DefaultLocation;

            await customerRepo.UpdateCustomerAsync(existing);
            return existing;
        }

        public async Task<Customer?> GetCustomerByIdAsync(int customerId)
        {
            CustomerRepository customerRepo = new CustomerRepository();
            return await customerRepo.GetByIdAsync(customerId);
        }

        public async Task<Customer?> GetCustomerByUserIdAsync(int userId)
        {
            CustomerRepository customerRepo = new CustomerRepository();
            return await customerRepo.GetByUserIdAsync(userId);
        }

        public async Task IncrementJobsPostedAsync(int customerId)
        {
            CustomerRepository customerRepo = new CustomerRepository();
            var customer = await customerRepo.GetByIdAsync(customerId);
            if (customer == null)
                throw new Exception("Customer not found");

            await customerRepo.IncrementTotalJobsPostedAsync(customerId);
        }

        public async Task UpdateTrustScoreAsync(int customerId, string newScore)
        {
            CustomerRepository customerRepo = new CustomerRepository();
            var customer = await customerRepo.GetByIdAsync(customerId);
            if (customer == null)
                throw new Exception("Customer not found");
            if (int.Parse(newScore)<0 || int.Parse(newScore)>100)
                throw new Exception("Trust Score must be between 0 and 100");
            await customerRepo.UpdateTrustScoreAsync(customerId, newScore);
        }

    }
}
