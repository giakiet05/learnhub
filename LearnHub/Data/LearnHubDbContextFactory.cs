using Microsoft.EntityFrameworkCore;
using System;

namespace LearnHub.Data
{
    public class LearnHubDbContextFactory
    {
        private readonly string _connectionString = "Data Source=LearnHubSqlite.db";

       
        private static readonly Lazy<LearnHubDbContextFactory> _instance = new Lazy<LearnHubDbContextFactory>(() => new LearnHubDbContextFactory());

      
        public static LearnHubDbContextFactory Instance => _instance.Value;

     
        private LearnHubDbContextFactory()
        {          
        }

   
        public LearnHubDbContext CreateDbContext()
        {
            DbContextOptions options = new DbContextOptionsBuilder().UseSqlite(_connectionString).Options;
            return new LearnHubDbContext(options);
        }
    }
}
