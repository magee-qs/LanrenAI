using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAuth.ComfyUI.Model.Sys
{
    public class ALoginForm: EntityString, IValidateForm
    {
        private ALoginFormValidator validator = new ALoginFormValidator();


        public string Telephone { get; set; }

        public string Password { get; set; }


        public void Validate()
        {
            validator.IsValid(this);
        }
    }

    public class ALoginFormValidator : EntityValidator<ALoginForm>
    {
        public ALoginFormValidator()
        {
            RuleFor(m => m.Telephone).NotEmpty().WithMessage("手机号不能为空");
            RuleFor(m => m.Telephone).Must((acount) =>
            {
                return acount.IsPhoneNumber();
            }).WithMessage("手机号不正确");

            RuleFor(m => m.Password).NotEmpty().WithMessage("短信验证码不能为空");
        }
    }
}
