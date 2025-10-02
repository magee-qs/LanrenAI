using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAuth
{
    /// <summary>
    /// 权限服务
    /// </summary>
    public interface IAuthService  
    {
        string TenantId { get; }

        string UserId { get; }

        IUserInfo UserInfo { get;}

        string Token { get; }

        string WebId { get; }

        void Logout();

        void Login(IUserInfo userInfo , string token);

        /// <summary>
        /// 登录校验
        /// </summary>
        void VerifyToken();


        /// <summary>
        /// 生成token
        /// </summary>
        /// <returns></returns>
        string CreateToken(string userId, string appId, string checkCode);
    }
}
