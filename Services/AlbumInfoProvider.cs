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
        private static readonly List<TreeItem> Tree = new List<TreeItem>
        {
            new TreeItem(1,0,"2011"),
            new TreeItem(2,1,"may"),
            new TreeItem(3,2,"pictures"),
            new TreeItem(4,0,"2012"),
            new TreeItem(5,0,"2013"),
        };

        public async Task<List<TreeItem>> GetAlbumInfo()
        {
            return Tree;
        }
    }
}
