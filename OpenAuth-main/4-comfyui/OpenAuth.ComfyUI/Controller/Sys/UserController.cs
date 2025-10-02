using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenAuth.ComfyUI.Domain;
using OpenAuth.ComfyUI.Service.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAuth.ComfyUI.Controller.Sys
{
    [ApiController]
    [Route("/[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private IUserService  userService;
        public UserController(IUserService  userService) 
        {
            this.userService = userService;
        }

        
        [HttpPost]  
        public AjaxResult List()
        {
            var list = userService.Queryable().FirstOrDefault();
            return AjaxResult.OK(list,"");
        }
        [HttpPost]
        public AjaxResult GetUserByTelephone([FromBody] string telephone)
        {
            var user = userService.Find(t => t.Account == telephone);

            if (user == null)
                return AjaxResult.OK(null, "");
            var data = new { user.Id, user.Account };
            return AjaxResult.OK(data, "");
        }
    }
}
