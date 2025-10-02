using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenAuth.ComfyUI.Service.ComfyUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAuth.ComfyUI.Controller.ComfyUI
{
    [ApiController]
    [Route("/[controller]/[action]")]
    public class CostController : ControllerBase
    {
        private ICostService costService;
        public CostController(ICostService costService) 
        {
            this.costService = costService;
        }

        [HttpPost]
        public AjaxResult Add([FromQuery] string userId, [FromQuery] string content, 
            [FromQuery] int cost, [FromQuery] DateTime expire)
        {
            costService.AddCost(userId, content, cost, expire);
            return AjaxResult.OK("");
        }


        [HttpPost]
        public AjaxResult Task([FromQuery] string taskId,  [FromQuery] int cost)
        {
            costService.Cost(taskId, cost);
            return AjaxResult.OK("");
        }

        [HttpPost]
        public AjaxResult ReturnCost([FromQuery] string taskId)
        {
            costService.ReturnCost(taskId);
            return AjaxResult.OK("");
        }

        [HttpPost]
        [AllowAnonymous]
        public AjaxResult GetCost()
        { 
            var total = costService.GetCost();
            return AjaxResult.OK(total, "");
        }

        [HttpPost]
        public AjaxResult GetCostList([FromBody] Page page)
        {
            var list = costService.GetCostList(page);
            return AjaxResult.OK(new { rows = list, total = page.total }, "");
        }
    }
}
