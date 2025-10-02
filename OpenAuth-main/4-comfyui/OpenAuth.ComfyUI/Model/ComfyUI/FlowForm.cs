using OpenAuth.ComfyUI.Model.SYS;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace OpenAuth.ComfyUI.Model.ComfyUI
{
    public class FlowForm:EntityString, IValidateForm
    {

        private FlowFormValidator _validator; 
        public string? Name { get; set; }

        
        public string? Img { get; set; }

       
        public string? Description { get; set; }

        public string? Route { get; set; }

        public string? Component { get; set; }


        public int Cost { get; set; }

         
        public int Sort { get; set; }

        
        public string? Version { get; set; }

        public string? JsonTemplate { get; set; }

        public string? Service { get; set; }

        public FlowForm()
        {
            _validator = new FlowFormValidator();
        }

        public void Validate()
        {
            _validator.IsValid(this);
        }

        class FlowFormValidator : EntityValidator<FlowForm>
        {
            public FlowFormValidator()
            {
                RuleFor(m => m.Name).NotEmpty().WithMessage("工作流名称不能为空")
                    .MaximumLength(100).WithMessage("工作流名称长度100以内");

                RuleFor(m => m.Description).NotEmpty().WithMessage("工作流描述不能为空")
                    .MaximumLength(1000).WithMessage("工作流描述长度100以内");

                RuleFor(m => m.Route).NotEmpty().WithMessage("路由参数不能为空");

                RuleFor(m => m.Component).NotEmpty().WithMessage("路由参数不能为空");
            }
        }
    }
}
