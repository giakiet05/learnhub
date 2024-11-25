using LearnHub.Models;
using System;
using System.Threading.Tasks;
using LearnHub.Exceptions;
using LearnHub.Data;
using Microsoft.AspNetCore.Identity;

namespace LearnHub.Services
{
    public class AuthenticationService 
    {
        // Singleton instance
        private static readonly Lazy<AuthenticationService> _instance = new Lazy<AuthenticationService>(() => new AuthenticationService());

        // Singleton property to access the instance
        public static AuthenticationService Instance => _instance.Value;
        private readonly UserService _userService;
   
        private readonly PasswordHasher<User> _passwordHasher;

      
        private AuthenticationService()
        {
          
            _userService = UserService.Instance;
            _passwordHasher = new PasswordHasher<User>(); 
        }

        public async Task<User> Login(string username, string password)
        {
           
            User existingUser = await _userService.GetByUsername(username);
            if (existingUser == null)
                throw new UserNotFoundException(username);

          
            var result = _passwordHasher.VerifyHashedPassword(existingUser, existingUser.Password, password);
            if (result != PasswordVerificationResult.Success)
                throw new InvalidPasswordException(username, password);

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
