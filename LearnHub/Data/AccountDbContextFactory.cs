using Microsoft.EntityFrameworkCore;
using System;

namespace LearnHub.Data
{
    public class AccountDbContextFactory
    {
        private readonly string _connectionString = "Data Source=Account.db";

        // Singleton instance (Lazy initialization for thread safety)
        private static readonly Lazy<AccountDbContextFactory> _instance = new Lazy<AccountDbContextFactory>(() => new AccountDbContextFactory());

        // Public property to access the Singleton instance
        public static AccountDbContextFactory Instance => _instance.Value;

        // Private constructor to prevent external instantiation
        private AccountDbContextFactory()
        {
            // Optionally, you can set the connection string here or pass it dynamically
        }

        // Method to create DbContext
        public AccountDbContext CreateDbContext()
        {
            DbContextOptions options = new DbContextOptionsBuilder().UseSqlite(_connectionString).Options;
            return new AccountDbContext(options);
        }
    }
}
