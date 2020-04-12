using System;
using System.Collections.Generic;
using System.Text;

namespace bcfamilyalbum_db
{
    public class FileInfo
    {
        public int Id { get; set; }
        public string RelativePath { get; set; }

        public FileInfo() { }
        public FileInfo(string relativePath)
        {
            RelativePath = relativePath;
        }
    }
}
