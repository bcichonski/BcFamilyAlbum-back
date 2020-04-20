using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace bcfamilyalbum_api.Model
{
    public class MediaFileTreeItem : FileTreeItem
    {
        private static readonly HashSet<string> MediaFileExtensions = new HashSet<string>(
            PictureTreeItem.PictureExtensions.Concat(VideoTreeItem.VideoExtensions)
        );

        public MediaFileTreeItem(string relativePath, TreeItem parent, string name, string fullPath) : base(relativePath, parent, name, fullPath)
        {
        }

        public static bool IsAnInstance(string name)
        {
            return IsAnInstance(name, MediaFileExtensions);
        }

        protected static bool IsAnInstance(string name, HashSet<string> extensions)
        {
            var ext = Path.GetExtension(name).ToLowerInvariant();
            return extensions.Contains(ext);
        }

        public static MediaFileTreeItem GetNew(string relativePath, TreeItem parent, string name, string fullPath)
        {
            if (IsAnInstance(fullPath))
            {
                if (PictureTreeItem.IsAnInstance(fullPath))
                {
                    return new PictureTreeItem(relativePath, parent, name, fullPath);
                }
                else if (VideoTreeItem.IsAnInstance(fullPath))
                {
                    return new VideoTreeItem(relativePath, parent, name, fullPath);
                }
                else
                {
                    throw new Exception($"Unexpected error occured. Media file that is neither a picture nor a video was found: {fullPath}");
                }
            }
            else throw new Exception("Unsupported media type");
        }
    }
}
