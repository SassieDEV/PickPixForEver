using System;
using System.Collections.Generic;
using System.Text;

namespace PickPixForEver.Models
{
    public class PictureTag
    {
        public int PictureTagId { get; set; }
        public string TagDescriptor { get; set; }
        //TODO: Revisit this approach, I'm not sure a tag should contain pictures
        public List<Picture> Pictures { get; set; }
    }
}
