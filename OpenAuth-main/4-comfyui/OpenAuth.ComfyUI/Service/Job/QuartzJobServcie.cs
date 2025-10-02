using Microsoft.Extensions.Logging;
using OpenAuth.ComfyUI.Domain;
using OpenAuth.ComfyUI.Domain.Sys;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAuth.ComfyUI.Service.Job
{
    public class QuartzJobServcie : BaseService<JobEntity>, IQuartzJobServcie
    {
        
        private IScheduler scheduler;

        private static string JobKey = "OpenAuthJob";
        public QuartzJobServcie(OpenAuthDbContext context, IAuthService authService, 
            ILogger<QuartzJobServcie> logger , IScheduler scheduler)
            : base(context, authService)
        {
            this.logger = logger;
            this.scheduler = scheduler;
        }

        /// <summary>
        /// 启动所有任务
        /// </summary>
        public void StartAll()
        {
            var list = List(t => t.State == 1);
            foreach (var job in list)
            {
                Start(job);
            } 
        }

        /// <summary>
        /// 启动任务
        /// </summary>
        /// <param name="job"></param>
        public void Start(JobEntity job)
        { 

            var type = Type.GetType(job.ClassName);
            if (type == null)
                throw new QuartzJobException("任务实例不存在");

            var jobBuilder = JobBuilder.Create(type);
            var jobDetail = jobBuilder.WithIdentity(job.Id).Build();
            jobDetail.JobDataMap[JobKey] = job.Id;
            var trigger = TriggerBuilder.Create()
                .WithCronSchedule(job.Cron)
                .WithIdentity(job.Id)
                .StartNow() 
                .Build();


            scheduler.ScheduleJob(jobDetail, trigger);
            logger.LogInformation("启动定时任务:" + job.JobName); 
                
        }

        public void StartSimpleJob(Type job,   string key, object param)
        {
            var jobBuilder = JobBuilder.Create(job);
            var jobDetail = jobBuilder.WithIdentity(key).Build();
            jobDetail.JobDataMap[JobKey] = key;
            jobDetail.JobDataMap.Add("param", param);
            var trigger = TriggerBuilder.Create() 
                .WithIdentity(key)
                .StartNow()
                .Build();
            scheduler.ScheduleJob(jobDetail, trigger);
            logger.LogInformation("启动任务:" + key);
        }


        /// <summary>
        /// 停止定时任务
        /// </summary>
        /// <param name="job"></param>
        /// <param name="scheduler"></param>
        public void Stop(JobEntity job)
        { 
            var triggerKey = new TriggerKey(job.Id);
            // 停止触发器
            scheduler.PauseTrigger(triggerKey);
            // 移除触发器
            scheduler.UnscheduleJob(triggerKey);
            // 删除任务
            scheduler.DeleteJob(new JobKey(job.Id));

            logger.LogInformation("停止定时任务:" + job.JobName);
        }

        /// <summary>
        /// 关闭服务
        /// </summary>
        public void Shutdown()
        {
            scheduler.Shutdown();
            logger.LogInformation("关闭定时任务服务");
        }
      
    }

    public interface IQuartzJobServcie : IBaseService<JobEntity>, IScopeDependency
    {
        /// <summary>
        /// 启动任务
        /// </summary>
        /// <param name="jobEntity"></param>
        void Start(JobEntity jobEntity);


        /// <summary>
        /// 停止定时任务
        /// </summary>
        /// <param name="job"></param>
        /// <param name="scheduler"></param>
        void Stop(JobEntity job);

        /// <summary>
        /// 启动所有任务
        /// </summary>
        void StartAll();

        /// <summary>
        /// 关闭服务
        /// </summary>
        void Shutdown();


        void StartSimpleJob(Type job, string key, object param);
    }
}
