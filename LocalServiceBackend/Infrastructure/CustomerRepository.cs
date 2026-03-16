using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trial;

namespace Infrastructure
{
    public class CustomerRepository
    {
        public async Task InsertCustomerAsync(Customer customer)
        {
            using (var context = new AppDBContext())
            {
                var newCustomer = new Customer
                {
                    UserId = customer.UserId,
                    DefaultLocation = customer.DefaultLocation,
                    TotalJobPosted = customer.TotalJobPosted,
                    TrustScore = customer.TrustScore,
                };
                await context.Customers.AddAsync(newCustomer);
                context.SaveChanges();
            }
        }

        public async Task DeleteCustomerAsync(int CustomerId)
        {
            using (var context = new AppDBContext())
            {
                var customer = context.Customers.FirstOrDefault(c => c.CustomerId == CustomerId);
                if (customer != null)
                {
                    context.Customers.Remove(customer);
                    context.SaveChanges();
                }
            }
        }

        public async Task UpdateCustomerAsync(Customer Customer)
        {
            using (var context = new AppDBContext())
            {
                var customer = context.Customers.FirstOrDefault(c => c.CustomerId ==Customer.CustomerId);
                if (customer != null)
                {
                    customer.UserId = Customer.UserId;
                    customer.DefaultLocation = Customer.DefaultLocation;
                    customer.TotalJobPosted = Customer.TotalJobPosted;
                    customer.TrustScore = Customer.TrustScore;
                    context.SaveChanges();
                }
            }
        }

        public async Task<Customer?> GetByIdAsync(int customerId)
        {
            using (var context = new AppDBContext())
            {
                return await context.Customers
            .FirstOrDefaultAsync(c => c.CustomerId == customerId);
            }
        }

        public async Task<Customer?> GetByUserIdAsync(int userId)
        {
            using (var context = new AppDBContext())
            {
                return await context.Customers
                    .FirstOrDefaultAsync(c => c.UserId == userId);
            }
        }

        public async Task IncrementTotalJobsPostedAsync(int customerId)
        {
            using (var context = new AppDBContext())
            {
                var customer = await context.Customers
            .FirstOrDefaultAsync(c => c.CustomerId == customerId);
                if (customer == null) 
                    return; 
                int totalJobPosted=int.Parse(customer.TotalJobPosted)+1;
                customer.TotalJobPosted = totalJobPosted.ToString();
                await context.SaveChangesAsync();
            } 
        }

        public async Task UpdateTrustScoreAsync(int customerId, string newScore)
        {
            using (var context = new AppDBContext())
            {
                var customer = await context.Customers
            .FirstOrDefaultAsync(c => c.CustomerId == customerId);

                if (customer == null)
                    return;

                customer.TrustScore = newScore;
                await context.SaveChangesAsync();
            }
        }

    }
}
