using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using OpenAuth.Generator.Common;
using System.Data;
using static Dapper.SqlMapper;

namespace OpenAuth.Generator.Controllers
{
    public class HomeController: Controller
    {
        private readonly ILogger<HomeController> _logger;

        private AppSetting appSetting = new AppSetting();

        private string _wwwroot;

        private string _ConnectionString;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration, IWebHostEnvironment environment)
        {
            _logger = logger;

            //加载配置文件
            configuration.GetSection("AppSetting").Bind(appSetting);

            //加载连接字符串
            _ConnectionString = configuration.GetConnectionString("OpenAuthDBContext");

            //wwwroot物理路径
            _wwwroot = environment.WebRootPath;
        }

        public IActionResult Index()
        {
            ViewBag.setting = appSetting;
            return View();
        }

        public ActionResult GetTableList()
        {
            var service = new DatabaseService(_ConnectionString);
            var data = service.GetTableList();
            return Content(data.ToJson());
        }

        public ActionResult GetColumnList(string id)
        {
            if (string.IsNullOrEmpty(id))
                id = "0";

            var service = new DatabaseService(_ConnectionString);
            var data = service.GetColumnList(int.Parse(id));
            return Content(data.ToJson());
        }

        public ActionResult Build(string id, string entity, string Area)
        {
            var tableId = int.Parse(id);
            var service = new DatabaseService(_ConnectionString);

            var table = service.GetTable(tableId);
            var columnList = service.GetColumnList(tableId);

            entity = entity.Replace("_", "");

            var appName = appSetting.Application;
            if (Area != null && Area.Length > 0)
            {
                appName = Area;
            }
            BuildService buildService = new BuildService(appName, appSetting.DbContext, entity,
                table, columnList, _wwwroot);

            var entityContent = buildService.BuildEntity();
            var data = new
            {
                entity = entityContent,
            };
            return Content(data.ToJson());
        }
    }

 
}
