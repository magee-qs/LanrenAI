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
    /// 消费明细
    /// 创建人: 麦吉小小
    /// 创建时间:2025-05-31 06:01:56
    ///</summary>
    [Table("cost_list")]
    public class CostListEntity : EntityString
    {
        /// <summary>
        ///任务id
        ///</summary>
        [Description("任务id")]
        [MaxLength(50)]
        public string? TaskId { get; set; }

        /// <summary>
        ///
        ///</summary>
        [Description("")]
        [MaxLength(50)]
        public string? CostId { get; set; }

        /// <summary>
        ///消费点数
        ///</summary>
        [Description("消费点数")]
        public int Cost { get; set; }

        /// <summary>
        ///消费说明
        ///</summary>
        [Description("消费说明")]
        [MaxLength(1000)]
        public string? Content { get; set; }

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

    }
}
