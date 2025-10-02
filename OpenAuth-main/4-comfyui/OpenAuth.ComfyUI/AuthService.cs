using Microsoft.AspNetCore.Http;
using OpenAuth.Infrastructure.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAuth.ComfyUI
{
    public class AuthService : AbstractAuthService
    {
        public AuthService(ICacheContext cacheContext, IHttpContextAccessor httpContextAccessor) 
            : base(cacheContext, httpContextAccessor)
        {
        }

        public override IUserInfo UserInfo 
        {
            get
            {
                var key = GetCacheKey(Token);

                return _cacheContext.Get<UserInfo>(key);
            } 
        }
    }
}
