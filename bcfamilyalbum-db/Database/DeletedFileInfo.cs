using System;
using System.Collections.Generic;
using System.Text;

namespace bcfamilyalbum_db
{
    public class DeletedFileInfo : FileInfo
    {
        public DateTime? RemovalTimestamp { get; set; }

        public DeletedFileInfo() : base() { }

        public DeletedFileInfo(string relativePath) : base(relativePath) { }
    }
}
