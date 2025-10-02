using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAuth.ComfyUI.Model.ComfyUI
{
    public class FlowListQueryModel 
    {
        /// <summary>
        /// 流程类别
        /// </summary>
        public string FlowTypeId { get; set; }

        /// <summary>
        /// 分页参数
        /// </summary>
        public EasyPage EasyPage { get; set; }
    }
}
