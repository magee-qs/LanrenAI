using Aop.Api.Domain;
using Microsoft.AspNetCore.Hosting; 
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using Newtonsoft.Json.Linq;
using OpenAuth.ComfyUI.Domain.ComfyUI;
using OpenAuth.ComfyUI.Model.ComfyUI;
using OpenAuth.ComfyUI.Service.ComfyUI.Node; 
using System.Text;
using System.Threading.Tasks;


namespace OpenAuth.ComfyUI.Service.ComfyUI
{
    /// <summary>
    /// comfyui请求服务
    /// </summary>
    public class ComfyUIServer : IComfyUIServer
    {
        private ComfyUIConfig config;
        private ILogger<ComfyUIServer> logger;
        private HttpClient httpClient;
        private IWebHostEnvironment env;
        private IAuthService authService;


        

        public ComfyUIServer(AppSetting appSetting, ILogger<ComfyUIServer> logger, IWebHostEnvironment env, IAuthService authService)
        {
            config = appSetting.ComfyUIConfig;
            this.logger = logger;
            this.env = env;
            this.authService = authService;
            httpClient = new HttpClient(); 
        }
         
        /// <summary>
        /// 生成图像
        /// </summary>
        /// <param name="workflowJson"></param>
        /// <param name="outputDir"></param>
        /// <returns></returns>
        public async Task<List<FileDTO>> BuildImage(TaskModel taskModel)
        {
            logger.LogInformation($"开始出图,任务id:{taskModel.Id}");
            logger.LogInformation($"任务参数:{taskModel.FlowJson}");

            var workflowJson = new { prompt = taskModel.FlowJson.ToJObject(), client_id = authService.WebId };

            //获取promptId
            var promptResponse = await QueryPrompt(workflowJson.ToJson());

            if (promptResponse["prompt_id"] == null)
            {
                logger.LogError("comfyui服务请求失败:" + promptResponse.ToString());
                throw new BizException("comfyui服务请求失败");
            }
                
            var promptId = promptResponse["prompt_id"].ToString();

            //轮询获取结果  图片数量 * 单次等待数量
            taskModel.NodeConfig.Runtime = taskModel.NodeConfig.Runtime * taskModel.Unit;
            var result = await PollForResult(promptId, taskModel.NodeConfig);

            if (result == null || result.HasValues == false)
            {
                logger.LogError($"出图超时,任务id:{taskModel.Id}执行失败");
                throw new CommonException("任务超时", 500);
            }

            //获取图片地址
            return await SaveImage(result, config.path);  
        }

  
        private async Task<JObject> QueryPrompt(string workflow)
        { 
            HttpContent httpContent = new StringContent(workflow, Encoding.UTF8, "application/json");
            logger.LogInformation("发起出图申请:" + DateTime.Now ); 
            var result = await httpClient.PostAsync(config.url + "/api/prompt", httpContent);
                
            var content = await result.Content.ReadAsStringAsync();
            var jsonObject = content.ToJObject(); 
            return jsonObject;   
        }

         

        /// <summary>
        /// 轮询获取生成图片信息
        /// </summary>
        /// <param name="prompId"></param>
        /// <returns></returns>
        private async Task<JObject> PollForResult(string prompId, NodeConfig nodeConfig)
        {
            var now = DateTime.Now;
            var expire = now.AddMinutes(nodeConfig.Runtime);
          
            var delayTime = nodeConfig.WaitTime * 1000;
            int count = 1; 
            while (DateTime.Now < expire)
            {

                logger.LogInformation("轮询图片:" + prompId +", 时间: "+ DateTime.Now + ", 次数:" + count);
                var response = await httpClient.GetAsync(config.url + "/api/history/" + prompId);
                if (response.IsSuccessStatusCode)
                { 
                    var content = await response.Content.ReadAsStringAsync();

                    logger.LogDebug($"轮询结果:{content}"); 
                    var history = content.ToJObject();
                    // 无结果: {} ,
                    // 有结果:   { 'prompt':'',status:{status_str: 'success' }, outputs:{3: { images:[...]}}
                    if (history[prompId] != null  && history[prompId].HasValues == true)
                    {
                        return  history[prompId] as JObject;
                    } 
                } 
                count++;
                //等待1秒
                await Task.Delay(delayTime);
            }
            return null;
        }

        /// <summary>
        /// 保存图片
        /// </summary>
        /// <param name="result"></param>
        /// <param name="dir"></param>
        /// <returns></returns>
        private async Task<List<FileDTO>> SaveImage(JObject result, string dir)
        {
            var outputs = result["outputs"] as JObject;
            List<FileDTO> fileList = new List<FileDTO>();
             
            if (outputs == null)
            {
                logger.LogError("没有找到图片节点");
                return fileList;
            }

            logger.LogDebug("返回图像:" + outputs.ToString());
             

            logger.LogDebug("开始下载图片");
            string date = DateTime.Now.ToString("yyyyMMdd");
           
            foreach (var output in outputs)
            {
                var node = output.Value as JObject;
                if (node["images"] != null && node["images"].HasValues == true)
                {
                    // { images: [{ filename: 'xxx',subfolder:'',type: 'output'; }]}
                    var images = node["images"] as JArray;
                    foreach (var image in images)
                    {
                        logger.LogDebug($"图片输出节点: {image.ToString()}"); 
                      
                        var filename = image["filename"].ToString();
                        var type = image["type"].ToString();
                        var subfolder = image["subfolder"].ToString();

                        var url = "/api/view?filename=" + filename + "&subfolder=" + subfolder
                             + "&type=" + type + "&rand=" + DateTime.Now.ToUnixTime();
                        var response = await httpClient.GetAsync(config.url + url);
                        if (response.IsSuccessStatusCode)
                        {
                             
                            var _fileName = FileHelper.GetRandomFileName(filename); 
                            //保存文件 到oss存储
                            var file = await FileHelper.SaveFile(response.Content,dir, _fileName, 205, 360);

                            //保存到本地
                            #region 保存到本地
                            //logger.LogDebug("图片名称:" + filename);
                            //var uploadPath = Path.Combine(FileHelper.GetBasePath(), dir, date);
                            //logger.LogDebug("路径:" + uploadPath);
                            ////判断目录是否存在
                            //if (!Directory.Exists(uploadPath))
                            //{
                            //    Directory.CreateDirectory(uploadPath);
                            //}

                            ////保存地址
                            //string savePath = Path.Combine(uploadPath, _fileName);
                            //logger.LogDebug("物理地址:" + uploadPath);
                            ////相对地址 返回数据
                            //string filePath = Path.Combine(dir, date, _fileName);
                            //logger.LogDebug("相对路径:" + filePath);

                            //var buffer = await response.Content.ReadAsByteArrayAsync();
                            //await File.WriteAllBytesAsync(savePath, buffer);


                            ////缩微图 
                            //var fileName =  FileHelper.GetThumbnailName(savePath, out var thumbPath);
                            //logger.LogDebug("开始生成缩微图:" + fileName);
                            //await  FileHelper.CreateThumbnailImage(savePath, fileName, 205, 360);

                            //FileDTO file = new FileDTO()
                            //{
                            //    FileName = _fileName,
                            //    FilePath = filePath,
                            //    FileType = MediaTypeHelper.GetMimeTypeByName(filename),
                            //    FileSize = buffer.Length
                            //};
                            #endregion
                            fileList.Add(file); 
                        }
                        
                    }
                }
            }

            return fileList;
        }


        public async Task<string> UploadImage(Stream fileStream, string fileName)
        {
            // 创建MultipartFormDataContent
            using var formData = new MultipartFormDataContent(); 
            using var fileContent = new System.Net.Http.StreamContent(fileStream);

            // 设置Content-Type头
            string contentType = MediaTypeHelper.GetMimeTypeByName(fileName);
            fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(contentType);

            // 添加到表单数据
            formData.Add(fileContent, "image", Path.GetFileName(fileName));

            // 发送POST请求  /api/upload/image
            HttpResponseMessage response = await httpClient.PostAsync(config.url + config.uploadImage, formData);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                logger.LogDebug("上传图片到comfyui返回结果:" + result);
                return result;
            }
            else
            {
                logger.LogError($"上传失败:{response.ReasonPhrase},code: {response.StatusCode}, content:{response.Content}");
                throw new CommonException($"上传失败:{response.ReasonPhrase},code: {response.StatusCode}", 500);
            }
        }

         
        public async Task<string> UploadImage(string filePath)
        {
            // 创建MultipartFormDataContent
            using var formData = new MultipartFormDataContent();
             
            // 读取文件内容
            await using var fileStream = File.OpenRead(filePath);
            using var fileContent = new System.Net.Http.StreamContent(fileStream);

            // 设置Content-Type头
            string contentType = MediaTypeHelper.GetMimeTypeByName(filePath);
            fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(contentType);

            // 添加到表单数据
            formData.Add(fileContent, "image", Path.GetFileName(filePath));

         
            // 发送POST请求  /api/upload/image
            HttpResponseMessage response = await httpClient.PostAsync(config.url +   config.uploadImage, formData);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync(); 
                logger.LogDebug("上传图片到comfyui返回结果:" + result);
                return result;
            }
            else
            {
                logger.LogError($"上传失败:{response.ReasonPhrase},code: {response.StatusCode}, content:{response.Content}" );
                throw new CommonException($"上传失败:{response.ReasonPhrase},code: { response.StatusCode}"  ,500);
            } 
        }

        public async Task<string> UploadMask(string filePath)
        {
            // 创建MultipartFormDataContent
            using var formData = new MultipartFormDataContent();
             
            // 读取文件内容
            await using var fileStream = File.OpenRead(filePath);
            using var fileContent = new System.Net.Http.StreamContent(fileStream);

            // 设置Content-Type头
            string contentType = MediaTypeHelper.GetMimeTypeByName(filePath);
            fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(contentType);

            // 添加到表单数据
            formData.Add(fileContent, "image", Path.GetFileName(filePath));


            // 发送POST请求  /api/upload/mask
            HttpResponseMessage response = await httpClient.PostAsync(config.url + config.uploadMask, formData);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                logger.LogDebug("上传图片到comfyui返回结果:" + result);
                return result;
            }
            else
            {
                logger.LogError($"上传失败:{response.ReasonPhrase},code: {response.StatusCode}, content:{response.Content}");
                throw new CommonException($"上传失败:{response.ReasonPhrase},code: {response.StatusCode}", 500);
            }
        }

        public async Task<string> UploadMask(Stream fileStream , string fileName)
        {
            // 创建MultipartFormDataContent
            using var formData = new MultipartFormDataContent();

            // 读取文件内容 
            using var fileContent = new System.Net.Http.StreamContent(fileStream);

            // 设置Content-Type头
            string contentType = MediaTypeHelper.GetMimeTypeByName(fileName);
            fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(contentType);

            // 添加到表单数据
            formData.Add(fileContent, "image", Path.GetFileName(fileName));


            // 发送POST请求  /api/upload/mask
            HttpResponseMessage response = await httpClient.PostAsync(config.url + config.uploadMask, formData);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                logger.LogDebug("上传图片到comfyui返回结果:" + result);
                return result;
            }
            else
            {
                logger.LogError($"上传失败:{response.ReasonPhrase},code: {response.StatusCode}, content:{response.Content}");
                throw new CommonException($"上传失败:{response.ReasonPhrase},code: {response.StatusCode}", 500);
            }
        }

        public Task<string> GetPrompt(string filePath)
        {
            return new Task<string>(() => { return "sss"; });
        }
    }

    /// <summary>
    /// comfyui请求服务
    /// </summary>
    public interface IComfyUIServer : IScopeDependency
    { 
         

        Task<List<FileDTO>> BuildImage(TaskModel taskModel);


        /// <summary>
        /// 返回数据{ "name":"clipspace-mask-2465914.5.png","subfolder":"","type":"input"}
        /// 提交数据: { "image":"clipspace-mask-2465914.5.png"}
        Task<string> UploadImage(string filePath);

        Task<string> UploadImage(Stream fileStream, string fileName);


        /// <summary>
        /// 返回数据{ "name":"clipspace-mask-2465914.5.png","subfolder":"","type":"input"}
        /// 提交数据: { "image":"clipspace-mask-2465914.5.png"}
        Task<string> UploadMask(string filePath);

        Task<string> UploadMask(Stream fileStream, string fileName);

        Task<string> GetPrompt(string filePath);
    }
}
