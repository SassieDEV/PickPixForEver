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
        public int UserId { get; set; }
        [NotMapped]
        public byte[] ImageArray
        {
            get
            {
                return Convert.FromBase64String(RawData);
            }
        }


        public override bool Equals(object obj)
        {
            Picture picture = obj as Picture;
            if (picture == null)
                return false;
            return picture.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

    }
}
