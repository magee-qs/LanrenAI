using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAuth
{
    public static class WebHelper
    {
         

        /// <summary>
        /// 获取租户
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        /// <returns></returns>
        public static string GetTenantId(this IHttpContextAccessor httpContextAccessor)
        {
            //读取请求头
            string tenantId = httpContextAccessor.GetHeader(CommonConstant.X_Tenant_Id);


            if (tenantId.IsEmpty())
            {
                //读取参数
                tenantId = httpContextAccessor.GetQuery(CommonConstant.X_Tenant_Id);
            }

            return tenantId;
        }

        /// <summary>
        /// 获取token
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        /// <returns></returns>
        public static string GetToken(this IHttpContextAccessor httpContextAccessor)
        {

            //读取请求头
            string token = httpContextAccessor.GetHeader(CommonConstant.X_Access_Token);


            if (token.IsEmpty())
            {
                //读取参数
                token = httpContextAccessor.GetQuery(CommonConstant.X_Access_Token);
            }

            return token;
        }



        /// <summary>
        /// 获取webId
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        /// <returns></returns>
        public static string GetWebId(this IHttpContextAccessor httpContextAccessor)
        {

            //读取请求头
            string webId = httpContextAccessor.GetHeader(CommonConstant.X_WebId);


            if (webId.IsEmpty())
            {
                //读取参数
                webId = httpContextAccessor.GetQuery(CommonConstant.X_WebId);
            }

            return webId;
        }

        /// <summary>
        /// 获取控制器
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        /// <returns></returns>
        public static ControllerActionDescriptor GetControllerActionDescriptor(this IHttpContextAccessor httpContextAccessor)
        {
            return httpContextAccessor?.HttpContext?.GetEndpoint()?.Metadata?.GetMetadata<ControllerActionDescriptor>();
        }

        
    }
}
