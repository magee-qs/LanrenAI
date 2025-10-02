using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAuth.ComfyUI.Model.ComfyUI
{
    /// <summary>
    /// 奖励发放参数
    /// </summary>
    public class ExecuteCostModel
    {
        /// <summary>
        /// 用户
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 到期月份
        /// </summary>
        public DateTime Month { get; set; } 

        /// <summary>
        /// 发放点数
        /// </summary>
        public int Cost { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Content { get; set; }
    }
}
