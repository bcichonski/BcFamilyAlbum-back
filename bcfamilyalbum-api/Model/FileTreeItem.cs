using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace bcfamilyalbum_api.Model
{
    public class FileTreeItem : TreeItem
    {
        public FileTreeItem(string id, TreeItem parent, string name, string fullPath) : base(id, parent, name, fullPath)
        {
        }

        internal override async Task MoveTo(string newPath, TreeItem trashNode)
        {
            try
            {
                await this.Lock();
                if (!Directory.Exists(newPath))
                {
                    Directory.CreateDirectory(newPath);
                }
                var newFullPath = Path.Combine(newPath, Path.GetFileName(this.FullPath));

                if (!File.Exists(newFullPath))
                {
                    File.Move(this.FullPath, newFullPath);
                    this.FullPath = newFullPath;
                    this.Parent?.RemoveChild(this);
                    trashNode.AddChild(this);
                    return;
                }
                throw new Exception($"File {newFullPath} already exists");
            } catch
            {
                throw;
            } finally
            {
                this.ReleaseLock();
            }
        }
    }
}
