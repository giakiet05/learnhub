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

        private AuthenticationService() { }

        public async Task<User> Login(string username, string password)
        {
            using (var context = LearnHubDbContextFactory.Instance.CreateDbContext())
            {
                User user = await context.Users.FirstOrDefaultAsync(e => e.Username == username);

                if (user == null)
                    throw new UserNotFoundException(username);

                var passwordHasher = new PasswordHasher<User>();

                var result = passwordHasher.VerifyHashedPassword(user, user.Password, password);

                if (result != PasswordVerificationResult.Success)
                    throw new InvalidPasswordException(username, password);

                if (user.Role == "Admin") return user;

                return null;
            }
        }    
    }
}
