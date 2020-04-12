﻿using bcfamilyalbum_api.Model;
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
        Task<TreeItem> GetItem(int id);

        string AlbumDbPath { get; }

        string GetRelativePath(string path);
    }
}
