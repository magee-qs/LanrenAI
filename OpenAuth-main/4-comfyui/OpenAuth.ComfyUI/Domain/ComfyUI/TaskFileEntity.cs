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
    /// 任务图片
    /// 创建人: 麦吉小小
    /// 创建时间:2025-06-04 16:03:46
    ///</summary>
    [Table("task_file")]
    public class TaskFileEntity : EntityString
    {
        /// <summary>
        ///
        ///</summary>
        [Description("")]
        [MaxLength(1000)]
        public string? FileName { get; set; }

        /// <summary>
        ///
        ///</summary>
        [Description("")]
        [MaxLength(100)]
        public string? FileType { get; set; }

        /// <summary>
        ///
        ///</summary>
        [Description("")]
        public long? FileSize { get; set; }

        /// <summary>
        ///
        ///</summary>
        [Description("")]
        [MaxLength(1000)]
        public string? FilePath { get; set; }
 

        /// <summary>
        ///
        ///</summary>
        [Description("")]
        [MaxLength(50)]
        public string? FormId { get; set; }

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


        public int Deleted { get; set; }

    }
}
