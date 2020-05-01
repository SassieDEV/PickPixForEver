using Microsoft.EntityFrameworkCore;
using PickPixForEver.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace PickPixForEver.Services
{
    public class PicturesRepository : IPictureRepository
    {

        private readonly string filePath;
        List<int> picIdsToUpdate = null;
        AlbumRepository albumRep = null;
        public int UserId { get; set; }
        public PicturesRepository(string filePath)
        {
            this.picIdsToUpdate = new List<int>();
            this.filePath = filePath;
            this.albumRep = new AlbumRepository(filePath);
            try
            {
                this.UserId = Preferences.Get("userId", -1);
            }
            catch (InvalidCastException ex)
            {
                this.UserId = -1;
            }

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
                //To-Do Implement logging
            }
            return 0;
        }

        public async Task<int> AddTagAsync(Models.Tag tag)
        {
            try
            {
                using (var dbContext = new PickPixDbContext(filePath))
                {
                    Tag findExistingTag = await dbContext.Tags.Where(s => (s.Name == tag.Name) && (s.TagType == tag.TagType)).FirstOrDefaultAsync().ConfigureAwait(false);
                    if (findExistingTag != null)
                        return findExistingTag.TagId;
                    var tracker = await dbContext.Tags.AddAsync(tag).ConfigureAwait(false);
                    await dbContext.SaveChangesAsync().ConfigureAwait(false);
                }
                return tag.TagId;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return 0;
        }

        public Task<bool> DeleteItemAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Picture> FindItemAsync(int ID)
        {
            Picture pic;
            using (var dbContext = new PickPixDbContext(filePath))
            {
                pic = await dbContext.Pictures

                    .Where(s => s.Id == ID).SingleOrDefaultAsync().ConfigureAwait(false);
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


        public async Task<IEnumerable<Models.Tag>> FindTagByPictureIdAsync(int PictureId)
        {

            using (var dbContext = new PickPixDbContext(filePath))
            {
                return await dbContext.PictureTags
                     .Where(s => s.PictureId == PictureId)
                     .Select(c => c.Tag)
                     .ToListAsync().ConfigureAwait(false);

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
            pic.RawData = GetImageStreamAsBytes(stream);
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

        public async Task<int> HandleImageCommit(int userId, Dictionary<Stream, string> streams, string[][] megaTags, int albumId, string privacy, string notes)
        {
            Models.Tag[] applyTags = Array.Empty<Models.Tag>();
            // Album[] applyAlbums = Array.Empty<Album>();

            if (megaTags.Length > 0)
                applyTags = await HandleTags(userId, megaTags).ConfigureAwait(false);
            //if (albums.Length > 0)
            //    applyAlbums = await HandleAlbums(userId, albums).ConfigureAwait(false);

            using (var ctx = new PickPixDbContext(filePath))
            {
                foreach (KeyValuePair<Stream, string> curPic in streams)
                {
                    Picture pic = getPictureModel(curPic.Key, curPic.Value);
                    pic.Notes = notes;
                    pic.Privacy = privacy;
                    pic.UserId = userId;
                    int curPicId = await this.AddItemAsync(pic).ConfigureAwait(false);

                    if (curPicId == 0)
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
                    if (albumId > 0)
                    {
                        await ctx.PictureAlbums.AddAsync(new PictureAlbum
                        {
                            PictureId = curPicId,
                            AlbumId = albumId
                        }).ConfigureAwait(false);
                    }
                }
                await ctx.SaveChangesAsync().ConfigureAwait(false);
            }
            return 0;
        }
        public async Task<Models.Tag[]> HandleTags(int userId, string[][] megaTags)
        {
            List<Models.Tag> applyTags = new List<Models.Tag>();
            Models.Tag newTag = new Models.Tag();

            //TODO: Figure out how to derive the current user of app and pass their user id instead

            foreach (string curPeople in megaTags.ElementAt(0))
            {
                newTag = new Models.Tag { Name = curPeople, TagType = "People", Updated = DateTime.Now, Created = DateTime.Now };
                int newTagId = await this.AddTagAsync(newTag).ConfigureAwait(false);
                applyTags.Add(await FindTagAsync(newTagId).ConfigureAwait(false));
            }
            foreach (string curPlaces in megaTags.ElementAt(1))
            {
                newTag = new Models.Tag { Name = curPlaces, TagType = "Places", Updated = DateTime.Now, Created = DateTime.Now };
                int newTagId = await this.AddTagAsync(newTag).ConfigureAwait(false);
                applyTags.Add(await FindTagAsync(newTagId).ConfigureAwait(false));
            }
            foreach (string curEvents in megaTags.ElementAt(2))
            {
                newTag = new Models.Tag { Name = curEvents, TagType = "Events",Updated = DateTime.Now, Created = DateTime.Now };
                int newTagId = await this.AddTagAsync(newTag).ConfigureAwait(false);
                applyTags.Add(await FindTagAsync(newTagId).ConfigureAwait(false));
            }
            foreach (string curCustom in megaTags.ElementAt(3))
            {
                newTag = new Models.Tag { Name = curCustom, TagType = "Custom", Updated = DateTime.Now, Created = DateTime.Now };
                int newTagId = await this.AddTagAsync(newTag).ConfigureAwait(false);
                applyTags.Add(await FindTagAsync(newTagId).ConfigureAwait(false));
            }
            foreach (string curRelationship in megaTags.ElementAt(4))
            {
                newTag = new Models.Tag { Name = curRelationship, TagType = "Relationship", Updated = DateTime.Now, Created = DateTime.Now };
                int newTagId = await this.AddTagAsync(newTag).ConfigureAwait(false);
                applyTags.Add(await FindTagAsync(newTagId).ConfigureAwait(false));
            }
            return applyTags.ToArray();
        }

        public async Task<int> EnterImgDataSource(Stream imgStream)
        {
            return 0;
        }


        public async Task<IEnumerable<Picture>> GetItemsAsync()
        {
            IEnumerable<Picture> pictures = new List<Picture>();
            try
            {
                using (var ctx = new PickPixDbContext(this.filePath))
                {
                    pictures = await Task.FromResult(ctx.Pictures.Where(s=>(s.UserId==this.UserId || s.Privacy.ToLower()=="public")).ToList()).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                //To Do: implement logging
            }
            return pictures;
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
                    var albumPics = ctx.Albums.Where(a => (a.Name.ToLower().Contains(searchTerm.ToLower()) && (a.UserId == this.UserId || a.Privacy.ToLower() == "public"))).Select(p => p.PictureAlbums).ToList();
                    List<int> pictureIds = new List<int>();
                    foreach (var albumPic in albumPics)
                    {
                        var pics = albumPic.Select(p => p.PictureId).ToList();
                        if (pics != null && pics.Count > 0)
                        {
                            pictureIds.AddRange(pics);
                        }
                    }
                    var tagPics = ctx.Tags.Where(a => a.Name.ToLower().Contains(searchTerm.ToLower())).Select(p => p.PictureTags).ToList();
                    foreach (var tagPic in tagPics)
                    {
                        var pics = tagPic.Select(p => p.PictureId).ToList();
                        if (pics != null && pics.Count > 0)
                        {
                            pictureIds.AddRange(pics);
                        }
                    }

                    if (pictureIds.Count > 0)
                    {
                        pictureIds = pictureIds.Distinct().ToList();
                    }
                    if (pictureIds != null && pictureIds.Count > 0)
                    {
                        pictures = await ctx.Pictures.
                            Where(p => (pictureIds.Contains(p.Id) && (p.UserId == this.UserId || p.Privacy.ToLower()=="public"))).
                            ToListAsync().ConfigureAwait(false);
                    }
                }
            }
            catch (Exception ex)
            {
                //To-do: Implement logging
            }
            return pictures;
        }

        public Task<bool> UpdateItemAsync(Picture item)
        {
            throw new NotImplementedException();
        }

    }
}
