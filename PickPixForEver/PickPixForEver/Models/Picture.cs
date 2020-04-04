using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace PickPixForEver.Models
{
    public class Picture
    {
        Image bitmap;
        public int Id { get; set; }
        public byte [] RawData { get; set; }
        public int Privacy { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public string PictureMetaData { get; set; }
        public List<PictureTag> PictureTags { get; set; }
        public Picture(Image image)
        {
            string path = image.Source.ToString();
            //TODO: Set up image creation and addition to list
            /*ExifReader exRead = new ExifReader();
            bitmap = bit
            Created =*/ 
        }
        //{
          //  Created = image.
        //}
    }
}
