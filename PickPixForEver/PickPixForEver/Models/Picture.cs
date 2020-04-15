using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PickPixForEver.Models
{
    public class Picture
    {
        public Picture()
        {
            PictureAlbums = new List<PictureAlbum>();
            PictureTags = new List<PictureTag>();
        }
        public int Id { get; set; }        
        public string RawData { get; set; }
        public string Privacy { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public string PictureMetaData { get; set; }
        public string Notes { get; set; }
        public List<PictureAlbum> PictureAlbums { get; set; }
        public List<PictureTag> PictureTags { get; set; }

        [NotMapped]
        public byte[] ImageArray
        {
            get
            {
                return Convert.FromBase64String(RawData);
            }
        }


    }
}
