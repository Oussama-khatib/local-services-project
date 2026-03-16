using Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trial;

namespace Application
{
    public class TransactionService : ITransactionService
    {
        public async Task RecordTransactionAsync(
        int fromWalletId,
        int toWalletId,
        int jobId,
        decimal amount,
        string type
        )
        {
            if (amount <= 0)
                throw new Exception("Transaction amount must be positive");

            WalletRepository walletRepo = new WalletRepository();
            var fromWallet = walletRepo.GetById(fromWalletId);
            if (fromWallet == null) throw new Exception("wallet not found");

            var toWallet = walletRepo.GetById(toWalletId);
            if (toWallet == null) throw new Exception("wallet not found");

            var transaction = new Transaction
            {
                FromWalletId = fromWalletId,
                ToWalletId = toWalletId,
                JobId = jobId,
                Amount = amount,
                Type = type, // JobPayment, Commission
            };
            TransactionRepository transactionRepo = new TransactionRepository();
            await transactionRepo.InsertTransactionAsync(transaction);
        }

        public async Task<IEnumerable<Transaction>> GetTransactionsByWalletAsync(int walletId)
        {
            WalletRepository walletRepo = new WalletRepository();
            var wallet = walletRepo.GetById(walletId);
            if (wallet == null) throw new Exception("wallet not found");

            TransactionRepository transactionRepo = new TransactionRepository();
            return await  transactionRepo.GetByWalletAsync(walletId);
        }

        public async Task<IEnumerable<Transaction>?> GetTransactionsByJobAsync(int jobId)
        {
            JobRepository jobRepo = new JobRepository();
            var job = jobRepo.GetByIdAsync(jobId);
            if (job == null) throw new Exception("job not found");

            TransactionRepository transactionRepo = new TransactionRepository();
            return await transactionRepo.GetByJobAsync(jobId);
        }

        public async Task<IEnumerable<Transaction>?> GetTransactionsByTypeAsync(string type)
        {
            TransactionRepository transactionRepo = new TransactionRepository();
            return await transactionRepo.GetTransactionsByTypeAsync(type);
        }

        public async Task<decimal> GetPlatformEarningsAsync()
        {
            TransactionRepository transactionRepo = new TransactionRepository();
            return await transactionRepo.GetPlatformEarningsAsync();
        }
    }
}
