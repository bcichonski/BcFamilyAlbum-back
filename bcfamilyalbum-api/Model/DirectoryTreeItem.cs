using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace bcfamilyalbum_api.Model
{
    public class DirectoryTreeItem : TreeItem
    {
        public DirectoryTreeItem(string relativePath, TreeItem parent, string fullPath) : base(relativePath, parent, GetDirectoryName(fullPath), fullPath)
        {
        }

        public DirectoryTreeItem(string relativePath, TreeItem parent, string name, string fullPath) : base(relativePath, parent, name, fullPath)
        {
        }

        public static string GetDirectoryName(string currentDir)
        {
            return Path.GetFileNameWithoutExtension(Path.TrimEndingDirectorySeparator(currentDir));
        }
    }
}
