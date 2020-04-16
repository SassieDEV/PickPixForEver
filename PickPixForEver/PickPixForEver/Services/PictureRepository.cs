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
    public class PictureRepository : IDataStore<Picture>
    {
        string filePath;
        List<int> picIdsToUpdate = new List<int>();

        public PictureRepository(string dbContextFilepath)
        {
            this.filePath = dbContextFilepath;
        }

        public async Task<int> AddItemAsync(Picture picture)
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
                Console.WriteLine(ex.Message);
                return 0;
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
        public async Task<bool> InitPic(Stream fileStream, string filePath, IReadOnlyList<MetadataExtractor.Directory> metaDataDirectories)
        {
            int picIdNew = 0;
            Picture pic = getPictureModel(fileStream, filePath);
            picIdNew = await this.AddItemAsync(pic).ConfigureAwait(false);
            if (picIdNew == 0)
            {
                System.Diagnostics.Debug.WriteLine("========================================= Failed to save picture");
                return false;
            }
            else
            {
                picIdsToUpdate.Add(picIdNew);
                return true;
            }

        }
        private Picture getPictureModel(Stream stream, string filePath)
        {
            Picture pic = new Picture();
            pic.RawData = Convert.ToBase64String(GetImageStreamAsBytes(stream));
            pic.Created = DateTime.Now;
            pic.Updated = DateTime.Now;
            pic.PictureMetaData = filePath;
            pic.Notes = "";
            pic.Privacy = "";
            return pic;
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
        public async Task<int> HandleImageIntro(Stream fileStream)
        {
            return 0;
        }
        public async Task<int> HandleImageCommit(Picture[] pics, string[][] megaTags, string[] albums, string notes)
        {
            List<Models.Tag> applyTags = await HandleTags(megaTags);
            List<Album> applyAlbums = await HandleAlbums(albums);
            foreach (Picture curPic in pics)
            {
                foreach (Models.Tag curTag in applyTags)
                {

                }
                foreach (Album curAlbum in applyAlbums)
                {

                }
            }
            return 0;
        }
        public async Task<List<Models.Tag>> HandleTags(string[][] megaTags)
        {
            List<Models.Tag> applyTags = new List<Models.Tag>();

            foreach (string curPeople in megaTags.ElementAt(0))
            {
                applyTags.Add(new Models.Tag { Value = curPeople, TagType = "People" });
            }
            foreach (string curPlaces in megaTags.ElementAt(1))
            {
                applyTags.Add(new Models.Tag { Value = curPlaces, TagType = "Places" });
            }
            foreach (string curEvents in megaTags.ElementAt(2))
            {
                applyTags.Add(new Models.Tag { Value = curEvents, TagType = "Events" });
            }
            foreach (string curCustom in megaTags.ElementAt(3))
            {
                applyTags.Add(new Models.Tag { Value = curCustom, TagType = "Custom" });
            }

            return applyTags;
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

        public async Task<List<Album>> HandleAlbums(string[] albums)
        {
            List<Album> applyAlbums = new List<Album>();
            foreach (string curAlbum in albums)
            {
                applyAlbums.Add(new Album { Name = curAlbum });
            }
            return applyAlbums;
        }
    }
}
