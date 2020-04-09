using Microsoft.EntityFrameworkCore;
using PickPixForEver.Models;
using PickPixForEver.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PickPixForEver.Services
{
    public class AccountRepository : IAccountRepository
    {
        private readonly string filePath;

        public AccountRepository(string filePath)
        {
            this.filePath = filePath;

        }
        public async Task<User> GetUser(string email)
        {

            User user;
            using(var dbContext = new PickPixDbContext(filePath))
            {
                 user = await dbContext.Users.
                Where(s => s.Email.ToLower() == email.ToLower())
                .SingleOrDefaultAsync().ConfigureAwait(false);
            }         
           
           return user;
        }

        public async Task<int> RegisterUser(User user)
        {
            try
            {    
               using(var dbContext = new PickPixDbContext(filePath))
                {
                    var tracker = await dbContext.Users.AddAsync(user).ConfigureAwait(false);
                    await dbContext.SaveChangesAsync().ConfigureAwait(false);
                }
                return user.Id;
            }
            catch (Exception)
            {

                return 0;
            }
        }

    }
}
