using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trial;

namespace Application
{
    public interface IWalletService
    {
        Task<Wallet> CreateWalletAsync(int userId);
        Task<Wallet?> GetWalletByUserIdAsync(int userId);
        Task DepositAsync(int walletId, decimal amount);
        Task<string> GetUserNameByWalletId(int walletId);
    }
}
