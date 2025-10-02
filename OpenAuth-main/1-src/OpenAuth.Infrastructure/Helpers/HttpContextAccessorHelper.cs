using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAuth
{
    public static class HttpContextAccessorHelper
    {
        public static string GetHeader(this IHttpContextAccessor httpContextAccessor, string name)
        {
            return httpContextAccessor?.HttpContext?.Request?.Headers[name];
        }

        public static string GetQuery(this IHttpContextAccessor httpContextAccessor, string key)
        {
            return httpContextAccessor?.HttpContext?.Request?.Query[key];
        }

        public static void SetItems(this IHttpContextAccessor httpContextAccessor, string key, object value)
        {
            if (httpContextAccessor?.HttpContext?.Items != null)
            {
                httpContextAccessor.HttpContext.Items[key] = value;
            }
        }

        public static object GetItems(this IHttpContextAccessor httpContextAccessor, string key)
        {
            if (httpContextAccessor?.HttpContext?.Items != null)
            {
                return httpContextAccessor.HttpContext.Items[key];
            }
            return null;
        }
    }
}
