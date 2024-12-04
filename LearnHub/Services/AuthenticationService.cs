using LearnHub.Models;
using System;
using System.Threading.Tasks;
using LearnHub.Exceptions;
using LearnHub.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LearnHub.Services
{
    public class AuthenticationService
    {
        // Singleton instance
        private static readonly Lazy<AuthenticationService> _instance = new Lazy<AuthenticationService>(() => new AuthenticationService());

        // Singleton property to access the instance
        public static AuthenticationService Instance => _instance.Value;

        private AuthenticationService() {}

        public async Task<User> Login(string username, string password)
        {
            using (var context = LearnHubDbContextFactory.Instance.CreateDbContext()) {

                //User existingUser = await GenericDataService<User>.Instance.GetOne(e => e.Username == username);
                User existingUser = await context.Users.FirstOrDefaultAsync(e => e.Username == username);

                if (existingUser == null)
                    throw new UserNotFoundException(username);

                var passwordHasher = new PasswordHasher<User>();
                var result = passwordHasher.VerifyHashedPassword(existingUser, existingUser.Password, password);
                if (result != PasswordVerificationResult.Success)
                    throw new InvalidPasswordException(username, password);

                if (existingUser.Role == "Admin") return (Admin)existingUser;

                return null;
            }

        }
    }
}
