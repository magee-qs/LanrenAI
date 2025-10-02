using OpenAuth.ComfyUI.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAuth.ComfyUI.Service.ComfyUI
{
    public class FeeLevelService : BaseService<FeeLevelEntity>, IFeeLevelService
    {
        public FeeLevelService(OpenAuthDbContext context, IAuthService authService) 
            : base(context, authService)
        {
        }
    }


    public interface IFeeLevelService : IBaseService<FeeLevelEntity>, IScopeDependency
    { 
    }
}
