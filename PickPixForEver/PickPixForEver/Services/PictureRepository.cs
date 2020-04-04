using Microsoft.EntityFrameworkCore;
using PickPixForEver.Models;
using PickPixForEver.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExifLib;
using Xamarin.Forms;

namespace PickPixForEver.Services
{
    public class PictureRepository : IPictureRepository
    {
        PickPixDbContext DbContext;

        public PictureRepository(PickPixDbContext dbContext)
        {
            this.DbContext = dbContext;

        }
        public async Task<int> EnterPicture(Picture picture)
        {
            try
            {
                var tracker = await DbContext.Pictures.AddAsync(picture).ConfigureAwait(false);
                await DbContext.SaveChangesAsync().ConfigureAwait(false);
                //TO-DO: Handle null ID
                //NOTE: should we put something like this in account repository as well?
                /*if (picture.Id == "NULL")
                {

                }*/
                return picture.Id;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 0;
            }
        }
        public async Task<Picture> GetPicture(int ID)
        {
            Picture pic;
            var pic1 = DbContext.Pictures.Select(s => s).FirstOrDefault();
            pic = await DbContext.Pictures.
            Where(s => s.Id == ID).SingleOrDefaultAsync().ConfigureAwait(false);
            return pic1;
        }
        //TODO: Consider whether picture repository should be responsible for creating Picture from Image
        public async Task<int> EnterPictureSource(Image image)
        {
            try
            {
                Picture newPic = new Picture(image);
                await EnterPicture(newPic);
                return newPic.Id;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 0;
            }
        }
    }
}
