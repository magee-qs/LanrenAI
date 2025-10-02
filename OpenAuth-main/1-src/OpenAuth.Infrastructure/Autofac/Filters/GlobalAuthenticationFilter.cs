using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OpenAuth.Filters
{
    public class GlobalAuthenticationFilter : IAuthorizationFilter
    {
        private readonly IAuthService _authService;
        private readonly ILogger _logger;


        public GlobalAuthenticationFilter(IAuthService authService, ILogger<GlobalAuthenticationFilter> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            //_logger.LogInformation("--------------token校验----------------");
            _logger.LogInformation("token校验:" + _authService.Token);
            //token认证
            DoAuthentication(context);

            //_logger.LogInformation("--------------权限校验----------------");
            //操作权限认证
            DoAuthorization(context);
        }

        private void DoAuthentication(AuthorizationFilterContext context)
        {
            var description =
             (Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)context.ActionDescriptor;

            var Controllername = description.ControllerName.ToLower();
            var Actionname = description.ActionName.ToLower();

            //匿名标识
            var authorize = description.MethodInfo.GetCustomAttribute(typeof(AllowAnonymousAttribute));
            if (authorize != null)
            {
                return;
            }

            //校验token 
            _authService.VerifyToken();

            // 用户缓存失效(考虑重登录)
            var userInfo = _authService.UserInfo;
            if (userInfo == null)
            {
                throw new AuthenticationException("token无效", 401);
            }
        }

        public void DoAuthorization(AuthorizationFilterContext context)
        {
            var description =
            (Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)context.ActionDescriptor;

            var Controllername = description.ControllerName.ToLower();
            var Actionname = description.ActionName.ToLower();

            //匿名标识
            var authorize = description.MethodInfo.GetCustomAttribute(typeof(PermCodeAttribute));
            if (authorize == null)
            {
                return;
            }

            var userInfo = _authService.UserInfo;
            var permCode = (authorize as PermCodeAttribute).Code;

            if (userInfo == null || userInfo.Perms == null || userInfo.Perms.Count == 0)
                throw new AuthenticationException("无权限", 403);

            if (!userInfo.Perms.Contains(permCode))
            {
                throw new PermissionException("无权限", 403);
            }


        }
    }
}
