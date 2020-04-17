using PickPixForEver.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PickPixForEver.Services
{
    public interface ITagRepository: IDataStore<Tag>
    {
        IEnumerable<Picture> GetTaggedPictures(int tagId);
    }
}
