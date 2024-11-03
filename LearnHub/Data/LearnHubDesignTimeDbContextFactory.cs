using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnHub.Data
{
    public class LearnHubDesignTimeDbContextFactory : IDesignTimeDbContextFactory<LearnHubDbContext>
    {
        public LearnHubDbContext CreateDbContext(string[] args)
        {
            DbContextOptions options = new DbContextOptionsBuilder().UseSqlite("Data Source=LearHubSqlite.db").Options;
            return new LearnHubDbContext(options);
        }
    }
}
