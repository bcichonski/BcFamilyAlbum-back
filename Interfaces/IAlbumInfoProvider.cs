using bcfamilyalbum_back.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bcfamilyalbum_back.Interfaces
{
    interface IAlbumInfoProvider
    {
        Task<List<TreeItem>> GetAlbumInfo();
    }
}
