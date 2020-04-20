using bcfamilyalbum_api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace bcfamilyalbum_api.Model
{
    public class TreeItem
    {
        protected TreeItem(string relative_path, TreeItem parent, string name, string fullPath)
        {
            Id = GetId(relative_path);
            Name = name;
            Children = null;

            Parent = parent;
            FullPath = fullPath;

            if(parent != null)
            {
                parent.AddChild(this);
            }
        }

        public string Id { get; private set; }

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

        internal void RemoveChild(TreeItem child)
        {
            Children.Remove(child);
        }

        internal virtual Task Rotate()
        {
            throw new NotImplementedException();
        }

        static readonly MD5 _md5 = MD5.Create();

        static string GetId(string path)
        {
            var data = _md5.ComputeHash(Encoding.UTF8.GetBytes(path));

            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }
    }
}
