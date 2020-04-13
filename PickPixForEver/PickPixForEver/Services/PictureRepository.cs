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
using System.IO;

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
                }
                return picture.Id;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("========ERROR========" + ex.Message);
                return 0;
            }
        }
        public async Task<IEnumerable<Picture>> GetPictures()
        {
            IEnumerable<Picture> pictures;
            using (var dbContext = new PickPixDbContext(filePath))
            {
                return await dbContext.Pictures.ToListAsync().ConfigureAwait(false);
            }

            //return pictures;
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


        public async Task<int> EnterImgDataSource(Stream imgStream)
        {
            try
            {
                byte[] imgByte = GetImageStreamAsBytes(imgStream);
                //String b64Str = Convert.ToBase64String(imgByte);
                //System.Diagnostics.Debug.WriteLine(b64Str);

                var pic = new Picture(imgByte, "");
                await EnterPicture(pic).ConfigureAwait(false);
                return pic.Id;
            } catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error: " + ex);
                return 0;
            }

        }

        public byte[] GetImageStreamAsBytes(Stream input)
        {
            var buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
    }
}
