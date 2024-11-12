using LearnHub.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using LearnHub.Data;

namespace LearnHub.Services
{
    public class UserService : IUserService
    {
        // Singleton instance
        private static readonly Lazy<UserService> _instance = new Lazy<UserService>(() => new UserService());

        // Singleton property to access the instance
        public static UserService Instance => _instance.Value;

        // Private constructor to prevent external instantiation
        private UserService()
        {
            // No need to inject DbContextFactory anymore; use Singleton instance
        }

        public async Task<User> GetByUsername(string username)
        {
            using (var context = LearnHubDbContextFactory.Instance.CreateDbContext())
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
            using (var context = LearnHubDbContextFactory.Instance.CreateDbContext())
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
            using (var context = LearnHubDbContextFactory.Instance.CreateDbContext())
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
