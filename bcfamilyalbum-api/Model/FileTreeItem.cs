using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace bcfamilyalbum_api.Model
{
    public class FileTreeItem : TreeItem
    {
        public FileTreeItem(int id, TreeItem parent, string name, string fullPath) : base(id, parent, name, fullPath)
        {
        }

        internal override void MoveTo(string newPath)
        {
            if(!Directory.Exists(newPath))
            {
                Directory.CreateDirectory(newPath);
                var newName = Path.Combine(newPath, Path.GetFileName(this.FullPath));

                if(!File.Exists(newName))
                {
                    File.Move(this.FullPath, newName);
                    return;
                }
                throw new Exception($"File {newName} already exists");
            }
            throw new Exception($"Path {newPath} not found");
        }
    }
}
