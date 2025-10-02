using Autofac.Extensions.DependencyInjection;
using Autofac;
using Newtonsoft.Json;
using NLog;
using OpenAuth.Filters;
using OpenAuth.Infrastructure.Cache;
using OpenAuth.Middleware;
using Swashbuckle.AspNetCore.SwaggerUI;
using OpenAuth.ComfyUI.Domain;
using NLog.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System.Data;
using Microsoft.Extensions.FileProviders; 
using OpenAuth.ComfyUI.Service.Job;
using Quartz; 
using Autofac.Extras.Quartz;
using System.Text.Json;
using OpenAuth.Infrastructure;
using OpenAuth.Infrastructure.Signalr;
using OpenAuth.ComfyUI.Service.ComfyUI.TaskQueue;
using OpenAuth.ComfyUI.Service.ComfyUI;
using OpenAuth.ComfyUI.Service.ComfyUI.Node;
using Microsoft.AspNetCore.StaticFiles;


namespace OpenAuth.ComfyUI.WebApp
{
    public class BootStrapper
    {
        public IHostEnvironment Environment { get; set; }

        public IConfiguration Configuration { get; set; }

        public NLog.Logger Logger { get; set; }

        public BootStrapper(IConfiguration configuration, IHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;

            Logger = LogManager.GetCurrentClassLogger();
        }


        /// <summary>
        /// 启动函数
        /// </summary>
        /// <param name="args"></param>
        public static void Start(WebApplicationBuilder builder)
        {
            Console.WriteLine($@" 
            Author           :  lr-ai
            Repository       :  https://www.lr-ai.cn
            -------------------------------------------------------------------
            Start Time:{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");

            // 完全清除所有默认日志提供程序
            builder.Logging.ClearProviders();



            // 禁用所有内置日志（包括Kestrel和Hosting日志）
            builder.Logging.AddFilter("Microsoft", Microsoft.Extensions.Logging.LogLevel.Warning);
            builder.Logging.AddFilter("System", Microsoft.Extensions.Logging.LogLevel.Warning);
            builder.Logging.AddFilter("Microsoft.AspNetCore", Microsoft.Extensions.Logging.LogLevel.Warning);

            // 使用NLog作为唯一日志提供程序
            builder.Logging.AddNLog("NLog.config");

            //启动参数
            BootStrapper startUp = new BootStrapper(builder.Configuration, builder.Environment);

            //注册服务
            startUp.ConfigureServices(builder);


            var app = builder.Build();



            startUp.Configure(app);

            //启动任务队列
            TaskQueueService.Init();

            //启动comfyui心跳检测 1分钟检测一次
            NodeHelper.PingComfyUI(1);

            //启动应用
            app.Run(); 
            
        }


        public void ConfigureServices(WebApplicationBuilder builder)
        {
            var services = builder.Services;


            Logger.Info("Services初始化");

            Logger.Info("雪花id初始化");
            IdGenerator.SetIdGenerator();


            if (builder.Environment.IsDevelopment())
            {
                Logger.Info("Swagger初始化");
                services.AddSwaggerGen(option =>
                {
                    // 添加httpHeader参数
                    option.OperationFilter<GlobalHttpHeaderOperationFilter>();
                });
            }

          

            Logger.Info("AppSetting初始化");
            ConfigHelper.Init(Configuration, services);

            //services.AddRouting(options => options.LowercaseUrls = false);
            services.AddMemoryCache();
            services.AddOptions();

            //映射配置文件
            //services.Configure<AppSetting>(Configuration.GetSection("AppSetting"));

            Logger.Info("HttpContextAccessor初始化");
            services.AddHttpClient();
            services.AddHttpContextAccessor();


            //组件注册
            services.AddScoped<ICacheContext, RedisCacheContext>();
            services.AddScoped<IAuthService, AuthService>();

            Logger.Info("cors初始化");
            string[] cors = ConfigHelper.AppSetting.CorsUrlConfig;
            //services.AddCors();
            
            services.AddCors(options => 
            {
                options.AddPolicy("Policy", policy => 
                {
                    policy.WithOrigins(cors).AllowAnyHeader().AllowAnyMethod().AllowCredentials();
                });
            });

            //在startup里面只能通过这种方式获取到appsettings里面的值，不能用IOptions
            Logger.Info("dbcontext设置初始化");
            var connectionString = Configuration.GetConnectionString("DefaultDB");
            Logger.Info($"数据库连接字符串：{connectionString}");
            services.AddDbContext<OpenAuthDbContext>();
            


            services.AddControllers(option =>
            {
                //option.Filters.Add<GlobalExceptionFilter>();
                option.Filters.Add<GlobalAuthenticationFilter>();

            }).ConfigureApiBehaviorOptions(options =>
            {
                // 禁用自动模态验证
                options.SuppressModelStateInvalidFilter = true;
            }).AddNewtonsoftJson(options =>
            {
                // 不改变字段大小
                options.SerializerSettings.ContractResolver = new LowerCaseContractResolver();
                // 设置格式化输出
                options.SerializerSettings.Formatting = Formatting.Indented;
                // 防止循环引用
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                // 默认日志格式化
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";

            });

            //注入SignalR实时通讯，默认用json传输
            builder.Services.AddSignalR()
            .AddJsonProtocol(options =>
            {
                options.PayloadSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            });

            //// 添加问题详情和异常处理
            //services.AddExceptionHandler<ResponseExceptionHandler>();
            //services.AddProblemDetails();

            //添加Autofac支持
            builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

            Logger.Info("ConfigureContainer初始化");

            // 注册模块
            builder.Host.ConfigureContainer<ContainerBuilder>(AutoRegister);


            // 设置定时启动的任务
            services.AddHostedService<QuartzService>(); 
        }





        public void Configure(WebApplication app)
        {
            //公共参数
            InternalApp.ServiceProvider = app.Services;
            InternalApp.Configuration = Configuration;
            InternalApp.WebHostEnvironment = app.Environment;


            if (app.Environment.IsDevelopment())
            {
                // swagger中间件
                app.UseSwagger();
                app.UseSwaggerUI(ui =>
                {
                    ui.DocExpansion(DocExpansion.List); //默认展开列表
                    ui.OAuthClientId("OpenAuth.WebApi"); //oauth客户端名称
                    ui.OAuthAppName("开源版webapi认证"); // 描述
                });
            }

          


            //使用全局异常中间件
            //app.UseMiddleware<GlobalExceptionMiddleware>();

            //请求头转发
            //ForwardedHeaders中间件会自动把反向代理服务器转发过来的X-Forwarded-For（客户端真实IP）
            //以及X-Forwarded-Proto（客户端请求的协议）自动填充到HttpContext.Connection.RemoteIPAddress
            //和HttpContext.Request.Scheme中，这样应用代码中读取到的就是真实的IP和真实的协议了 
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedFor | Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedProto
            });

 
             
            //默认配置文件 wwwroot
            app.UseDefaultFiles();

            //自定义目录
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(FileHelper.GetBasePath()),
                RequestPath = "/files",
                ContentTypeProvider = new FileExtensionContentTypeProvider(),
                OnPrepareResponse = ctx =>
                {

                    //可以在这里为静态文件添加其他http头信息，默认添加跨域信息
                    ctx.Context.Response.Headers["Access-Control-Allow-Origin"] = "*";

                }
            });

            // 路由中间件
            //app.UseRouting();

            //跨域配置中间件   
            app.UseCors("Policy");

            // 认证中间件

            // 授权中间件

            // 自定义业务逻辑中间件

            //日志中间件 
            app.UseMiddleware<RequestResponseLoggingMiddleware>();

            //终结点路由
            app.MapControllers();

            //设置socket连接
            app.MapHub<MessageHub>("/hub");

            //配置ServiceProvider
            AutofacServiceProvider.ConfigureServiceProvider(app.Configuration, app.Services);

           
        }


        //自动注入
        public void AutoRegister(ContainerBuilder builder)
        {

            //builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly());

            //程序集依次注入 
            var assemblyList = ReflectHelper.AssemblyList;
            //单例注册
            builder.RegisterAssemblyTypes(assemblyList.ToArray())
                .Where(t => typeof(ISingletonDependency).IsAssignableFrom(t) && !t.IsAbstract)
                .AsImplementedInterfaces()
                .SingleInstance();

            // 实例注册 每个请求创建一个实例
            builder.RegisterAssemblyTypes(assemblyList.ToArray())
                .Where(t => typeof(IScopeDependency).IsAssignableFrom(t) && !t.IsAbstract)
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();


            // 注册quartz
            builder.RegisterModule(new QuartzAutofacFactoryModule());


            // 注册job
            builder.RegisterModule(new QuartzAutofacJobsModule(typeof(SampleJob).Assembly));
        }
    }
}
