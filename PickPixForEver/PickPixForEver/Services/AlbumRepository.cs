using PickPixForEver.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PickPixForEver.Services
{
    public class AlbumRepository : IDataStore<Album>
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

        public Task<bool> UpdateItemAsync(Album item)
        {
            throw new NotImplementedException();
        }
    }
}
