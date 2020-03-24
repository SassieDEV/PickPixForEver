using PickPixForEver.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PickPixForEver.Services
{
    public interface IAccountRepository
    {
        Task<int> RegisterUser(User user);
        Task<User> GetUser(string email);
    }
}
