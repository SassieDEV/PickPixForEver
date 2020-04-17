using PickPixForEver.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PickPixForEver.Services
{
    public interface IPictureRepository: IDataStore<Picture>
    {
        Task<int> EnterImgDataSource(Stream imgStream);
        Task<int> AddTagAsync(Models.Tag tag);
        Task<Models.Tag> FindTagAsync(int ID);
        Task<bool> InitPic(Stream fileStream, string filePath, IReadOnlyList<MetadataExtractor.Directory> metaDataDirectories);
        Task<int> HandleImageCommit(Dictionary<Stream, string> streams, string[][] megaTags, string[] albums, string privacy, string notes);
    }
}
