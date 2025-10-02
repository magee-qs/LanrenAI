using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenAuth.ComfyUI.Domain
{
    /// <summary>
    /// 用户类型
    /// 创建人: 麦吉小小
    /// 创建时间:2025-06-19 21:03:39
    ///</summary>
    [Table("fee_level")]
    public class FeeLevelEntity : EntityString
    {
        /// <summary>
        ///类型
        ///</summary>
        [Description("类型")]
        [MaxLength(100)]
        public string? UserLevel { get; set; }

        /// <summary>
        ///
        ///</summary>
        [Description("")]
        [MaxLength(100)]
        public string? Title { get; set; }

        /// <summary>
        ///点数
        ///</summary>
        [Description("点数")]
        public int Cost { get; set; }

        /// <summary>
        ///并发数
        ///</summary>
        [Description("并发数")]
        public int Multiple { get; set; }

        /// <summary>
        ///存储
        ///</summary>
        [Description("存储")]
        public int Store { get; set; }

        /// <summary>
        ///状态
        ///</summary>
        [Description("状态")]
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