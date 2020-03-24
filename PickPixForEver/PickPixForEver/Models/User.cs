using System;
using System.Collections.Generic;
using System.Text;

namespace PickPixForEver.Models
{
    public class User
    {
        public User()
        {
            Tag = new Tag();
        }
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PasswordHash { get; set; }

        public Tag Tag { get; set; }
    }
}
