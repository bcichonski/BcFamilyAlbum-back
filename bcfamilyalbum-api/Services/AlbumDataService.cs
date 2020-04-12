using bcfamilyalbum_api.Interfaces;
using bcfamilyalbum_db.Interfaces;
using bcfamilyalbum_db.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bcfamilyalbum_api.Services
{
    public class AlbumDataService : FamilyAlbumDataService, IFamilyAlbumDataService
    {
        public AlbumDataService(IAlbumInfoProvider _albumInfo) : base(_albumInfo.AlbumDbPath)
        {
        }
    }
}
