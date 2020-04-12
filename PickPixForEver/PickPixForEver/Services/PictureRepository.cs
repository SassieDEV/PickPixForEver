using Microsoft.EntityFrameworkCore;
using PickPixForEver.Models;
using PickPixForEver.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using MetadataExtractor;

namespace PickPixForEver.Services
{
    public class PictureRepository : IPictureRepository
    {
        String filePath;

        public PictureRepository(string dbContextFilePath)
        {
            this.filePath = dbContextFilePath;
        }
        public async Task<int> EnterPicture(Picture picture)
        {
            try
            {
                using (var dbContext = new PickPixDbContext(filePath))
                {
                    var tracker = await dbContext.Pictures.AddAsync(picture).ConfigureAwait(false);
                    await dbContext.SaveChangesAsync().ConfigureAwait(false);
                    //TO-DO: Handle null ID
                    //NOTE: should we put something like this in account repository as well?
                    /*if (picture.Id == "NULL")
                    {

                    }*/
                }
                return picture.Id;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 0;
            }
        }
        public Task<IEnumerable<Picture>> GetPictures()
        {
            return null;
        }
        public async Task<Picture> GetPicture(int ID)
        {
            Picture pic;
            using (var dbContext = new PickPixDbContext(filePath))
            {
                var pic1 = dbContext.Pictures.Select(s => s).FirstOrDefault();
                pic = await dbContext.Pictures.
                Where(s => s.Id == ID).SingleOrDefaultAsync().ConfigureAwait(false);
                return pic1;
            }
        }
        //TODO: Consider whether picture repository should be responsible for creating Picture from Image
        public async Task<int> EnterPictureSource(Image image)
        {
            try
            {
                Picture newPic = new Picture(image);
                DateTime createDate;
                if (Device.RuntimePlatform == "Android")
                {
                    var uri = image.Source.GetValue(UriImageSource.UriProperty);
                    //IEnumerable<Directory> directories = MetadataExtractor.ImageMetadataReader.ReadMetadata();
                }
                else if (Device.RuntimePlatform == "UWP")
                {
                    var file = image.Source.GetValue(FileImageSource.FileProperty);
                    //IEnumerable<Directory> directories = MetadataExtractor.ImageMetadataReader.ReadMetadata(file);
                }
                await EnterPicture(newPic);
                return newPic.Id;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 0;
            }
        }
    }
}
