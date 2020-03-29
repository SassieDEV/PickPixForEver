using PickPixForEver.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PickPixForEver.Services
{
    public interface IPictureRepository
    {
        Task<int> EnterPicture(Picture picture);
        Task<Picture> GetPicture(int ID);
    }
}
