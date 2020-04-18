using PickPixForEver.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PickPixForEver.Services
{
    public interface IAlbumRepository: IDataStore<Album>
    {
        Task<IEnumerable<Picture>> GetAlbumPictures(int albumId);
    }
}
