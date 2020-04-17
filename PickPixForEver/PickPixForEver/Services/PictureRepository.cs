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
            bool result = false;
            try
            {
                using (var cxt = new PickPixDbContext(this.filePath))
                {
                    picture.Created = DateTime.Now;
                    picture.Updated = DateTime.Now;
                    cxt.Pictures.Add(picture);
                    await cxt.SaveChangesAsync().ConfigureAwait(false);
                    return picture.Id;
                }
            }
            catch (Exception ex)
            {
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
        private byte[] GetImageStreamAsBytes(Stream input)
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
        public async Task<int> HandleImageCommit(Dictionary<Stream,string>  streams, string[][] megaTags, string[] albums, string privacy, string notes)
        {
            Models.Tag[] applyTags = Array.Empty<Models.Tag>();
            Album[] applyAlbums = Array.Empty<Album>();

            if (megaTags.Length > 0)
                applyTags = await HandleTags(megaTags).ConfigureAwait(false);
            if(albums.Length > 0)
                applyAlbums = await HandleAlbums(albums).ConfigureAwait(false);

            using (var ctx = new PickPixDbContext(filePath))
            {
                foreach (KeyValuePair<Stream,string> curPic in streams)
                {
                    Picture pic = getPictureModel(curPic.Key, curPic.Value);
                    pic.Notes = notes;
                    pic.Privacy = privacy;
                    int curPicId = await this.AddItemAsync(pic).ConfigureAwait(false);

                    if(curPicId == 0)
                    {
                        System.Diagnostics.Debug.WriteLine("========================================= Failed to save picture");
                        return 0;
                    }
                        
                    //var tracker = await ctx.Pictures.UpdateItemAsync(curPic){
                    //}
                    foreach (Models.Tag curTag in applyTags)
                    {
                        var tracker = await ctx.PictureTags.AddAsync(new PictureTag
                        {
                            PictureId = curPicId,
                            TagId = curTag.TagId
                        }).ConfigureAwait(false);
                    await ctx.SaveChangesAsync().ConfigureAwait(false);
                    }
                    foreach (Album curAlbum in applyAlbums)
                    {
                        int curAlbumId = curAlbum.Id;
                        await ctx.PictureAlbums.AddAsync(new PictureAlbum
                        {
                            PictureId = curPicId,
                            AlbumId = curAlbum.Id
                        }).ConfigureAwait(false);
                    }
                }
                await ctx.SaveChangesAsync().ConfigureAwait(false);
            }
            return 0;
        }
        public async Task<Models.Tag[]> HandleTags(string[][] megaTags)
        {
            List<Models.Tag> applyTags = new List<Models.Tag>();
            Models.Tag newTag = new Models.Tag();

            //TODO: Figure out how to derive the current user of app and pass their user id instead
            int userId = 1;

            foreach (string curPeople in megaTags.ElementAt(0))
            {
                newTag = new Models.Tag { Name = curPeople, TagType = "People", Updated = DateTime.Now, Created = DateTime.Now};
                await this.AddTagAsync(newTag).ConfigureAwait(false);
                applyTags.Add(await FindTagAsync(newTag.TagId).ConfigureAwait(false));
            }
            foreach (string curPlaces in megaTags.ElementAt(1))
            {
                newTag = new Models.Tag { Name = curPlaces, TagType = "Places", Updated = DateTime.Now, Created = DateTime.Now};
                await this.AddTagAsync(newTag).ConfigureAwait(false);
                applyTags.Add(await FindTagAsync(newTag.TagId).ConfigureAwait(false));
            }
            foreach (string curEvents in megaTags.ElementAt(2))
            {
                newTag = new Models.Tag { Name = curEvents, TagType = "Events", Updated = DateTime.Now, Created = DateTime.Now};
                await this.AddTagAsync(newTag).ConfigureAwait(false);
                applyTags.Add(await FindTagAsync(newTag.TagId).ConfigureAwait(false));
            }
            foreach (string curCustom in megaTags.ElementAt(3))
            {
                newTag = new Models.Tag { Name = curCustom, TagType = "Custom", Updated = DateTime.Now, Created = DateTime.Now};
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

        public async Task<IEnumerable<Picture>> GetItemsAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return await GetItemsAsync().ConfigureAwait(false);
            IEnumerable<Picture> pictures = new List<Picture>();
            // IEnumerable<Picture> union = new List<Picture>();
            try
            {
                using (var ctx = new PickPixDbContext(this.filePath))
                {
                    var albumsResult = ctx.Albums.Where(a => a.Name.ToLower().Contains(searchTerm.ToLower())).Select(s => new
                    {
                        Pictures = s.PictureAlbums.Select(p => p.Picture)
                    }).ToList();
                    if (albumsResult != null && albumsResult.Count > 0)
                    {
                        pictures = albumsResult[0].Pictures;
                    }

                }
            }
            catch (Exception ex)
            {
                //To-do: Implement logging
            }
            return pictures;
        }


        public async Task<IEnumerable<Picture>> GetItemsAsync()
        {
            IEnumerable<Picture> pictures = new List<Picture>();
            try
            {
                using (var ctx = new PickPixDbContext(this.filePath))
                {
                    pictures = await Task.FromResult(ctx.Pictures.ToList()).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                //To Do: implement logging
            }
            return pictures;
        }
    }
}
