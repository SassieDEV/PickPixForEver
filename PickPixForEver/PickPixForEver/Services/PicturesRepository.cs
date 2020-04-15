using PickPixForEver.Models;
using System;
using System.Collections.Generic;
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

        public Task<IEnumerable<Picture>> GetItemsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Picture>> GetItemsAsync(string searchTerm)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateItemAsync(Picture item)
        {
            throw new NotImplementedException();
        }
    }
}
