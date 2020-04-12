using System;
using System.Collections.Generic;
using System.Text;

namespace bcfamilyalbum_db
{
    public class MovedFileInfo : FileInfo
    {
        public DateTime MovingTimestamp { get; set; }
        public string OriginalRelativePath { get; set; }

        public MovedFileInfo() : base() { }

        public MovedFileInfo(string from, string to) : base(to)
        {
            OriginalRelativePath = from;
        }
    }
}
