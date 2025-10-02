using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAuth.ComfyUI.Model.ComfyUI.Node
{
    /// <summary>
    /// 抠图参数
    /// </summary>
    public class PicExtModel
    {
        public int left { get; set; }

        public int right { get; set; }

        public int top { get; set; }

        public int bottom { get; set; }

        /// <summary>
        /// 边缘羽化
        /// </summary>
        public int edge { get; set; }
    }
}
