using Microsoft.EntityFrameworkCore;
using PickPixForEver.Models;
using PickPixForEver.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PickPixForEver.Data.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly PickPixDataContext _context;
        public AccountRepository(string dbPath)
        {
            _context = new PickPixDataContext(dbPath);
        }
        public Task<KeyValuePair<bool, string>> Authenticate(string email, string password)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> RegisterUser(User user)
        {
            try
            {
                var tracking = await this._context.Users.AddAsync(user);
                await this._context.SaveChangesAsync();

                var added = tracking.State == EntityState.Added;
                return added;
            }
            catch (Exception)
            {
                return false;
            }
            
        }
    }
}
