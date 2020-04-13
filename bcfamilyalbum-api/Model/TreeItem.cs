using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bcfamilyalbum_api.Model
{
    public class TreeItem
    {
        public TreeItem(int id, TreeItem parent, string name, string fullPath)
        {
            Id = id;
            Name = name;
            Children = null;

            Parent = parent;
            FullPath = fullPath;

            if(parent != null)
            {
                parent.AddChild(this);
            }
        }

        public int Id { get; private set; }

        internal TreeItem Parent { get; private set; }

        public List<TreeItem> Children { get; private set; }

        public string Name { get; private set; }

        internal string FullPath { get; private set; }

        public void AddChild(TreeItem child)
        {
            if(Children == null)
            {
                Children = new List<TreeItem>();
            }
            Children.Add(child);
        }

        internal virtual void MoveTo(string newPath)
        {
            throw new NotImplementedException();
        }
    }
}
