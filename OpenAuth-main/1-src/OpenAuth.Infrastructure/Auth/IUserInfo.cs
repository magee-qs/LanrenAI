using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAuth
{
    /// <summary>
    /// 登录用户
    /// </summary>
    public interface IUserInfo
    {
        public string Id { get; set; }

        public string Account { get; set; }
        public string UserName { get; set; }  

        public string TenantId { get; set; }

        /// <summary>
        /// 用户状态 0 停用 1 启用
        /// </summary>
        public int Status { get; set; }

        public string UserLevel { get; set; }

        /// <summary>
        /// 用户权限
        /// </summary>
        public List<string> Perms { get; set; }
         

    }
}
