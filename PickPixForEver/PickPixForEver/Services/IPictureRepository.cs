using PickPixForEver.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PickPixForEver.Services
{
    public interface IPictureRepository
    {
        Task<int> EnterPicture(Picture picture);
        Task<IEnumerable<Picture>> GetPictures();
        Task<Picture> GetPicture(int id);
        Task<int> EnterPictureSource(Image image);

        Task<int> EnterImgDataSource(Stream imgStrea);
    }
}
