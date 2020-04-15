using Microsoft.EntityFrameworkCore;
using PickPixForEver.Models;
using PickPixForEver.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
//using MetadataExtractor;
using System.IO;

namespace PickPixForEver.Services
{
    public class PictureRepository : IDataStore<Picture>
    {
        string filePath;

        public PictureRepository(string dbContextFilepath)
        {
            this.filePath = dbContextFilepath;
        }

        public async Task<bool> AddItemAsync(Picture picture)
        {
            try
            {
                using (var dbContext = new PickPixDbContext(filePath))
                {
                    var tracker = await dbContext.Pictures.AddAsync(picture).ConfigureAwait(false);
                    await dbContext.SaveChangesAsync().ConfigureAwait(false);
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        public async Task<bool> UpdateItemAsync(Picture pic)
        {
            return true;
        }
        public async Task<bool> DeleteItemAsync(int id)
        {
            return true;
        }
        public async Task<Picture> FindItemAsync(int ID)
        {
            Picture pic;
            using (var dbContext = new PickPixDbContext(filePath))
            {
                pic = await dbContext.Pictures.
                Where(s => s.Id == ID).SingleOrDefaultAsync().ConfigureAwait(false);
                return pic;
            }
        }
        public async Task<IEnumerable<Picture>> GetItemsAsync()
        {
            IEnumerable<Picture> pictures;
            using (var dbContext = new PickPixDbContext(filePath))
            {
                return await dbContext.Pictures.ToListAsync().ConfigureAwait(false);
            }
        }

        //TO-DO: Impement search-term returns
        public async Task<IEnumerable<Picture>> GetItemsAsync(string searchTerm)
        {
            IEnumerable<Picture> pictures;
            using (var dbContext = new PickPixDbContext(filePath))
            {
                return await dbContext.Pictures.ToListAsync().ConfigureAwait(false);
            }
        }

        public async Task<int> EnterImgDataSource(Stream imgStream)
        {
            /*try
            {
                byte[] imgByte = GetImageStreamAsBytes(imgStream);
                //String b64Str = Convert.ToBase64String(imgByte);
                //System.Diagnostics.Debug.WriteLine(b64Str);

                var pic = new Picture(imgByte, "");
                await AddItemAsync(pic).ConfigureAwait(false);
                return pic.Id;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error: " + ex);
                return 0;
            }*/
            return 0;
        }
        public async Task<int> HandleImage(View[] views, string[][] tags, string notes)
        {
            List<Tag> applyPeople = new List<Tag>();
            List<Tag> applyPlaces = new List<Tag>();
            List<Tag> applyEvents = new List<Tag>();
            List<Tag> applyCustom = new List<Tag>();
            List<Tag> applyAlbum = new List<Tag>();

            foreach (string curPeople in tags.ElementAt(0))
            {
                applyPeople.Add(new Tag{ Value = curPeople, TagType = "People"});
            }
            foreach (string curPlaces in tags.ElementAt(1))
            {
                applyPlaces.Add(new Tag { Value = curPlaces, TagType = "Places" });
            }
            foreach (string curEvents in tags.ElementAt(2))
            {
                applyEvents.Add(new Tag { Value = curEvents, TagType = "Events" });
            }
            foreach (string curCustom in tags.ElementAt(3))
            {
                applyCustom.Add(new Tag { Value = curCustom, TagType = "Custom" });
            }
            foreach (string curAlbum in tags.ElementAt(4))
            {

            }
            return 0;
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
