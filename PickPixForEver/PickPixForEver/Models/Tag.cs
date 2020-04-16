using System;
using System.Collections.Generic;
using System.Text;

namespace PickPixForEver.Models
{
    public class Tag
    {
        public Tag()
        {
            PictureTags = new List<PictureTag>();
        }
        public int TagId { get; set; }
        public string Name { get; set; }
        public string TagType { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public int UserId { get; set; }
        public List<PictureTag> PictureTags { get; set; }
       

    }
}
