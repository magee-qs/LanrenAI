 
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR; 
using Newtonsoft.Json.Linq;
using NLog;
using OpenAuth.ComfyUI.Domain.ComfyUI;
using OpenAuth.ComfyUI.Model.ComfyUI; 
using OpenAuth.ComfyUI.Service.ComfyUI;
using OpenAuth.ComfyUI.Service.ComfyUI.Node;
using OpenAuth.ComfyUI.Service.ComfyUI.TaskQueue;
using OpenAuth.Infrastructure.Cache;
using OpenAuth.Infrastructure.Helpers;
using OpenAuth.Infrastructure.Signalr; 
 

namespace OpenAuth.ComfyUI.Controller.ComfyUI
{
    [ApiController]
    [Route("/[controller]/[action]")]
    public class TaskController : ControllerBase  
    {
        protected ITaskService taskService;
        protected IAuthService authService;
      
        protected AppSetting appSetting;
        protected IFlowService flowService;
        protected ICacheContext cacheContext;
       
        protected ILogger logger = LogManager.GetCurrentClassLogger();

        protected RedisPriorityTaskQueue _queue = RedisPriorityTaskQueue.Instance;

        protected IHubContext<MessageHub> hubContext;
        public TaskController(ITaskService taskService, IAuthService authService , IFlowService flowService, 
            ICacheContext cacheContext, IWebHostEnvironment env, IHubContext<MessageHub> hubContext,
            AppSetting appSetting) 
        {
            this.taskService = taskService;
            this.authService = authService;
          
            this.flowService = flowService;
            this.cacheContext = cacheContext;
            this.hubContext = hubContext;
            this.appSetting = appSetting;
        }


        [HttpPost]
        public AjaxResult List([FromBody]Page page)
        {
            var userId = authService.UserId;
            var list = taskService.List(t => t.CreateBy == userId, page);

            return AjaxResult.OK(new { rows = list ,   page.total }, "ok");
        }

        [HttpPost]
        public AjaxResult GetFileList([FromBody] Page page) 
        {
            var userId = authService.UserId;
            var list = taskService.List<TaskFileEntity>(t => t.CreateBy == userId, page);
            return AjaxResult.OK(new { rows = list, page.total }, "ok");
        }

        [HttpPost]
        public AjaxResult GetTask([FromBody] string taskId)
        {
            var task = taskService.Find(taskId);
            return AjaxResult.OK(task, "");
        }

        [HttpPost]
        public AjaxResult GetHistory([FromBody] string flowId)
        {
            var userId = authService.UserId;
            var startDate = DateTime.Now.ToMin();
            var endDate = DateTime.Now.ToMax();
            var list = taskService.Queryable<TaskEntity>(t =>t.State ==  1 && t.FlowId == flowId 
              && t.CreateBy == userId
              //&& t.CreateTime >= startDate && t.CreateTime <= endDate
              )
                .OrderByDescending(t => t.CreateTime).Take(10);

            return AjaxResult.OK(list, "");

        }
         
        /// <summary>
        /// 提交任务
        /// </summary>
        /// <param name="jsonTemplate"></param>
        /// <returns></returns>
        [HttpPost] 
        public async Task<AjaxResult> SaveTask(JObject json)
        { 

            var service = NodeServiceProvider.Instance(json);

            var result = await service.SaveTask(json);

            return result;
        }


        [HttpPost]
        public AjaxResult RandSeed()
        {
            var seed = CommonHelper.RandSeed();
            return AjaxResult.OK(seed, "");
        }
          
        [HttpPost]
        public async Task<AjaxResult> TestTransform([FromBody] string content)
        {
            var str1 = await BaiduTransformHelper.ToEnglish(content);
            var str2 = await BaiduTransformHelper.ToChinese(str1);
            return AjaxResult.OK(new {str1, str2}, "");
        }

        ///// <summary>
        ///// 上传图片
        ///// </summary>
        ///// <param name="file"></param>
        ///// <returns></returns>
        //[HttpPost]
        //public async Task<AjaxResult> UploadImage(IFormFile file)
        //{
        //    var uploadPath = UploadPath();

        //    var fileDTO = await Upload(file, uploadPath);

        //    //文件真实地址
        //    var filePath = Path.Combine(FileHelper.GetBasePath(), fileDTO.FilePath);

        //    // 上传到comfyui
        //    var str = await taskService.UploadImage(filePath);

        //    var content = str.ToObject<FileUploadContent>();

        //    var data = new { content.Name, content.Type, content.Subfolder, fileDTO.FileName, fileDTO.FilePath };


        //    return AjaxResult.OK(data, "");
        //}

        //[HttpPost]
        //public async Task<AjaxResult> UploadMask()
        //{
        //    if(Request.Form.Files.Count == 0)
        //        return AjaxResult.Error("没有上传文件");

        //    var file = Request.Form.Files[0];

        //    var uploadPath = UploadPath();
        //    var fileDTO = await Upload(file, uploadPath);

        //    //文件地址
        //    var filePath =  Path.Combine(FileHelper.GetBasePath(), fileDTO.FilePath) ;


        //    // 上传到comfyui
        //    var str = await taskService.UploadMask(filePath);

        //    var content = str.ToObject<FileUploadContent>();

        //    var data = new { content.Name, content.Type, content.Subfolder, fileDTO.FileName, fileDTO.FilePath };


        //    return AjaxResult.OK(data, "");
        //}

        [HttpPost]
        public async Task<AjaxResult> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new CommonException("没有待上传文件", 500);


            if (FileHelper.IsSafeFile(file.FileName) == false)
                throw new CommonException("文件类型不正确", 500);

            var mimeType = FileHelper.CheckFileType(file.FileName);

            //随机文件名
            var fileName = FileHelper.GetRandomFileName(file.FileName, 64);

            long fileSize = file.Length;

            var filePath = FileHelper.Combine("temp", DateTime.Now.ToString("yyyyMMdd"), fileName);

            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);

                memoryStream.Position = 0;
                //保存临时文件
                AliOssHelper.Upload(filePath, memoryStream); 

                memoryStream.Position = 0;
                //上传到服务器
                string str =  await taskService.UploadIamge(memoryStream, fileName);
                var content = str.ToObject<FileUploadContent>();
                var data = new { content.Name, content.Type, content.Subfolder, fileName, filePath };

                return AjaxResult.OK(data, "");

            } 

        }

        //private string UploadPath()
        //{
        //    return Path.Combine("temp", DateTime.Now.ToString("yyyyMMdd"));
        //}

        //private async Task<FileDTO> Upload(IFormFile file, string uploadPath)
        //{
        //    if (file == null || file.Length == 0)
        //        throw new CommonException("没有待上传文件", 500);


        //    //随机文件名
        //    var fileName = FileHelper.GetRandomFileName(file.FileName, 64);
        //    //路径
        //    var filePath = Path.Combine(uploadPath, fileName);

        //    //物理目录
        //    var savePath = Path.Combine(FileHelper.GetBasePath(), uploadPath);

        //    if (!Directory.Exists(savePath))
        //    {
        //        Directory.CreateDirectory(savePath);
        //    }

        //    //物理路径
        //    savePath = Path.Combine(savePath, fileName);

        //    long fileSize = 0;

        //    using (var fileStream = new FileStream(savePath, FileMode.Create))
        //    {
        //        fileSize = fileStream.Length;
        //        await file.CopyToAsync(fileStream);
        //    }

        //    FileDTO fileDTO = new FileDTO()
        //    {
        //        Id = IdGenerator.NextId().ToString(),
        //        FileName = file.FileName,
        //        FileSize = fileSize,
        //        FilePath = filePath,
        //        FileType = MediaTypeHelper.GetMimeType(fileName)
        //    };

        //    return fileDTO;
        //}

        [HttpPost]
        public async Task<AjaxResult> GetServerState()
        {
            try
            {
                HttpClient client = new HttpClient();
                var config = appSetting.ComfyUIConfig;
                string url = config.url + "/api/history/5a00d295-7741-4c3e-9b88-cc4f1054ccd";
                logger.Debug("comfyui心跳检测");
                var response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    //正常
                    logger.Debug("comfyui心跳检测正常");

                    return AjaxResult.OK("ok","");
                }
                else
                {
                    //异常 发送短信通知
                    logger.Error("comfyui服务异常:" + response.ReasonPhrase);
                    return AjaxResult.OK("","");
                }

            }
            catch (Exception ex)
            {
                logger.Error( ex, "心跳检测抛出异常:" + ex.Message);
                return AjaxResult.OK("fail", "");
            }
        }
    } 
}
