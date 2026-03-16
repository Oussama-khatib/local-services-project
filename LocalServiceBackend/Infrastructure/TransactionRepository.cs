using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trial;

namespace Infrastructure
{
    public class TransactionRepository
    {
        public async Task InsertTransactionAsync(Transaction transaction)
        {
            using (var context = new AppDBContext())
            {
                var newTransaction = new Transaction
                {
                    FromWalletId = transaction.FromWalletId,
                    ToWalletId = transaction.ToWalletId,
                    JobId = transaction.JobId,
                    Amount = transaction.Amount,
                    Type = transaction.Type,
                };
                await context.Transactions.AddAsync(newTransaction);
                context.SaveChanges();
            }
        }

        public async Task DeleteTransactionAsync(int TransactionId)
        {
            using (var context = new AppDBContext())
            {
                var transaction = context.Transactions.FirstOrDefault(t => t.TransactionId == TransactionId);
                if (transaction != null)
                {
                    context.Transactions.Remove(transaction);
                    context.SaveChanges();
                }
            }
        }

        public async Task UpdateTransactionAsync(Transaction Transaction)
        {
            using (var context = new AppDBContext())
            {
                var transaction = context.Transactions.FirstOrDefault(t => t.TransactionId == Transaction.TransactionId);
                if (transaction != null)
                {
                    transaction.FromWalletId=Transaction.FromWalletId;
                    transaction.ToWalletId=Transaction.ToWalletId;
                    transaction.JobId=Transaction.JobId;
                    transaction.Amount=Transaction.Amount;
                    transaction.Type = Transaction.Type;
                    context.SaveChanges();
                }
            }
        }

        public async Task<IEnumerable<Transaction>> GetByWalletAsync(int walletId)
        {
            using (var context = new AppDBContext())
            {
                return await context.Transactions
            .Where(t => t.FromWalletId == walletId || t.ToWalletId == walletId)
            .OrderByDescending(t => t.TransactionId)
            .ToListAsync();
            }
        }

        public async Task<IEnumerable<Transaction>?> GetByJobAsync(int jobId)
        {
            using (var context = new AppDBContext())
            {
                return await context.Transactions
            .Where(t => t.JobId == jobId)
            .ToListAsync();
            }
        }

        public async Task<decimal> GetPlatformEarningsAsync()
        {
            using (var context = new AppDBContext())
            {
                return await context.Transactions
            .Where(t => t.Type == "Commission")
            .SumAsync(t => t.Amount);
            }
        }

        public async Task<IEnumerable<Transaction>?> GetTransactionsByTypeAsync(String type)
        {
            using (var context = new AppDBContext())
            {
                return await context.Transactions
            .Where(t => t.Type == type)
            .OrderByDescending(t => t.TransactionId)
            .ToListAsync();
            }
        }
    }
}
