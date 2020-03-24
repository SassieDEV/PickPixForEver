using System;
using System.Collections.Generic;
using System.Text;

namespace PickPixForEver.Models
{
    public class PictureTag
    {
        public int PictureTagId { get; set; }
        public int PictureId { get; set; }
        public int TagId { get; set; }
        //public Picture Picture { get; set; }
       // public Tag Tag { get; set; }
    }
}
