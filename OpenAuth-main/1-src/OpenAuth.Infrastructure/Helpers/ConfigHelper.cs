using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAuth
{
    public class ConfigHelper
    {

        private static AppSetting _appSetting = null;

        private static object _locker = new object();

        private static IConfiguration _configuration = null;

        public static IConfiguration GetConfiguration()
        {
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.SetBasePath(System.IO.Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile(
                    $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development"}.json",
                    optional: true)
                .AddEnvironmentVariables();

            var configuration = configurationBuilder.Build();
            return configuration;

        }

        /// <summary>
        /// 读取配置文件
        /// </summary>
        /// <returns></returns>
        private static AppSetting GetAppSetting()
        {
            if (_appSetting == null)
            {
                lock (_locker)
                {
                    if (_appSetting == null)
                    {


                        AppSetting setting = new AppSetting();
                        _configuration.GetSection("AppSetting").Bind(setting);

                        _appSetting = setting;
                    }
                }
            }
            return _appSetting;
        }


        public static AppSetting AppSetting { get { return GetAppSetting(); } }


        public static void Init(IConfiguration configuration, IServiceCollection services)
        {
            _configuration = configuration;


            services.AddSingleton<AppSetting>( x=> GetAppSetting());

            //services.Configure<RedisConfig>(configuration.GetSection("AppSetting:RedisConfig"));
            //services.Configure<AuthConfig>(configuration.GetSection("AppSetting:AuthConfig"));
            //services.Configure<OssConfig>(configuration.GetSection("AppSetting:OssConfig"));
            //services.Configure<DbConnectionConfig>(configuration.GetSection("AppSetting:DbConnection"));
            //services.Configure<DbConnectionConfig>(configuration.GetSection("AppSetting:Upload"));


            //var provider = services.BuildServiceProvider();
            //AppSetting.RedisConfig = provider.GetRequiredService<IOptions<RedisConfig>>().Value;
            //AppSetting.UploadConfig = provider.GetRequiredService<IOptions<UploadConfig>>().Value;
            //AppSetting.OssConfig = provider.GetRequiredService<IOptions<OssConfig>>().Value;
            //AppSetting.DbConnectionConfig = provider.GetRequiredService<IOptions<DbConnectionConfig>>().Value;
            //AppSetting.CorsUrls = configuration.GetSection("AppSetting:CorsUrl").Get<string[]>();



        }

    }

    
}
