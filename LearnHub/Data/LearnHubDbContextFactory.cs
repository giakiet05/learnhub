using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnHub.Data
{
   public class LearnHubDbContextFactory
    {
        private readonly string _connectionString;

        public LearnHubDbContextFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public LearnHubDbContext CreateDbContext() {
            DbContextOptions options = new DbContextOptionsBuilder().UseSqlite(_connectionString).Options;
            return new LearnHubDbContext(options);
        }

    }
}
