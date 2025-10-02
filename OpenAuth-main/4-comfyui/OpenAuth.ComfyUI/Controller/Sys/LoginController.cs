using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenAuth.ComfyUI.Domain.Sys;
using OpenAuth.ComfyUI.Model.Sys;
using OpenAuth.ComfyUI.Model.SYS;
using OpenAuth.ComfyUI.Service.Sys;
using OpenAuth.Infrastructure.Cache;
using static OpenAuth.ComfyUI.Controller.Sys.LoginController;


namespace OpenAuth.ComfyUI.Controller.Sys
{
    [ApiController] 
    public class LoginController : ControllerBase
    {
        private IUserService userService;

        private IAuthService authService;

        private ICacheContext cacheContext;

        private readonly int checkCodeCount = 5;

        private readonly int smsCount = 3;

        private AppSetting appSetting;
        public LoginController(IUserService userService, IAuthService authService, 
            ICacheContext cacheContext, AppSetting appSetting) 
        {
            this.userService = userService;
            this.authService = authService; 
            this.cacheContext = cacheContext;
            this.appSetting = appSetting;
        }


        [HttpPost]
        [Route("/Login")]
        [AllowAnonymous]
        public AjaxResult Login([FromBody] LoginForm loginForm)
        {
            //校验
            loginForm.Validate();

            //校验短信验证码
            var smsKey =  GetSMSKey(); 
            var _checkCode = cacheContext.Get<CheckCode>(smsKey);
            if (_checkCode == null)
                return AjaxResult.Error("验证码无效");

            if (_checkCode.code  != loginForm.VerifyCode)
                return AjaxResult.Error("验证码无效");



            var user = userService.DbContext.Users.IgnoreQueryFilters()
                .Where(t => t.Account == loginForm.Telephone).FirstOrDefault();

            // 检测用户状态
            if (user == null)
            {
                //注册新用户
                user = new  UserEntity()
                {
                    Account = loginForm.Telephone,
                    Phone = loginForm.Telephone,
                    UserLevel = "normal",
                    State = 1,
                    UserName = CommonHelper.GenerateNickName(),
                    Password = InitPwd(loginForm.Telephone).Md5Hash()
                };

                userService.Insert(user);
                userService.SaveChanges();
            }

            if (user.Deleted == 1)
                return AjaxResult.Error("用户无效");

            if (user.State == 0)
                return AjaxResult.Error("用户未启用");

            //获取用户信息
            var userInfo = GetUserInfo(user); 

            //生成token
            var webId = authService.WebId;
            var secretKey = appSetting.AuthConfig.SecretKey;
            var token = authService.CreateToken(user.Id, webId, secretKey);

            //保持到缓存
            authService.Login(userInfo, token);

            //返回 token 和 userInfo
            return AjaxResult.OK(new { token, userInfo }, "");
        }


        [HttpPost]
        [Route("/admin/login")]
        [AllowAnonymous]
        public AjaxResult AdminLogin([FromBody] ALoginForm loginForm)
        {
            loginForm.Validate();

            var user = userService.DbContext.Users.IgnoreQueryFilters()
              .Where(t => t.Account == loginForm.Telephone).FirstOrDefault();

            if (user == null)
                return AjaxResult.Error("用户不存在");

            var pass = loginForm.Password.Md5Hash();
            if (pass != user.Password)
                return AjaxResult.Error("用户名或密码不正确");


            if (user.Deleted == 1)
                return AjaxResult.Error("用户无效");

            if (user.State == 0)
                return AjaxResult.Error("用户未启用");


            //获取用户信息
            var userInfo = GetUserInfo(user);

            //生成token
            var webId = authService.WebId;
            var secretKey = appSetting.AuthConfig.SecretKey;
            var token = authService.CreateToken(user.Id, webId, secretKey);

            //保持到缓存
            authService.Login(userInfo, token);

            //返回 token 和 userInfo
            return AjaxResult.OK(new { token, userInfo }, "");

        }

        /// <summary>
        /// 检测用户有效
        /// </summary>
        /// <param name="userEntity"></param>
        /// <returns></returns>
        private IUserInfo GetUserInfo(UserEntity userEntity)
        {
            UserInfo userInfo = new UserInfo()
            {
                Id = userEntity.Id, 
                Account = userEntity.Account, 
                UserLevel = userEntity.UserLevel, 
                Status = userEntity.State, 
                UserName = userEntity.UserName
            };

            //用户类型
            var query =  from u in userService.DbContext.FeeUsers
            join c in userService.DbContext.FeeLevels on u.FeeId equals c.Id
            where u.UserId == userEntity.Id
            orderby u.Expire ascending
            select new { level = c , u.Expire}; 
            var fee = query.FirstOrDefault();
            if (fee == null)
            {
                userInfo.UserLevel = "normal";
            }
            else
            {
                userInfo.Fee = fee.level.MapTo<FeeModel>();
                userInfo.Fee.Expire = fee.Expire.ToDate();
                userInfo.UserLevel = fee.level.UserLevel;
            }  
            return userInfo;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("/getCaptcha")]
        public AjaxResult GetCaptchaImage()
        {
            if (authService.WebId.IsEmpty())
                return AjaxResult.Error("缺少参数!");

            var captchaKey = GetCaptchaKey();
            var checkCode = cacheContext.Get<CheckCode>(captchaKey);
            if (checkCode == null)
            {
                checkCode = new CheckCode() { count = 1 };
            }

            if (checkCode.count >= checkCodeCount)
            {
                //超出限定时间 等待15分钟
                cacheContext.Set(captchaKey, checkCode, TimeSpan.FromMinutes(15));
                return AjaxResult.Error("超出限定次数,请稍后再试!");
            }

            var captcha = new MathCaptchaHelper();

            var imageBytes = captcha.GenerateImage(120,28);
             

            //保存验证码
            checkCode.count++;
            checkCode.code = captcha.CorrectAnswer.ToString();

            cacheContext.Set(captchaKey, checkCode, TimeSpan.FromSeconds(60));

            //返回结果
            return AjaxResult.OK(imageBytes, ""); 
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("/getSmsCode")]
        public AjaxResult GetSmsCode([FromBody] string telephone)
        {
            if (authService.WebId.IsEmpty())
                return AjaxResult.Error("缺少参数!");

            var smsKey = GetSMSKey();
            var checkCode = cacheContext.Get<CheckCode>(smsKey);
            if (checkCode == null)
            {
                checkCode = new CheckCode() { count = 1 };
            }

            if (checkCode.count >= smsCount)
            {
                //超出限定时间 等待15分钟
                cacheContext.Set(smsKey, checkCode, TimeSpan.FromMinutes(60));
                return AjaxResult.Error("超出限定次数,请稍后再试!");
            }

            var code = CommonHelper.RandomNumber(4);
            checkCode.code = code; 
            //保存验证码
            checkCode.count++;
            AliSMSHelper.SendMessage(code, telephone, "login");
            cacheContext.Set(smsKey, checkCode, TimeSpan.FromSeconds(60));

            //返回结果
            return AjaxResult.OK("");
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("/verifyCheckCode")]
        public AjaxResult VerifyCheckCode([FromBody] string checkCode)
        {
            if (checkCode.IsEmpty())
                return AjaxResult.Error("验证码不能为空");

            var captchaKey = GetCaptchaKey();
            var _checkCode = cacheContext.Get<CheckCode>(captchaKey);
            if (_checkCode == null)
                return AjaxResult.Error("验证码无效");

            if (_checkCode.code == checkCode)
                return AjaxResult.OK("");

            return AjaxResult.Error("验证码无效");

        }

        private string GetCaptchaKey()
        {
             return "captcha:" + authService.WebId;
        }

        private string GetSMSKey()
        {
            return "sms:" + authService.WebId;
        }

        public class CheckCode
        {
            public string code { get; set; }

            public int count { get; set; }
        }

        private string InitPwd(string telephone)
        {
            if (telephone.Length != 11)
                throw new CommonException("电话号码不正确", 500);

            var str = telephone.SubStr(0, 3) + telephone.SubStr(7, 4);
            return str;
        }
        
    }
}
