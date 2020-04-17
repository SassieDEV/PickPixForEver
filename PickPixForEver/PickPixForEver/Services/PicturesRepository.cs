using PickPixForEver.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PickPixForEver.Services
{
    public class PicturesRepository : IPictureRepository
    {

        private readonly string filePath;

        public PicturesRepository(string filePath)
        {
            this.filePath = filePath;

        }

        public async Task<bool> AddItemAsync(Picture picture)
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
                    result = true;
                }
            }
            catch (Exception ex)
            {
                //To-Do Implement logging
            }
            return result;
        }

        public Task<bool> DeleteItemAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Picture> FindItemAsync(int id)
        {
            throw new NotImplementedException();
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

        public Task<bool> UpdateItemAsync(Picture item)
        {
            throw new NotImplementedException();
        }
    }
}
