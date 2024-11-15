using LearnHub.Models;
using Microsoft.Identity.Client.NativeInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnHub.Services
{
    public interface IAuthenticationService
    {
        Task<User> CreateAccount(User user);
        Task<User> Login(string username, string password);
    }
}
