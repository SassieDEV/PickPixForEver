using System;
using System.Collections.Generic;
using Xamarin.Forms;
using MetadataExtractor;

namespace PickPixForEver.Models
{
    public class Picture
    {
        Image image;
        public int Id { get; set; }
        public byte [] RawData { get; set; }
        public int Privacy { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public string PictureMetaData { get; set; }
        public List<PictureTag> PictureTags { get; set; }
        public string Notes { get; set; }
        public Picture(Image image) {
            try
            {
                this.image = image;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
