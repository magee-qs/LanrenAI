using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenAuth.ComfyUI.Domain.ComfyUI;
using OpenAuth.ComfyUI.Model.ComfyUI;
using OpenAuth.ComfyUI.Service.ComfyUI;
 

namespace OpenAuth.ComfyUI.Controller.ComfyUI
{
    [ApiController]
    [Route("/[controller]/[action]")]
    public class FlowController : ControllerBase
    { 
        private IFlowService flowService;
        public FlowController(IFlowService flowService) 
        {
            this.flowService = flowService;
        }

        [HttpPost] 
        [AllowAnonymous]
        public AjaxResult List()
        { 
            var list = flowService.Queryable().Where(t=>t.State == 1).OrderBy(t=>t.Sort).ToList();
            var query = list.MapToList<FlowListModel>();
            return AjaxResult.OK(query, "");
        }


        [HttpPost]
        public AjaxResult Add([FromBody] FlowForm flowForm)
        {
            flowForm.Validate();
            var flowEntity = flowForm.MapTo<FlowEntity>();
            flowEntity.State = 1;

            flowService.Insert(flowEntity);
            flowService.SaveChanges();

            return AjaxResult.OK("ok");
        }

        [HttpPost]
        public AjaxResult Update([FromBody] FlowForm flowForm)
        {
            flowForm.Validate();
            var entity = flowService.Find(flowForm.Id);
            if (entity == null)
                return AjaxResult.Error("流程不存在");

            flowForm.MapTo(entity);
            flowService.Update(entity);
            flowService.SaveChanges();
            return AjaxResult.OK("ok");
        }

        [HttpPost]
        public AjaxResult GetFlow(string id)
        {
            var entity = flowService.Find(id);
            return AjaxResult.OK(entity, "");
        }
    }
}
