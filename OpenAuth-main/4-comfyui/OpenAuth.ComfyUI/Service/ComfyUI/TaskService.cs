
using Aop.Api.Domain;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using NLog;
using OpenAuth.ComfyUI.Domain;
using OpenAuth.ComfyUI.Domain.ComfyUI;
using OpenAuth.ComfyUI.Model.ComfyUI;
using OpenAuth.ComfyUI.Repository.ComfyUI;
using OpenAuth.ComfyUI.Service.Job;
using OpenAuth.Infrastructure.Signalr;
using System.Threading.Tasks;


namespace OpenAuth.ComfyUI.Service.ComfyUI
{
    public class TaskService : BaseService<TaskEntity>, ITaskService
    {
        protected ITaskFileRepository fileRepository { get; set; }

        protected IComfyUIServer comfyUIServer { get; set; }

        protected IHubContext<MessageHub> hubContext { get; set; }

        protected ICostService costService { get; set; }

        public TaskService(OpenAuthDbContext context, IAuthService authService,
            ITaskFileRepository fileRepository, IComfyUIServer comfyUIServer,
            IHubContext<MessageHub> hubContext, ICostService costService)
            : base(context, authService)
        {
            this.fileRepository = fileRepository;
            this.comfyUIServer = comfyUIServer;
            this.hubContext = hubContext;
            this.costService = costService;
        }

        public void SaveTask(TaskModel taskModel)
        {
            taskModel.Validate();

            var flow = Find<FlowEntity>(taskModel.FlowId);

            //检测当前进行的任务 
            TaskEntity entity = new TaskEntity()
            {
                FlowId = taskModel.FlowId,
                Unit = taskModel.Unit,
                Cost = taskModel.Cost,
                Keyword = taskModel.Keyword,
                Negtive = taskModel.Negtive,
                Width = taskModel.Width,
                Height = taskModel.Height,
                Scale = taskModel.Scale,
            };


            BeginTransaction(() =>
            {
                Insert(entity);

                //扣减费用
                costService.Cost(entity.Id, taskModel.Cost);

                SaveChanges();
            });

            taskModel.Id = entity.Id;
        }


        public async Task Execute(TaskModel taskModel)
        {
            var start = CommonHelper.TimerStart();
            logger.LogInformation("执行comfyui出图任务");
            //获取数据 
            var files = new List<FileDTO>();
            long total = 0;
            try
            { 
                files = await comfyUIServer.BuildImage(taskModel);
                total = CommonHelper.TimerEnd(start);
                logger.LogInformation("执行comfyui出图任务完成,用时:" + total + "毫秒"); 
            }
            catch (Exception ex)
            {
                logger.LogError("出图失败:" + ex.Message, ex);

                costService.ReturnCost(taskModel.Id);
            }


            fileRepository.BeginTransaction(() => 
            {
                //清空数据
                DeleteFile(taskModel.Id);
                var taskEntity = Find(taskModel.Id);
                if (files.Count > 0)
                {
                    foreach (var file in files)
                    {

                        var fileEntity = file.MapTo<TaskFileEntity>();
                        fileEntity.FormId = taskModel.Id;
                        fileEntity.CreateBy = taskEntity.CreateBy;
                        fileEntity.CreateTime = DateTime.Now;
                        //file.FormId = taskEntity.Id;
                        //file.CreateBy = taskEntity.CreateBy;
                        //file.CreateTime = DateTime.Now;

                        //保存信息
                        fileRepository.Insert(fileEntity);
                    }

                    //保存信息到task
                    taskEntity.FileJson = files.ToJson();
                    taskEntity.State = 1;
                    taskEntity.CostTime = (total / 1000).ToInt(0);
                    Update(taskEntity);
                }
                else
                {
                    taskEntity.State = -1;
                    Update(taskEntity);
                }
                SaveChanges(); 
            });

            //发送信息
            await MessageHub.SendTo(hubContext, taskModel.token, "updateTask", taskModel.Id);
        }


        public void DeleteFile(string formId)
        {
            fileRepository.Delete(t => t.FormId == formId);
        }

        public async Task<string> UploadImage(string filePath)
        {
            return await comfyUIServer.UploadImage(filePath);
        }

        public async Task<string> UploadMask(string filePath)
        {
            return await comfyUIServer.UploadMask(filePath);
        }

        public async Task<string> UploadIamge(Stream fileStream, string fileName)
        {
            return await comfyUIServer.UploadImage(fileStream, fileName);
        }

        public async Task<string> UploadMask(Stream fileStream, string fileName)
        {
            return await comfyUIServer.UploadMask(fileStream, fileName);
        }

    }


    public interface ITaskService : IBaseService<TaskEntity>, IScopeDependency
    {
         
        void SaveTask(TaskModel taskModel);

        void DeleteFile(string formId);

        //void StartJob(TaskModel taskModel);

        Task Execute(TaskModel taskModel);

        Task<string> UploadImage(string filePath);

        Task<string> UploadMask(string filePath);

        Task<string> UploadIamge(Stream fileStream, string fileName);

        Task<string> UploadMask(Stream fileStream, string fileName);
    }
}
