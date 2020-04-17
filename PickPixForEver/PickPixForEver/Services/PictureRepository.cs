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
        List<int> picIdsToUpdate = null;
        AlbumRepository albumRep = null;

        public PictureRepository(string dbContextFilepath)
        {
            this.picIdsToUpdate = new List<int>();
            this.filePath = dbContextFilepath;
            this.albumRep = new AlbumRepository(filePath);
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

        public async Task<int> AddTagAsync(Models.Tag tag)
        {
            try
            {
                using (var dbContext = new PickPixDbContext(filePath))
                {
                    var tracker = await dbContext.Tags.AddAsync(tag).ConfigureAwait(false);
                    await dbContext.SaveChangesAsync().ConfigureAwait(false);
                }
                return tag.TagId;
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
        public async Task<Models.Tag> FindTagAsync(int ID)
        {
            Models.Tag tag;
            using (var dbContext = new PickPixDbContext(filePath))
            {
                tag = await dbContext.Tags.
                Where(s => s.TagId == ID).SingleOrDefaultAsync().ConfigureAwait(false);
                return tag;
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
        private async Task<Picture[]> GetActivePictures()
        {
            List<Picture> activePics = new List<Picture>();
            Picture curActivePic;

            foreach(int activePicId in picIdsToUpdate)
            {
                try
                {
                    curActivePic = await FindItemAsync(activePicId).ConfigureAwait(false);
                    activePics.Add(curActivePic);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    picIdsToUpdate = new List<int>();
                    return null;
                }
            }
            return activePics.ToArray();
        }
        public async Task<bool> InitPic(Stream fileStream, string filePath,  IReadOnlyList<MetadataExtractor.Directory> metaDataDirectories)
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
        public async Task<int> HandleImageCommit(string[][] megaTags, string[] albums, string privacy, string notes)
        {
            Models.Tag[] applyTags = Array.Empty<Models.Tag>();
            Album[] applyAlbums = Array.Empty<Album>();

            if (megaTags.Length > 0)
                applyTags = await HandleTags(megaTags).ConfigureAwait(false);
            if(albums.Length > 0)
                applyAlbums = await HandleAlbums(albums).ConfigureAwait(false);

            Picture[] applyPics = await GetActivePictures().ConfigureAwait(false);
            using (var ctx = new PickPixDbContext(filePath))
            {
                foreach (Picture curPic in applyPics)
                {
                    //var tracker = await ctx.Pictures.UpdateItemAsync(curPic){
                    //}
                    int curPicId = curPic.Id;
                    foreach (Models.Tag curTag in applyTags)
                    {
                        var tracker = await ctx.PictureTags.AddAsync(new PictureTag
                        {
                            PictureId = curPic.Id,
                            TagId = curTag.TagId
                        }).ConfigureAwait(false);
                    await ctx.SaveChangesAsync().ConfigureAwait(false);
                    }
                    foreach (Album curAlbum in applyAlbums)
                    {
                        int curAlbumId = curAlbum.Id;
                        await ctx.PictureAlbums.AddAsync(new PictureAlbum
                        {
                            PictureId = curPic.Id,
                            AlbumId = curAlbum.Id
                        }).ConfigureAwait(false);
                    }
                    curPic.Notes = notes;
                }
                await ctx.SaveChangesAsync().ConfigureAwait(false);
            }
            picIdsToUpdate = new List<int>();
            return 0;
        }
        public async Task<Models.Tag[]> HandleTags(string[][] megaTags)
        {
            List<Models.Tag> applyTags = new List<Models.Tag>();
            Models.Tag newTag = new Models.Tag();

            foreach (string curPeople in megaTags.ElementAt(0))
            {
                newTag = new Models.Tag { Name = curPeople, TagType = "People" };
                await this.AddTagAsync(newTag).ConfigureAwait(false);
                applyTags.Add(await FindTagAsync(newTag.TagId).ConfigureAwait(false));
            }
            foreach (string curPlaces in megaTags.ElementAt(1))
            {
                newTag = new Models.Tag { Name = curPlaces, TagType = "Places" };
                await this.AddTagAsync(newTag).ConfigureAwait(false);
                applyTags.Add(await FindTagAsync(newTag.TagId).ConfigureAwait(false));
            }
            foreach (string curEvents in megaTags.ElementAt(2))
            {
                newTag = new Models.Tag { Name = curEvents, TagType = "Events" };
                await this.AddTagAsync(newTag).ConfigureAwait(false);
                applyTags.Add(await FindTagAsync(newTag.TagId).ConfigureAwait(false));
            }
            foreach (string curCustom in megaTags.ElementAt(3))
            {
                newTag = new Models.Tag { Name = curCustom, TagType = "Custom" };
                await this.AddTagAsync(newTag).ConfigureAwait(false);
                applyTags.Add(await FindTagAsync(newTag.TagId).ConfigureAwait(false));
            }

            return applyTags.ToArray();
        }

        private async Task<Album[]> HandleAlbums(string[] albums)
        {
            List<Album> applyAlbums = new List<Album>();
            Album newAlbum = new Album();

            foreach (string curAlbum in albums)
            {
                newAlbum = new Album { Name = curAlbum };
                await albumRep.AddItemAsync(newAlbum).ConfigureAwait(false);
                applyAlbums.Add(await albumRep.FindItemAsync(newAlbum.Id).ConfigureAwait(false));
            }
            return applyAlbums.ToArray();
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
    }
}
