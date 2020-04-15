using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PickPixForEver.Models
{
    public class PictureAlbum
    {
        public int PictureId { get; set; }
        public Picture Picture { get; set; }
        public int AlbumId { get; set; }       
        public Album Tag { get; set; }
    }
}
