using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAuth
{
    public class CacheConstent
    {
        /// <summary>
        /// 登录用户
        /// </summary>
        public static readonly string Prefix_UserInfo = "userInfo:";

        /// <summary>
        /// 用户权限
        /// </summary>
        public static readonly string Prefix_User_Authorization = "user_author:";

        /// <summary>
        /// 系统租户
        /// </summary>
        public static readonly string User_Tenant = "tenant";

        /// <summary>
        /// 数据字典
        /// </summary>
        public static readonly string User_Dict = "dict";
    }
}
