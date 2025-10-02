using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace OpenAuth.ComfyUI.Model.System
{
    public class JobModel : EntityLong, IValidateForm
    {
        JobModelValidator validator = new JobModelValidator();

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

        public void Validate()
        {
            validator.IsValid(this);
        }
    }

    public class JobModelValidator : EntityValidator<JobModel>
    {
        public JobModelValidator() 
        {
            RuleFor(m => m.JobName).NotEmpty().WithMessage("任务名不能为空");
            RuleFor(m => m.ClassName).NotEmpty().WithMessage("任务实例不能为空");
            RuleFor(m => m.Cron).NotEmpty().WithMessage("cron不能为空");
        }
    }
}
