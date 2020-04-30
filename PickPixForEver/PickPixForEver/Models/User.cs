using System;
using System.Collections.Generic;
using System.Text;

namespace PickPixForEver.Models
{
    public class User
    {
        public User()
        {
            Pictures = new List<Picture>();
            Albums = new List<Album>();
        }
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PasswordHash { get; set; }

        public List<Picture> Pictures { get; set; }
        public List<Album> Albums { get; set; }
    }
}
