using OpenAuth.ComfyUI.Domain;
using OpenAuth.ComfyUI.Domain.Sys;
using OpenAuth.ComfyUI.Model.SYS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAuth.ComfyUI.Service.Sys
{
    public class UserService : BaseService<UserEntity>, IUserService
    {
        public UserService(OpenAuthDbContext context, IAuthService authService) 
            : base(context, authService)
        {
        }
    }


    public interface IUserService : IBaseService<UserEntity>, IScopeDependency
    {
         
    }
}
