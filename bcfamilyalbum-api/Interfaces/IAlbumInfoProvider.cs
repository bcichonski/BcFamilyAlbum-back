using bcfamilyalbum_api.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bcfamilyalbum_api.Interfaces
{
    public interface IAlbumInfoProvider
    {
        Task<TreeItem> GetAlbumInfo();

        public void Invalidate();
        Task<TreeItem> GetItem(string id);

        string AlbumDbPath { get; }

        string GetRelativePath(string path);
        Task<TreeItem> DeleteItem(string id);
        Task RotateItem(string id);
    }
}
