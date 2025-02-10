using System;
using System.Collections.Generic;
using System.Text;

namespace RmKV.NXT.SilkWARP.DataModel.Model.Shared
{
    public class GetFileStream
    {
        public int base_path_id { get; set; }
        public string base_path { get; set; }
        public string base_path_url { get; set; }
    }

    public class GetFileNames
    {
        public string file_path1 { get; set; }
        public string file_path2 { get; set; }
        public string file_path3 { get; set; }
        public string file_path4 { get; set; }
    }
    public class FilePath
    {
        public int base_path_id { get; set; }
        public string file_path { get; set; }
    }
    public class FileAfterAttachment
    {
        public List<Files> removedFile { get; set; }
        public List<Files> attachedFiles { get; set; }
    }
    public class Files
    {
        public int dataIndex { get; set; }
        public string file_path { get; set; }
    }
}
