using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAuth.ComfyUI.Model.ComfyUI
{
    /// <summary>
    /// comfyui 图片上传响应
    /// </summary>
    public class FileUploadContent
    {
        public string Name { get; set; }

        public string Subfolder { get; set; }

        public string Type { get; set; }
    }
}
