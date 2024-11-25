using LearnHub.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using LearnHub.Data;

namespace LearnHub.Services
{
    public class UserService 
    {
        // Singleton instance
        private static readonly Lazy<UserService> _instance = new Lazy<UserService>(() => new UserService());

        // Singleton property to access the instance
        public static UserService Instance => _instance.Value;

      
        private readonly LearnHubDbContextFactory _contextFactory;

       
        private UserService()
        {
            _contextFactory = LearnHubDbContextFactory.Instance;
        }

        public async Task<User> GetByUsername(string username)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                try
                {
                    return await context.Set<User>().FirstOrDefaultAsync(e => e.Username == username);
                }
                catch (Exception)
                {
                    throw new Exception("An error occurred while fetching the user.");
                }
            }
        }

        public async Task<T> GetUserWithRole<T>(User user) where T : User
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                try
                {
                    T entity = await context.Set<T>().FirstOrDefaultAsync(e => e.Id == user.Id);
                    entity.Role = user.Role;
                    entity.Username = user.Username;
                    entity.Password = user.Password;
                    return entity;
                }
                catch (Exception)
                {
                    throw new Exception("An error occurred while fetching the user with role.");
                }
            }
        }

        public async Task<User> CreateUser(User user)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                try
                {
                    var createdResult = await context.Set<User>().AddAsync(user);
                    await context.SaveChangesAsync();
                    return createdResult.Entity;
                }
                catch (DbUpdateException)
                {
                    throw new DbUpdateException("An error occurred while creating the user.");
                }
                catch (Exception)
                {
                    throw new Exception("An error occurred while creating the user.");
                }
            }
        }
    }
}
