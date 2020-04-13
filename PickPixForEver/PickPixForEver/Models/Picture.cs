using System;
using System.Collections.Generic;
using System.Text;

namespace PickPixForEver.Models
{
    public class Picture
    {
        public int Id { get; set; }
        public byte [] RawData { get; set; }
        public int Privacy { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public string PictureMetaData { get; set; }
        public int AlbumId { get; set; }
        public string Notes { get; set; }

        //public Album Album { get; set; }
        //public List<PictureTag> PictureTags { get; set; }


    }
}
