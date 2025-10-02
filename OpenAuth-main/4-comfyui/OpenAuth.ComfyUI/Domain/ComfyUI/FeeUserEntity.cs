using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenAuth.ComfyUI.Domain
{
    /// <summary>
    /// 用户状态
    /// 创建人: 麦吉小小
    /// 创建时间:2025-06-20 02:07:23
    ///</summary>
    [Table("fee_user")]
    public class FeeUserEntity : EntityString
    {
        /// <summary>
        ///
        ///</summary>
        [Description("")]
        [MaxLength(50)]
        public string? UserId { get; set; }

        /// <summary>
        ///
        ///</summary>
        [Description("")]
        [MaxLength(50)]
        public string? FeeId { get; set; }

        /// <summary>
        ///
        ///</summary>
        [Description("")]
        public int Expire { get; set; }

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

        /// <summary>
        ///
        ///</summary>
        [Description("")]
        public int Deleted { get; set; }

    }

}