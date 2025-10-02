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
    /// 消费点数
    /// 创建人: 麦吉小小
    /// 创建时间:2025-05-31 06:01:19
    ///</summary>
    [Table("cost")]
    public class CostEntity : EntityString
    {
        /// <summary>
        ///
        ///</summary>
        [Description("")]
        [Required]
        [MaxLength(50)]
        public string UserId { get; set; }

        /// <summary>
        ///总数
        ///</summary>
        [Description("总数")]
        public int Total { get; set; }

        /// <summary>
        ///消耗点数
        ///</summary>
        [Description("消耗点数")]
        public int Cost { get; set; }

        /// <summary>
        ///剩余点数
        ///</summary>
        [Description("剩余点数")]
        public int Leave { get; set; }

        /// <summary>
        ///
        ///</summary>
        [Description("")]
        public int Exipre { get; set; }

        /// <summary>
        ///
        ///</summary>
        [Description("")]
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
        public int State { get; set; }

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
