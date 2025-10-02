using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using OpenAuth.ComfyUI.Domain.ComfyUI;
using OpenAuth.ComfyUI.Model.ComfyUI;
using OpenAuth.ComfyUI.Service.ComfyUI;
using OpenAuth.Infrastructure.Signalr;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAuth.ComfyUI.Service.Job
{
    public class BuildImageJob : IJob
    {
        protected IComfyUIServer server;
        protected ITaskService taskService;
        protected ILogger logger;
        protected IHubContext<MessageHub> hubContext;
        public BuildImageJob(IComfyUIServer server, ITaskService taskService, 
            ILogger<BuildImageJob> logger, IHubContext<MessageHub> hubContext)
        {
            this.server = server;
            this.taskService = taskService;
            this.logger = logger;
            this.hubContext = hubContext;
        }


        public async Task Execute(IJobExecutionContext context)
        {
            var start = CommonHelper.TimerStart();
            logger.LogInformation("执行comfyui出图任务");
            //获取数据
            var taskModel = (TaskModel)context.MergedJobDataMap.Get("param");
            var files = new List<FileDTO>();
            long total = 0;
            try
            {
                files = await server.BuildImage(taskModel);
                total = CommonHelper.TimerEnd(start);
                logger.LogInformation("执行comfyui出图任务完成,用时:" + total + "毫秒");
            }
            catch (Exception ex)
            {
                logger.LogError("出图失败:" + ex.Message, ex);
            }


            //清空数据
            taskService.DeleteFile(taskModel.Id);
            var taskEntity =  taskService.Find(taskModel.Id);
            if (files.Count > 0)
            {
                foreach (var fileDTO in files)
                {
                    var file = fileDTO.MapTo<TaskFileEntity>();

                    file.FormId = taskEntity.Id;
                    //待处理 
                    file.CreateBy = taskEntity.CreateBy;
                    file.CreateTime = DateTime.Now;

                    //保存信息
                     
                }

                //保存信息到task
                taskEntity.FileJson = files.ToJson();
                taskEntity.State = 1;
                taskEntity.CostTime = (total / 1000).ToInt(0);
                taskService.Update(taskEntity);
            }
            else
            {
                taskEntity.State = -1;
                taskService.Update(taskEntity);
            }
            taskService.SaveChanges(); 
            
        }
    }
}
