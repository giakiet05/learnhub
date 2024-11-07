using LearnHub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LearnHub.Exceptions;

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

        public async Task<User> CreateAccount(User user)
        {
            User exisingUser = await _userService.GetByUsername(user.Username);
            if (exisingUser != null) throw new UsernameAlreadyExistsException(exisingUser.Username);

            user.Password = _passwordHasher.HashPassword(user.Password); //mã hóa mật khẩu

            return await _userService.CreateUser(user);

        }

        public async Task<User> Login(string username, string password)
        {
            User existingUser = await _userService.GetByUsername(username);
            if (existingUser == null) throw new UserNotFoundException(username);

            bool isPasswordMatched = _passwordHasher.VerifyPassword(password, existingUser.Password);
            if (!isPasswordMatched) throw new InvalidPasswordException(username, password);

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
