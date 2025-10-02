using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAuth.ComfyUI.Model.SYS
{
    public class LoginForm : EntityLong, IValidateForm
    {

        private LoginFormValidator _validator;

        public LoginForm()
        {
            _validator = new LoginFormValidator(); 
        }
        /// <summary>
        /// 手机号
        /// </summary>
        public string Telephone { get; set; }
         

        /// <summary>
        /// 短信验证码
        /// </summary>
        public string VerifyCode { get; set; }


        


        public void Validate()
        {
            _validator.IsValid(this);
        }
    }



    public class LoginFormValidator : EntityValidator<LoginForm>
    {
        public LoginFormValidator()
        {
            RuleFor(m => m.Telephone).NotEmpty().WithMessage("手机号不能为空");
            RuleFor(m => m.Telephone).Must((acount) =>
            {
                return acount.IsPhoneNumber();
            }).WithMessage("手机号不正确");

            RuleFor(m => m.VerifyCode).NotEmpty().WithMessage("短信验证码不能为空");
        }
    }
}
