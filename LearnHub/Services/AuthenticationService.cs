using LearnHub.Models;
using System;
using System.Threading.Tasks;
using LearnHub.Exceptions;
using LearnHub.Data;
using Microsoft.AspNetCore.Identity;

namespace LearnHub.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        // Singleton instance
        private static readonly Lazy<AuthenticationService> _instance = new Lazy<AuthenticationService>(() => new AuthenticationService());

        // Singleton property to access the instance
        public static AuthenticationService Instance => _instance.Value;

        private readonly IUserService _userService; // Access UserService Singleton
        private readonly PasswordHasher<User> _passwordHasher; // Use ASP.NET Identity's PasswordHasher

        // Private constructor to prevent external instantiation
        private AuthenticationService()
        {
            // Use Singleton instance of UserService
            _userService = UserService.Instance;
            _passwordHasher = new PasswordHasher<User>(); // Initialize ASP.NET Identity's PasswordHasher
        }

        public async Task<User> CreateAccount(User user)
        {
            // Check if the user already exists
            User existingUser = await _userService.GetByUsername(user.Username);
            if (existingUser != null)
                throw new UsernameAlreadyExistsException(existingUser.Username);

            // Hash the password using ASP.NET Identity's PasswordHasher
            user.Password = _passwordHasher.HashPassword(user, user.Password);

            // Create and return the new user
            return await _userService.CreateUser(user);
        }

        public async Task<User> Login(string username, string password)
        {
            // Retrieve the user by username
            User existingUser = await _userService.GetByUsername(username);
            if (existingUser == null)
                throw new UserNotFoundException(username);

            // Verify the password using ASP.NET Identity's PasswordHasher
            var result = _passwordHasher.VerifyHashedPassword(existingUser, existingUser.Password, password);
            if (result != PasswordVerificationResult.Success)
                throw new InvalidPasswordException(username, password);

            // Return user based on their role
            switch (existingUser.Role)
            {
                case "Admin":
                    return existingUser;

                case "Student":
                    return await _userService.GetUserWithRole<Student>(existingUser);

                case "Teacher":
                    return await _userService.GetUserWithRole<Teacher>(existingUser);
            }

            return null;
        }
    }
}
