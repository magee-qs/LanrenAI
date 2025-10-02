using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace OpenAuth.ComfyUI.Model.ComfyUI
{
    public class TaskModel : EntityString, IValidateForm
    {
        private TaskFormValidator _validator = new TaskFormValidator();

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
        /// 图片比例
        /// </summary>
        public string Scale { get; set; }

        /// <summary>
        /// 图片比例
        /// </summary>
        public int? Width { get; set; }

        /// <summary>
        /// 图片比例
        /// </summary>
        public int? Height { get; set; }

        /// <summary>
        /// 清晰度 1=> 1k 2 = 2k 3= 3k 4 = 4k
        /// </summary>
        public int? Size { get; set; }


        /// <summary>
        /// 工作流请求数据
        /// </summary>
        public  string FlowJson { get; set; }


        public int  Cost { get; set; }

        public string WebId { get; set; }
         
        public NodeConfig NodeConfig { get; set; }

        public string userId { get; set; }

        public string token { get; set; }

        public string KeywordEN { get; set; }

        public string NegtiveEN { get; set; }

        /// <summary>
        /// 种子 0 随机 
        /// </summary>
        public string Seed { get; set; }
        
        /// <summary>
        /// 种子类型  rand , fixed
        /// </summary>
        public string SeeType { get; set; }


        /// <summary>
        /// 是否自动翻译
        /// </summary>
        public bool translacte { get; set; }

        public void Validate()
        {
            _validator.IsValid(this);
        }

        class TaskFormValidator : EntityValidator<TaskModel>
        {
            public TaskFormValidator()
            {
                RuleFor(m => m.FlowId).NotEmpty().WithMessage("工作流不能为空");

                RuleFor(m => m.Unit).Must(( count ) => 
                {
                    return count > 0 && count <= 10;
                }).WithMessage("图片数量在1-8之间");

                
            }
        } 
    }


     
    
}
