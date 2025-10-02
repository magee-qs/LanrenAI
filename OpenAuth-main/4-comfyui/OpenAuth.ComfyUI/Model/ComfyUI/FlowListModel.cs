using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAuth.ComfyUI.Model.ComfyUI
{
    /// <summary>
    /// 工作流列表展示
    /// </summary>
    public class FlowListModel : EntityString
    {
        /// <summary>
        ///流程名
        ///</summary>
        [Description("流程名")]
        [MaxLength(100)]
        public string? Name { get; set; }

        /// <summary>
        ///图片封面
        ///</summary>
        [Description("图片封面")]
        [MaxLength(1000)]
        public string? Img { get; set; }

        /// <summary>
        ///描述
        ///</summary>
        [Description("描述")]
        [MaxLength(1000)]
        public string? Description { get; set; }

        /// <summary>
        ///费用
        ///</summary>
        [Description("费用")]
        public int Cost { get; set; }


        [Description("排序")]
        public int Sort { get; set; }

        /// <summary>
        ///
        ///</summary>
        [Description("")]
        [MaxLength(100)]
        public string? Version { get; set; }

        /// <summary>
        /// 路由参数
        /// </summary>
        public string? Route { get; set; }

        /// <summary>
        /// 路由组件
        /// </summary>
        public string? Component { get; set; }


      

        /// <summary>
        /// 接口
        /// </summary>
        public string? Path { get; set; }

        /// <summary>
        /// 服务接口
        /// </summary>
        public string? Service { get; set; }

        /// <summary>
        ///
        ///</summary>
        [Description("")]
        public int State { get; set; }

       
    }
}
