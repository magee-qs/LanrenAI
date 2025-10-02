using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAuth 
{
    /// <summary>
    /// 文件上传
    /// </summary>
    public class FileDTO
    {
        public string Id { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; } 
        public long FileSize { get; set; } 
        public string FileType { get; set; }
    }
}
