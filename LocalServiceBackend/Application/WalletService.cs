using Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Trial;

namespace Application
{
    public class WalletService : IWalletService
    {
        public async Task<Wallet> CreateWalletAsync(int userId)
        {
            WalletRepository walletRepo = new WalletRepository();
            var existing = await walletRepo.GetByUserIdAsync(userId);
            if (existing != null)
                throw new Exception("Wallet already exists for this user");

            UserRepository userRepo = new UserRepository();
            var existingUser = await userRepo.GetByIdAsync(userId);
            if (existingUser == null)
                throw new Exception("User not found");

            var wallet = new Wallet
            {
                UserId = userId,
                Balance = 0
            };
            wallet= await walletRepo.InsertWalletAsync(wallet);
            return wallet;
        }

        public async Task<Wallet?> GetWalletByUserIdAsync(int userId)
        {
            UserRepository userRepo = new UserRepository();
            var existingUser = await userRepo.GetByIdAsync(userId);
            if (existingUser == null)
                throw new Exception("User not found");

            WalletRepository walletRepo = new WalletRepository();
            return await walletRepo.GetByUserIdAsync(userId);
        }

        public async Task DepositAsync(int walletId, decimal amount)
        {
            WalletRepository walletRepo = new WalletRepository();
            if (amount <= 0)
                throw new Exception("Deposit amount must be positive");

            var wallet = await walletRepo.GetById(walletId);
            if (wallet==null)
                throw new Exception("Wallet not found");

            wallet.Balance += amount;
            await walletRepo.UpdateBalanceAsync(walletId, wallet.Balance);

        }

        public async Task WithdrawAsync(int walletId, decimal amount)
        {
            if (amount <= 0)
                throw new Exception("Amount must be positive");

            WalletRepository walletRepo = new WalletRepository();
            var wallet = await walletRepo.GetById(walletId);
            if (wallet==null)
                throw new Exception("Wallet not found");

            if (!await walletRepo.HasSufficientBalanceAsync(walletId, amount))
                throw new Exception("Insufficient balance");

            wallet.Balance -= amount;
            await walletRepo.UpdateBalanceAsync(walletId, wallet.Balance);
        }

        public async Task TransferAsync(int fromWalletId, int toWalletId, decimal amount)
        {
            if (amount <= 0)
                throw new Exception("Transfer amount must be positive");

            WalletRepository walletRepo = new WalletRepository();
            var fromWallet = await walletRepo.GetById(fromWalletId);
            var toWallet = await walletRepo.GetById(toWalletId);

            if (fromWallet == null) throw new Exception("Wallet not found");
            if (toWallet == null) throw new Exception("Wallet not found");

            if (!await walletRepo.HasSufficientBalanceAsync(fromWalletId, amount))
                throw new Exception("Insufficient balance");

            fromWallet.Balance -= amount;
            toWallet.Balance += amount;
            await walletRepo.UpdateBalanceAsync(fromWalletId, fromWallet.Balance);
            await walletRepo.UpdateBalanceAsync(toWalletId, toWallet.Balance);
        }

        public async Task<string> GetUserNameByWalletId (int walletId)
        {
            WalletRepository walletRepo = new WalletRepository();
            var exist = await walletRepo.GetById(walletId);
            if (exist == null) throw new Exception("wallet does not exist");

            UserRepository userRepo= new UserRepository();
            var user = await userRepo.GetByIdAsync(exist.UserId);
            return user.FullName;
        }
    }
}
