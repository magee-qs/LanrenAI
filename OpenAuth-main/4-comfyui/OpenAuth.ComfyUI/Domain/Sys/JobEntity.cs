using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAuth.ComfyUI.Domain.Sys
{
    /// <summary>
    /// 自动任务

    /// 创建人: 麦吉小小
    /// 创建时间:2025-06-01 17:09:23
    ///</summary>
    [Table("quartz_job")]
    public class JobEntity : EntityString
    {
        /// <summary>
        ///任务
        ///</summary>
        [Description("任务")]
        [MaxLength(1000)]
        public string? JobName { get; set; }

        /// <summary>
        ///类名
        ///</summary>
        [Description("类名")]
        [MaxLength(1000)]
        public string? ClassName { get; set; }

        /// <summary>
        ///cron表达式
        ///</summary>
        [Description("cron表达式")]
        [MaxLength(1000)]
        public string? Cron { get; set; }

        /// <summary>
        ///运行参数
        ///</summary>
        [Description("运行参数")]
        [MaxLength(1000)]
        public string? Parameter { get; set; }

        /// <summary>
        ///任务描述
        ///</summary>
        [Description("任务描述")]
        [MaxLength(1000)]
        public string? Description { get; set; }

        /// <summary>
        ///状态 0  等待 1 运行
        ///</summary>
        [Description("状态 0  等待 1 运行 ")]
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
