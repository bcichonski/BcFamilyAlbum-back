using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bcfamilyalbum_api.Model
{
    public class VideoTreeItem : MediaFileTreeItem
    {
        internal static readonly HashSet<string> VideoExtensions = new HashSet<string>()
        {
            ".mp4",
            ".avi",
            ".m4v",
            ".ogv",
            ".mkv",
            ".flv",
            ".mpg",
            ".mpeg"
        };

        public VideoTreeItem(string relativePath, TreeItem parent, string name, string fullPath) : base(relativePath, parent, name, fullPath)
        {
        }

        public static new bool IsAnInstance(string name)
        {
            return IsAnInstance(name, VideoExtensions);
        }
    }
}
