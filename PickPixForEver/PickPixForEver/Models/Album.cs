using System;
using System.Collections.Generic;
using System.Text;

namespace PickPixForEver.Models
{
    public class Album
    {
        public Album()
        {
            Pictures = new List<Picture>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Privacy { get; set; }

        public bool Active { get; set; }
        public List<Picture> Pictures { get; set; }

    }
}
