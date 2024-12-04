using LearnHub.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnHub.Data
{
    public class AccountDbContext : DbContext
    {
        public DbSet<Admin> Admins { get; set; }
    }
}
