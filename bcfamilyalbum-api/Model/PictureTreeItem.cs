﻿using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bcfamilyalbum_api.Model
{
    public class PictureTreeItem : MediaFileTreeItem
    {
        internal static readonly HashSet<string> PictureExtensions = new HashSet<string>()
        {
            ".jpg",
            ".jpeg"
        };

        public PictureTreeItem(string relativePath, TreeItem parent, string name, string fullPath) : base(relativePath, parent, name, fullPath)
        {
        }

        internal override Task Rotate()
        {
            return Task.Run(() =>
            {
                using (var image = Image.Load(this.FullPath))
                {
                    image.Mutate(x => x.Rotate(RotateMode.Rotate90));
                    image.Save(this.FullPath);
                }
            });
        }

        public static new bool IsAnInstance(string name)
        {
            return IsAnInstance(name, PictureExtensions);
        }
    }
}
