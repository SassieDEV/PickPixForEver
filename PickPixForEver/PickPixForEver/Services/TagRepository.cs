using Microsoft.EntityFrameworkCore;
using PickPixForEver.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PickPixForEver.Services
{
    public class TagRepository : ITagRepository
    {

        private readonly string filePath;

        public TagRepository(string filePath)
        {
            this.filePath = filePath;

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

 
        public async Task<IEnumerable<Tag>> GetItemsAsync(string searchTerm)
        {
            IEnumerable<Tag> tags = new List<Tag>();
            try
            {
                using (var ctx = new PickPixDbContext(this.filePath))
                {
                    tags = await Task.FromResult(ctx.Tags.Where(S => S.Name.ToLower().Contains(searchTerm)).ToList()).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return tags;
        }



        public async Task<IEnumerable<Picture>> GetTaggedPictures(int tagId)
        {
            IEnumerable<Picture> pictures = new List<Picture>();
            try
            {
                /*using (var ctx = new PickPixDbContext(this.filePath))
                {
                    var res =  ctx.Tags.Where(a => a.TagId == tagId).Select(s => new
                    {
                        Pictures = s.PictureTags.Select(p => p.Picture)
                    }).ToList();
                    if (res != null && res.Count > 0)
                    {
                        pictures = res[0].Pictures;
                    }
                }*/
                using (var ctx = new PickPixDbContext(this.filePath)) {
                    PictureTag[] picTagsResult = await ctx.PictureTags.Where(a => a.TagId == tagId).ToArrayAsync().ConfigureAwait(false);
                    foreach (PictureTag picTag in picTagsResult)
                    {
                        Picture pic = await ctx.Pictures.Where(p => p.Id == picTag.PictureId).FirstOrDefaultAsync().ConfigureAwait(false);
                        if (!pictures.Contains(pic))
                        {
                            IEnumerable<Picture> pic1 = await ctx.Pictures.Where(p => p.Id == pic.Id).Distinct().ToArrayAsync().ConfigureAwait(false);
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
