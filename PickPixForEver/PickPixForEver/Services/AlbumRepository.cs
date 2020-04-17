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
    public class AlbumRepository : IAlbumRepository
    {
        private readonly string filePath;

        public AlbumRepository(string filePath)
        {
            this.filePath = filePath;

        }
        public async Task<int> AddItemAsync(Album album)
        {
            try
            {
                using(var ctx = new PickPixDbContext(this.filePath))
                {
                    album.CreatedAt = DateTime.Now;
                    album.UpdatedAt = DateTime.Now;
                    album.Active = true;
                    var tracker = await ctx.Albums.AddAsync(album).ConfigureAwait(false);
                    await ctx.SaveChangesAsync().ConfigureAwait(false);
                }
                return album.Id;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return 0;
        }

        public async Task<bool> DeleteItemAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Album> FindItemAsync(int ID)
        {
            Album album;
            using (var dbContext = new PickPixDbContext(filePath))
            {
                album = await dbContext.Albums.
                Where(s => s.Id == ID).SingleOrDefaultAsync().ConfigureAwait(false);
                return album;
            }
        }

        public async Task<IEnumerable<Album>> GetItemsAsync()
        {
            IEnumerable<Album> albums = new List<Album>();
            try
            {
                using(var ctx = new PickPixDbContext(this.filePath))
                {
                    albums = await Task.FromResult(ctx.Albums.ToList()).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return albums;
        }

        public async Task<IEnumerable<Album>> GetItemsAsync(string searchTerm)
        {
            IEnumerable<Album> albums = new List<Album>();
            try
            {
                using (var ctx = new PickPixDbContext(this.filePath))
                {
                    albums = await Task.FromResult(ctx.Albums.Where(S=>S.Name.ToLower().Contains(searchTerm)).ToList()).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return albums;
        }

        public async Task<IEnumerable<Picture>> GetAlbumPictures(int albumId)
        {
            IEnumerable<Picture> pictures = new List<Picture>();
            try
            {
                using(var ctx = new PickPixDbContext(this.filePath))
                {
                    var res = ctx.Albums.Where(a => a.Id == albumId).Select(s => new
                    {
                        Pictures = s.PictureAlbums.Select(p => p.Picture)
                    }).ToList() ;
                    if(res!=null && res.Count > 0)
                    {
                        pictures = res[0].Pictures;
                    }
                }
            }
            catch (Exception ex)
            {
                //To-do: Implement logging
            }
            return pictures;
        }

        public async Task<bool> UpdateItemAsync(Album item)
        {
            bool result = false;
            try
            {
                using(var ctx = new PickPixDbContext(this.filePath))
                {
                    Album album = ctx.Albums.SingleOrDefault(A => A.Id == item.Id);
                    if (album != null)
                    {
                        album.Name = item.Name;
                        album.Description = item.Description;
                        album.Privacy = item.Privacy;
                        album.Active = item.Active;
                        album.UpdatedAt = item.UpdatedAt;
                        await ctx.SaveChangesAsync().ConfigureAwait(false);
                        result = true;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }

            return result;
        }
    }
}
