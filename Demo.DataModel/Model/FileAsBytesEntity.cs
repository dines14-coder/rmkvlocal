using System;
using System.Collections.Generic;
using System.Text;

namespace RmKV.NXT.SilkWARP.DataModel.Model.Shared
{
    public class FileAsBytesEntity
    {
        public byte[] file { get; set; }
        public string Type { get; set; }
        public string extension { get; set; }
        public string FileName { get; set; }
    }

    public class ReturnFileWithName
    {
        public string doument_Path { get; set; }
        public string response { get; set; }
    }
}
