using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using OpenAuth.Infrastructure.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAuth
{ 
    public abstract class AbstractAuthService :IAuthService
    {
        protected ICacheContext _cacheContext;

        protected IHttpContextAccessor _httpContextAccessor;
        
        public AppSetting AppSetting { get; set; }

       
        public AbstractAuthService(ICacheContext cacheContext, IHttpContextAccessor httpContextAccessor)
        {
            _cacheContext = cacheContext;
            _httpContextAccessor = httpContextAccessor;

            AppSetting = ConfigHelper.AppSetting;
        }


        public string UserId { get {  return UserInfo == null ? null : UserInfo.Id; } }

        public string TenantId { get { return UserInfo == null ? null : UserInfo.TenantId; } }

        protected string GetCacheKey(string token)
        {
            return CacheConstent.Prefix_UserInfo + token.ToStr(string.Empty);
        }

        public abstract IUserInfo UserInfo { get; }
        
        
        public string Token 
        {
            get 
            {
                return _httpContextAccessor.GetToken();
            } 
        }

        public string WebId
        {
            get
            {
                return _httpContextAccessor.GetWebId();
            }
        }
       

        public void Logout()
        {
            var key = GetCacheKey(Token);
            _cacheContext.Remove(key);
        }

        public void Login(IUserInfo userInfo, string token)
        {
            var key = GetCacheKey(token);
            var expire = AppSetting.AuthConfig.ExpireTime.ToInt(1440);
            _cacheContext.Set(key, userInfo, TimeSpan.FromMinutes(expire));
        }


        public string CreateToken(string userId, string appId, string checkCode)
        {  
            string token = IdGenerator.NextId() + "-"  + CommonHelper.Random(32, true);
            return token;
        }

        public void VerifyToken()
        {
            var token = Token;
            if (token.IsEmpty())
            {
                throw new AuthenticationException("token无效", 401);
            }

            var arr = token.Split("-");
            if (arr.Length != 2)
                throw new AuthenticationException("token无效", 401);

            if (arr[0].Length != 15 || arr[1].Length != 32)
                throw new AuthenticationException("token无效", 401);

            if (arr[0].ToLong(0) == 0)
                throw new AuthenticationException("token无效", 401);

           

            var _userInfo = UserInfo;
            if (_userInfo == null)
                throw new AuthenticationException("token已失效,请重新登录", 401); 
        }
    }
}
