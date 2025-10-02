using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;

namespace OpenAuth.ComfyUI.Domain.ComfyUI
{
    /// <summary>
    /// 工作流任务
    /// 创建人: 麦吉小小
    /// 创建时间:2025-05-28 08:34:03
    ///</summary>
    [Table("task")] 
    public class TaskEntity : EntityString
    {
        /// <summary>
        ///
        ///</summary>
        [Description("")]
        [MaxLength(50)]
        public string? FlowId { get; set; }

        /// <summary>
        ///图片数量
        ///</summary>
        [Description("图片数量")]
        public int Unit { get; set; }

        /// <summary>
        ///单价
        ///</summary>
        [Description("单价")]
        public int Cost { get; set; }

        /// <summary>
        ///费用
        ///</summary>
        [Description("费用")]
        public int Amount { get; set; }

        /// <summary>
        ///关键字
        ///</summary>
        [Description("关键字")]
        [MaxLength(1000)]
        public string? Keyword { get; set; }

        /// <summary>
        ///负面词
        ///</summary>
        [Description("负面词")]
        [MaxLength(1000)]
        public string? Negtive { get; set; }

        /// <summary>
        ///
        ///</summary>
        [Description("")]
        [MaxLength(50)]
        public string? Scale { get; set; }

        /// <summary>
        ///
        ///</summary>
        [Description("")]
        public int? Width { get; set; }

        /// <summary>
        ///
        ///</summary>
        [Description("")]
        public int? Height { get; set; }

        /// <summary>
        ///用时
        ///</summary>
        [Description("用时")]
        public int CostTime { get; set; }

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
        [MaxLength(50)]
        public string? UpdateBy { get; set; }

        /// <summary>
        ///
        ///</summary>
        [Description("")]
        public DateTime? CreateTime { get; set; }

        /// <summary>
        ///
        ///</summary>
        [Description("")]
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        ///
        ///</summary>
        [Description("")]
        public int Deleted { get; set; }

        /// <summary>
        ///
        ///</summary>
        [Description("")]
        public int State { get; set; }

        /// <summary>
        ///
        ///</summary>
        [Description("")]
        public string? FileJson { get; set; }
          

    }
}
