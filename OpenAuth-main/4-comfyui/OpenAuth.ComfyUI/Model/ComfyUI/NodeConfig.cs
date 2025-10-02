using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAuth.ComfyUI.Model.ComfyUI
{
    /// <summary>
    /// 运行基本参数
    /// </summary>
    public class NodeConfig
    {
        /// <summary>
        /// 任务执行时长(分钟)
        /// </summary>
        public int Runtime { get; set; }

        /// <summary>
        /// 轮询等待时间(秒)
        /// </summary>
        public int WaitTime { get; set; }

        /// 其他参数
         

        
    }
}
