using Microsoft.Extensions.DependencyInjection;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAuth
{
    public abstract class AbstractRegistSqlClient : IRegisterSqlClient
    {
        protected ILogger Logger;

        protected IAuthService AuthService;
        public AbstractRegistSqlClient(IAuthService authService)
        {
            Logger = LogManager.GetCurrentClassLogger();
            AuthService = authService;
        }
         
        /// <summary>
        /// 注册IDbContext
        /// 示例代码
        /// logger.Info("dbcontext设置初始化");
        /// services.AddScoped<IDbContextProvider, DbContextProvider>();
        /// services.AddDbContext<OpenFlowDbContext>();
        /// </summary>
        /// <param name="services"></param>
        public abstract void Register(IServiceCollection services);
    }


    public interface IRegisterSqlClient 
    {
        void Register(IServiceCollection services);
    }
}
