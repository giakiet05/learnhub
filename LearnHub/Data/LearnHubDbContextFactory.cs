using Microsoft.EntityFrameworkCore;
using System;

namespace LearnHub.Data
{
    public class LearnHubDbContextFactory
    {
        private readonly string _connectionString = "Data Source=LearnHubSqlite.db";

        // Singleton instance (Lazy initialization for thread safety)
        private static readonly Lazy<LearnHubDbContextFactory> _instance = new Lazy<LearnHubDbContextFactory>(() => new LearnHubDbContextFactory());

        // Public property to access the Singleton instance
        public static LearnHubDbContextFactory Instance => _instance.Value;

        // Private constructor to prevent external instantiation
        private LearnHubDbContextFactory()
        {
            // Optionally, you can set the connection string here or pass it dynamically
        }

        // Method to create DbContext
        public LearnHubDbContext CreateDbContext()
        {
            DbContextOptions options = new DbContextOptionsBuilder().UseSqlite(_connectionString).Options;
            return new LearnHubDbContext(options);
        }
    }
}
