using LearnHub.Data;
using LearnHub.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace LearnHub.Services
{
    public class UserService : IUserService
    {
        private readonly LearnHubDbContextFactory _contextFactory;

        public UserService(LearnHubDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<User> GetByUsername(string username)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                return await context.Set<User>().FirstOrDefaultAsync(e => e.Username == username);
            }
        }

        public async Task<T> GetUserWithRole<T>(User user) where T : User
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                T entity = await context.Set<T>().FirstOrDefaultAsync(e => e.Id == user.Id);
                entity.Role = user.Role;
                entity.Username = user.Username;
                entity.Password = user.Password;
                return entity;
            }
        }


    }
}
