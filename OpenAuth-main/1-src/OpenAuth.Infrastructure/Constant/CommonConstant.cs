using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAuth
{
    public class CommonConstant
    {
        /// <summary>
        /// 请求头
        /// </summary>
        public static readonly string X_Access_Token = "X-Access-Token";

        /// <summary>
        /// 租户
        /// </summary>
        public static readonly string X_Tenant_Id = "X-Tenant-Id";

        /// <summary>
        /// sign
        /// </summary>
        public static readonly string X_WebId = "X-WebId";


        /// <summary>
        /// 正常响应
        /// </summary>
        public static readonly int Http_OK = 200;

        /// <summary>
        /// 执行发现错误
        /// </summary>

        public static readonly int Http_Error = 500;


        /// <summary>
        /// 登录过期
        /// </summary>
        public static readonly int Http_NoAuth = 401;

        /// <summary>
        /// 无权限
        /// </summary>
        public static readonly int Http_NoPerm = 403;

        /// <summary>
        /// 资源不存在
        /// </summary>
        public static readonly int Http_NotFound = 404;


        /// <summary>
        /// HttpContext中保存异常
        /// </summary>
        public static readonly string Http_Handler_Error = "__http_handler_error__";


        /// <summary>
        /// ali支付方式
        /// </summary>
        public static readonly string Alipay = "alipay";
        /// <summary>
        /// 微信支付
        /// </summary>
        public static readonly string WXPay = "weixin";
        /// <summary>
        /// 银联支付
        /// </summary>
        public static readonly string UnionPay = "union";
        
    }
}
