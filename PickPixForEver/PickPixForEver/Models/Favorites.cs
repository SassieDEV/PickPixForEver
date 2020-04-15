using System;
using System.Collections.Generic;
using System.Text;

namespace PickPixForEver.Models
{
    public class Favorites
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int PictureId { get; set; }

        public User User { get; set; }
        public Picture Picture { get; set; }
    }
}
