using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Trial
{
    public class AppDBContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(Connections.sqlConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ================= USER =================
            modelBuilder.Entity<User>()
                .HasKey(u => u.UserId);

            // User → Customer (1-M)
            modelBuilder.Entity<Customer>()
                .HasOne<User>()
                .WithMany(u => u.Customers)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // User → ServiceProvider (1-M)
            modelBuilder.Entity<ServiceProvider>()
                .HasOne<User>()
                .WithMany(u => u.ServiceProviders)
                .HasForeignKey(sp => sp.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // User → Wallet (1-M)
            modelBuilder.Entity<Wallet>()
                .HasOne<User>()
                .WithMany(u => u.Wallets)
                .HasForeignKey(w => w.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // ================= CUSTOMER =================
            modelBuilder.Entity<Customer>()
                .HasKey(c => c.CustomerId);

            // Customer → Job (1-M)
            modelBuilder.Entity<Job>()
                .HasOne<Customer>()
                .WithMany(c => c.Jobs)
                .HasForeignKey(j => j.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            // ================= SERVICE CATEGORY =================
            modelBuilder.Entity<ServiceCategory>()
                .HasKey(sc => sc.ServiceCategoryId);

            // Category → Job (1-M)
            modelBuilder.Entity<Job>()
                .HasOne<ServiceCategory>()
                .WithMany(sc => sc.Jobs)
                .HasForeignKey(j => j.ServiceCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // ================= PROVIDER SERVICE =================
            modelBuilder.Entity<ProviderService>()
                .HasKey(ps => ps.ServiceId);

            modelBuilder.Entity<ProviderService>()
                .HasOne<ServiceProvider>()
                .WithMany(sp => sp.ProviderServices)
                .HasForeignKey(ps => ps.ProviderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ProviderService>()
                .HasOne<ServiceCategory>()
                .WithMany(sc => sc.ProviderServices)
                .HasForeignKey(ps => ps.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // ================= JOB ASSIGNMENT =================
            modelBuilder.Entity<JobAssignment>()
                .HasKey(ja => ja.JobAssignmentId);

            modelBuilder.Entity<JobAssignment>()
                .HasOne<Job>()
                .WithMany(j => j.JobAssignments)
                .HasForeignKey(ja => ja.JobId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<JobAssignment>()
                .HasOne<ServiceProvider>()
                .WithMany(sp => sp.JobAssignments)
                .HasForeignKey(ja => ja.ProviderId)
                .OnDelete(DeleteBehavior.Restrict);

            // ================= REVIEW =================
            modelBuilder.Entity<Review>()
                .HasKey(r => r.ReviewId);

            modelBuilder.Entity<Review>()
                .HasOne<Job>()
                .WithMany(j => j.Reviews)
                .HasForeignKey(r => r.JobId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Review>()
                .HasOne<ServiceProvider>()
                .WithMany(sp => sp.Reviews)
                .HasForeignKey(r => r.ProviderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Review>()
                .HasOne<Customer>()
                .WithMany(c => c.Reviews)
                .HasForeignKey(r => r.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            // ================= REVIEW SUMMARY =================
            modelBuilder.Entity<ReviewsSummary>()
                .HasKey(rs => rs.ReviewsSummaryId);

            modelBuilder.Entity<ReviewsSummary>()
                .HasOne<ServiceProvider>()
                .WithMany(sp => sp.ReviewsSummaries)
                .HasForeignKey(rs => rs.ProviderId)
                .OnDelete(DeleteBehavior.Restrict);

            // ================= TRANSACTION =================
            modelBuilder.Entity<Transaction>()
                .HasKey(t => t.TransactionId);

            modelBuilder.Entity<Transaction>()
                .HasOne<Job>()
                .WithMany(j => j.Transactions)
                .HasForeignKey(t => t.JobId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Transaction>()
                .HasOne<Wallet>()
                .WithMany()
                .HasForeignKey(t => t.FromWalletId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Transaction>()
                .HasOne<Wallet>()
                .WithMany(w => w.Transactions)
                .HasForeignKey(t => t.ToWalletId)
                .OnDelete(DeleteBehavior.Restrict);

            // ================= WALLET =================
            modelBuilder.Entity<Wallet>()
                .HasKey(w => w.WalletId);
        }


        public DbSet<User> Users { get; set; }
        public DbSet<ServiceProvider> ServiceProviders { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<ServiceCategory> ServiceCategories { get; set; }
        public DbSet<ProviderService> ProviderServices { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<JobAssignment> JobAssignments { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<ReviewsSummary> ReviewsSummaries { get; set; }
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
    }

}
