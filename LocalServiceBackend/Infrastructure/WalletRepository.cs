using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trial;

namespace Infrastructure
{
    public class WalletRepository
    {
        public async Task<Wallet> InsertWalletAsync(Wallet wallet)
        {
            using (var context = new AppDBContext())
            {
                var newWallet = new Wallet
                {
                    UserId = wallet.UserId,
                    Balance = wallet.Balance,
                    
                };
                await context.Wallets.AddAsync(newWallet);
                context.SaveChanges();
                return newWallet;
            }
        }

        public async Task DeleteWalletAsync(int WalletId)
        {
            using (var context = new AppDBContext())
            {
                var wallet = context.Wallets.FirstOrDefault(w => w.WalletId == WalletId);
                if (wallet != null)
                {
                    context.Wallets.Remove(wallet);
                    context.SaveChanges();
                }
            }
        }

        public async Task UpdateWalletAsync(Wallet Wallet)
        {
            using (var context = new AppDBContext())
            {
                var wallet = context.Wallets.FirstOrDefault(w => w.WalletId == Wallet.WalletId);
                if (wallet != null)
                {
                    wallet.UserId = Wallet.UserId;
                    wallet.Balance = Wallet.Balance;
                    context.SaveChanges();
                }
            }
        }

        public async Task<Wallet?> GetByUserIdAsync(int userId)
        {
            using (var context = new AppDBContext())
            {
                var wallet = context.Wallets.FirstOrDefault(w=>w.UserId == userId);
                return wallet;
            }
        }

        public async Task<Wallet?> GetById(int walletId)
        {
            using (var context = new AppDBContext())
            {
                var wallet = context.Wallets.FirstOrDefault(w=>w.WalletId== walletId);
                return wallet;
            }
        }

        public async Task UpdateBalanceAsync(int walletId, decimal newBalance)
        {
            using (var context = new AppDBContext())
            {
                var wallet = await context.Wallets
            .FirstOrDefaultAsync(w => w.WalletId == walletId);
                if (wallet == null)
                    return;
                wallet.Balance = newBalance;

                await context.SaveChangesAsync();
            }
        }

        public async Task<bool> HasSufficientBalanceAsync(int walletId, decimal amount)
        {
            using (var context = new AppDBContext())
            {
                var wallet = await context.Wallets
            .FirstOrDefaultAsync(w => w.WalletId == walletId);

                if (wallet == null)
                    return false;
                return wallet.Balance >= amount;

            }
        }


    }
}
