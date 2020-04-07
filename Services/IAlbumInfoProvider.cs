using bcfamilyalbum_back.Interfaces;
using bcfamilyalbum_back.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bcfamilyalbum_back.Services
{
    public class AlbumInfoProvider : IAlbumInfoProvider
    {
        public Task<List<TreeItem>> GetAlbumInfo()
        {
            throw new NotImplementedException();
        }
    }
}
