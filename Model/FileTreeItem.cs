using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bcfamilyalbum_back.Model
{
    public class FileTreeItem : TreeItem
    {
        public FileTreeItem(int id, TreeItem parent, string name, string fullPath) : base(id, parent, name, fullPath)
        {
        }
    }
}
