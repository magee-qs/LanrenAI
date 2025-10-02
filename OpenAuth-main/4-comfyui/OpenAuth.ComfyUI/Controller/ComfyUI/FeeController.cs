using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenAuth.ComfyUI.Domain;
using OpenAuth.ComfyUI.Model.ComfyUI;
using OpenAuth.ComfyUI.Service.ComfyUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAuth.ComfyUI.Controller.ComfyUI
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class FeeController : ControllerBase
    {
        private IFeeLevelService feeLevelService;

        private IFeeOrderService orderService;

        private IFeeUserService feeUserService;
        public FeeController(IFeeLevelService feeLevelService, IFeeOrderService orderService, IFeeUserService feeUserService)
        {
            this.feeLevelService = feeLevelService;
            this.orderService = orderService;
            this.feeUserService = feeUserService;
        }

        [HttpPost]
        public AjaxResult AddFee([FromBody]FeeLevelEntity feeLevelEntity)
        { 
            feeLevelService.Insert(feeLevelEntity);
            feeLevelService.SaveChanges();
            return AjaxResult.OK("");
        }

        [HttpPost]
        public AjaxResult SaveOrder([FromBody] FeeOrderModel feeOrderModel)
        {
            orderService.AddOrder(feeOrderModel);
            return AjaxResult.OK("");
        }
        [HttpPost]
        public AjaxResult PayOrder([FromBody] string orderId)
        {
            orderService.PayOrder(orderId);
            return AjaxResult.OK("");
        }
        [HttpPost]
        public AjaxResult AbandonOrder([FromBody] string orderId)
        {
            orderService.AbandonOrder(orderId);
            return AjaxResult.OK("");
        }

        [HttpPost] 
        public AjaxResult OrderList([FromBody] Page page)
        {
            var list =  orderService.OrderList(page);
            return AjaxResult.OK(new { rows = list, page.total }, "ok");
        }

        [HttpPost]
        public AjaxResult GetFeeLevels()
        {
            var list = feeLevelService.Queryable().Where(t => t.State == 1)
                .Select(t => new { t.Id, t.Title }).ToList();

            return AjaxResult.OK(list, "");
                

        }

        /// <summary>
        /// 发放点券
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public  AjaxResult AutoExecute([FromBody] ExecuteCostModel model)
        { 
            feeUserService.AutoExecute(model.Month);  
            return AjaxResult.OK("");
        }

        /// <summary>
        /// 发放点券
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public AjaxResult Execute([FromBody] ExecuteCostModel model)
        {
            feeUserService.Execute(model);
            return AjaxResult.OK("");
        }
    }
}
