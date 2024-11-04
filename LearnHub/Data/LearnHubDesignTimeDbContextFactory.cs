using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnHub.Data
{
    public class LearnHubDesignTimeDbContextFactory : IDesignTimeDbContextFactory<LearnHubDbContext>
    {
        public LearnHubDbContext CreateDbContext(string[] args)
        {

            var dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LearnHubSqlite.db");
            DbContextOptions options = new DbContextOptionsBuilder().UseSqlite($"Data Source={dbPath}").Options;
            return new LearnHubDbContext(options);
        }
    }
}
