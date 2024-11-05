using LearnHub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LearnHub.Services
{
    public interface IUserService
    {
        Task<User> GetByUsername(string username);
        Task<T> GetUserWithRole<T>(User user) where T : User;

    }
}
