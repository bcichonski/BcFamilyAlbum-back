using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace bcfamilyalbum_back.Model
{
    public class DirectoryTreeItem : TreeItem
    {
        public DirectoryTreeItem(int id, TreeItem parent, string fullPath) : base(id, parent, GetDirectoryName(fullPath), fullPath)
        {
        }

        public DirectoryTreeItem(int id, TreeItem parent, string name, string fullPath) : base(id, parent, name, fullPath)
        {
        }

        public static string GetDirectoryName(string currentDir)
        {
            return Path.GetFileNameWithoutExtension(Path.TrimEndingDirectorySeparator(currentDir));
        }
    }
}
