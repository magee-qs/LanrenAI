using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAuth
{
    public class MediaTypeHelper
    {
        private static readonly Dictionary<string, string> _mimeTypeMappings = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { ".txt", "text/plain" },
            { ".html", "text/html" },
            { ".htm", "text/html" },
            { ".css", "text/css" },
            { ".csv", "text/csv" },
            { ".js", "application/javascript" },
            { ".json", "application/json" },
            { ".xml", "application/xml" },
            { ".pdf", "application/pdf" },
            { ".zip", "application/zip" },
            { ".doc", "application/msword" },
            { ".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document" },
            { ".xls", "application/vnd.ms-excel" },
            { ".xlsx", "application/vnd.openxmlformats-offencedocument.spreadsheetml.sheet" },
            { ".ppt", "application/vnd.ms-powerpoint" },
            { ".pptx", "application/vnd.openxmlformats-officedocument.presentationml.presentation" },
            { ".jpg", "image/jpeg" },
            { ".jpeg", "image/jpeg" },
            { ".png", "image/png" },
            { ".gif", "image/gif" },
            { ".bmp", "image/bmp" },
            { ".tiff", "image/tiff" },
            { ".svg", "image/svg+xml" },
            { ".mp3", "audio/mpeg" },
            { ".wav", "audio/wav" },
            { ".ogg", "audio/ogg" },
            { ".mp4", "video/mp4" },
            { ".avi", "video/x-msvideo" },
            { ".mov", "video/quicktime" },
            { ".wmv", "video/x-ms-wmv" }
        };

        public static string GetMimeType(string fileExt)
        {
            if (fileExt.IsEmpty() || !_mimeTypeMappings.TryGetValue(fileExt, out var miniType))
            {
                //默认二进制文件
                return "application/octet-stream";
            }
            return miniType;
        }


        public static string GetMimeTypeByName(string fileName)
        {
            var index = fileName.LastIndexOf(".");
            var flieExt = fileName.Substring(index);
            return GetMimeType(flieExt);
        }


        public static void AddMimeType(string fileExt, string mimeType)
        {
            if (_mimeTypeMappings.ContainsKey(fileExt))
                return;

            _mimeTypeMappings.Add(fileExt, mimeType);
        }


        public static string GetStrictMimeTypeByName(string fileName)
        {
            var index = fileName.LastIndexOf(".");
            var fileExt = fileName.Substring(index);

            _mimeTypeMappings.TryGetValue(fileExt, out var value);
            return value;
        }
    }
}
