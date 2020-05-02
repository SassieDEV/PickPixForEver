using PickPixForEver.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace PickPixForEver.Services
{
    public interface IPictureRepository: IDataStore<Picture>
    {
        Task<int> EnterImgDataSource(Stream imgStream);
        Task<int> AddTagAsync(Models.Tag tag);
        Task<Models.Tag> FindTagAsync(int ID);
        Task<IEnumerable<Models.Tag>> FindTagByPictureIdAsync(int PictureId);
        Task<bool> InitPic(Stream fileStream, string filePath, IReadOnlyList<MetadataExtractor.Directory> metaDataDirectories);
        Task<int> HandleImageCommit(int userId, Dictionary<Stream, string> streams, string[][] megaTags, int albumId, string privacy, string notes);
    }
}
