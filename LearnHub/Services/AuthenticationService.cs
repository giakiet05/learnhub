using LearnHub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnHub.Services
{

    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserService _userService;
        private readonly IPasswordHasher _passwordHasher;

        public AuthenticationService(IUserService userService, IPasswordHasher passwordHasher)
        {
            _userService = userService;
            _passwordHasher = passwordHasher;
        }

        public async Task<AccountCreationResult> CreateAccount(User user)
        {
            User exisingUser = await _userService.GetByUsername(user.Username);
            if (exisingUser != null) return AccountCreationResult.UsernameAlreadyExists;

            user.Password = _passwordHasher.HashPassword(user.Password);

            await _userService.CreateUser(user);

            return AccountCreationResult.Success;

        }

        public async Task<User> Login(string username, string password)
        {
            User existingUser = await _userService.GetByUsername(username);
            if (existingUser == null) return null;

            bool isPasswordMatched = _passwordHasher.VerifyPassword(password, existingUser.Password);
            if (!isPasswordMatched) return null;

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
