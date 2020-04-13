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
        public async Task<bool> AddItemAsync(Album album)
        {
            bool result = false;
            try
            {
                using(var cxt = new PickPixDbContext(this.filePath))
                {
                    album.CreatedAt = DateTime.Now;
                    album.UpdatedAt = DateTime.Now;
                    album.Active = true;
                    cxt.Albums.Add(album);
                    await cxt.SaveChangesAsync().ConfigureAwait(false);
                    result = true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return result;
        }

        public Task<bool> DeleteItemAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Album> FindItemAsync(int id)
        {
            throw new NotImplementedException();
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
                    pictures = await Task.FromResult(ctx.Pictures.Where(P => P.AlbumId == albumId).ToList()).ConfigureAwait(false);
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
