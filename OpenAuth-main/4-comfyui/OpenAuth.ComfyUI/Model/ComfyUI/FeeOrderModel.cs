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
    public class FeeOrderModel : EntityString, IValidateForm
    {
        private  FormValidator _validator;

        public FeeOrderModel()
        {
            _validator = new FormValidator();
        }

        public void Validate()
        {
            _validator.IsValid(this);
        }
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


        class FormValidator : EntityValidator<FeeOrderModel>
        {
            public FormValidator()
            {
                RuleFor(m => m.FeeId).NotEmpty().WithMessage("会员类型不能为空");

                RuleFor(m => m.UserId).NotEmpty().WithMessage("会员id不能为空");
            }
        }
    }
}
