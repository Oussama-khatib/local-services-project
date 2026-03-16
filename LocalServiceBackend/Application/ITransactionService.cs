using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trial;

namespace Application
{
    public interface ITransactionService
    {
        Task RecordTransactionAsync(int fromWalletId, int toWalletId, int jobId, decimal amount, string type);
        Task<IEnumerable<Transaction>> GetTransactionsByWalletAsync(int walletId);
        Task<IEnumerable<Transaction>?> GetTransactionsByJobAsync(int jobId);
        Task<IEnumerable<Transaction>?> GetTransactionsByTypeAsync(string type);
        Task<decimal> GetPlatformEarningsAsync();
    }
}
