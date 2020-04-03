using Microsoft.EntityFrameworkCore;
using PickPixForEver.Models;
using PickPixForEver.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PickPixForEver.Services
{
    public class PictureRepository : IPictureRepository
    {
        private readonly string filePath;

        public PictureRepository(string filePath)
        {
            this.filePath = filePath;

        }
        public async Task<int> EnterPicture(Picture picture)
        {
            try
            {
                using (var dbContext = new PickPixDbContext(filePath))
                {
                    var tracker = await dbContext.Pictures.AddAsync(picture).ConfigureAwait(false);
                    await dbContext.SaveChangesAsync().ConfigureAwait(false);
                }
                //TO-DO: Handle null ID
                //NOTE: should we put something like this in account repository as well?
                /*if (picture.Id == "NULL")
                {

                }*/
                return picture.Id;
            }
            catch (Exception)
            {

                return 0;
            }
        }
        public async Task<Picture> GetPicture(int ID)
        {
            Picture pic;

            using (var dbContext = new PickPixDbContext(filePath)) {
                var pic1 = dbContext.Pictures.Select(s => s).FirstOrDefault();
                pic = await dbContext.Pictures.
                Where(s => s.Id == ID).SingleOrDefaultAsync().ConfigureAwait(false);
                return pic1;
            }
        }
    }
}
