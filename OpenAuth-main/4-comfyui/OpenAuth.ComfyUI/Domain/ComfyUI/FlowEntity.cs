using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAuth.ComfyUI.Domain.ComfyUI
{
    /// <summary>
    /// 工作流
    /// 创建人: 麦吉小小
    /// 创建时间:2025-05-28 06:22:08
    ///</summary>
    [Table("flow")] 
    public class FlowEntity : EntityString
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
        /// 工作流模板
        /// </summary>
        public string? JsonTemplate { get; set; }

        /// <summary>
        /// 接口
        /// </summary>
        public string? Path { get; set; }

        public string? Service { get; set; }

        /// <summary>
        ///
        ///</summary>
        [Description("")]
        public int State { get; set; }

        /// <summary>
        ///
        ///</summary>
        [Description("")]
        public int Deleted { get; set; }

        /// <summary>
        ///
        ///</summary>
        [Description("")]
        [MaxLength(50)]
        public string? CreateBy { get; set; }

        /// <summary>
        ///
        ///</summary>
        [Description("")]
        public DateTime? CreateTime { get; set; }

        /// <summary>
        ///
        ///</summary>
        [Description("")]
        [MaxLength(50)]
        public string? UpdateBy { get; set; }

        /// <summary>
        ///
        ///</summary>
        [Description("")]
        public DateTime? UpdateTime { get; set; }

    }
}
