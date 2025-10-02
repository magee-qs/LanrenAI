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
    /// 用户表
    /// 创建人: 麦吉小小
    /// 创建时间:2025-05-27 12:29:23
    ///</summary>
    [Table("sys_user")]
    public class UserEntity : EntityString
    {
        /// <summary>
        ///账号
        ///</summary>
        [Description("账号")]
        [MaxLength(100)]
        public string? Account { get; set; }

        /// <summary>
        ///用户名
        ///</summary>
        [Description("用户名")]
        [MaxLength(100)]
        public string? UserName { get; set; }

        /// <summary>
        ///密码
        ///</summary>
        [Description("密码")]
        [MaxLength(100)]
        public string? Password { get; set; }

        /// <summary>
        ///类型
        ///</summary>
        [Description("类型")]
        [MaxLength(100)]
        public string? UserLevel { get; set; }

        /// <summary>
        ///头像
        ///</summary>
        [Description("头像")]
        [MaxLength(1000)]
        public string? Avatar { get; set; }

        /// <summary>
        ///电话
        ///</summary>
        [Description("电话")]
        [MaxLength(50)]
        public string? Phone { get; set; }

        /// <summary>
        ///邮箱
        ///</summary>
        [Description("邮箱")]
        [MaxLength(100)]
        public string? Email { get; set; }

        /// <summary>
        ///
        ///</summary>
        [Description("")]
        [MaxLength(50)]
        public string? TenantId { get; set; }

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
