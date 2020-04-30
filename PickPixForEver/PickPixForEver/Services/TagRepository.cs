using PickPixForEver.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace PickPixForEver.Services
{
    public class TagRepository : ITagRepository
    {

        private readonly string filePath;
        public int UserId { get; set; }
        public TagRepository(string filePath)
        {
            this.filePath = filePath;
            try
            {
                this.UserId = Preferences.Get("userId", -1);
            }
            catch (InvalidCastException ex)
            {
                this.UserId = -1;
            }
        }

        public Task<int> AddItemAsync(Tag item)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteItemAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Tag> FindItemAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Tag>> GetItemsAsync()
        {
            IEnumerable<Tag> tags = new List<Tag>();
            try
            {
                using (var ctx = new PickPixDbContext(this.filePath))
                {
                    tags = await Task.FromResult(ctx.Tags.ToList()).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                //To Do: implement logging
            }
            return tags;
        }

        public Task<IEnumerable<Tag>> GetItemsAsync(string searchTerm)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Picture> GetTaggedPictures(int tagId)
        {
            IEnumerable<Picture> pictures = new List<Picture>();
            try
            {
                using (var ctx = new PickPixDbContext(this.filePath)) {
                    PictureTag[] picTagsResult = ctx.PictureTags.Where(a => (a.TagId == tagId && (a.Picture.UserId==this.UserId || a.Picture.Privacy.ToLower()=="public"))).ToArray();
                    foreach (PictureTag picTag in picTagsResult)
                    {
                        Picture pic = ctx.Pictures.Where(p => p.Id == picTag.PictureId).FirstOrDefault();
                        if (!pictures.Contains(pic))
                        {
                            IEnumerable<Picture> pic1 = ctx.Pictures.Where(p => p.Id == pic.Id).Distinct().ToArray();
                            pictures = pictures.Union(pic1);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //To-do: Implement logging
            }
            return pictures;
        }

        public Task<bool> UpdateItemAsync(Tag item)
        {
            throw new NotImplementedException();
        }
    }
}
