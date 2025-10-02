using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenAuth.ComfyUI.Domain
{
    /// <summary>
    /// 会员充值
    /// 创建人: 麦吉小小
    /// 创建时间:2025-06-20 02:06:20
    ///</summary>
    [Table("fee_order")]
    public class FeeOrderEntity : EntityString
    {
        /// <summary>
        ///订单编号
        ///</summary>
        [Description("订单编号")]
        [MaxLength(1000)]
        public string? OrderId { get; set; }

        public string? UserId { get; set; }

        /// <summary>
        ///
        ///</summary>
        [Description("")]
        [MaxLength(50)]
        public string? FeeId { get; set; }

        /// <summary>
        ///金额
        ///</summary>
        [Description("金额")]
        public int Cost { get; set; }

        /// <summary>
        ///有效时长
        ///</summary>
        [Description("有效时长")]
        public int Month { get; set; }

        /// <summary>
        ///支付方式
        ///</summary>
        [Description("支付方式")]
        [MaxLength(100)]
        public string? PayType { get; set; }

        /// <summary>
        ///支付金额
        ///</summary>
        [Description("支付金额")]
        public int Amount { get; set; }

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

        /// <summary>
        ///0 支付中 1 支付完成 -1 作废
        ///</summary>
        [Description("0 支付中 1 支付完成 -1 作废")]
        public int State { get; set; }

    }

}