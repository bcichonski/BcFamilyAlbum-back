using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bcfamilyalbum_back.Model
{
    public class TreeItem
    {
        public TreeItem(int id, int parentId, string name, string fullPath)
        {
            Id = id;
            ParentId = parentId;
            Name = name;
            _fullPath = fullPath;
        }

        public int Id { get; set; }

        public int ParentId { get; set; }

        public string Name { get; set; }

        string _fullPath;

        public string GetFullPath()
        {
            return _fullPath;
        }
    }
}
