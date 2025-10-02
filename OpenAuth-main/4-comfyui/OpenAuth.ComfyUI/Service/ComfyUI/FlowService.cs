using OpenAuth.ComfyUI.Domain;
using OpenAuth.ComfyUI.Domain.ComfyUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAuth.ComfyUI.Service.ComfyUI
{
    public class FlowService : BaseService<FlowEntity>, IFlowService
    {
        public FlowService(OpenAuthDbContext context, IAuthService authService) 
            : base(context, authService)
        {
        }
    }

    public interface IFlowService : IBaseService<FlowEntity>, IScopeDependency
    {
        
    }
}
