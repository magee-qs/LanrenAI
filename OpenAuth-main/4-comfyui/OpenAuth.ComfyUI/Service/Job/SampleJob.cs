using Microsoft.Extensions.Logging; 
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAuth.ComfyUI.Service.Job
{
    public class SampleJob : IJob 
    {
        private ILogger logger  ;
        public SampleJob(ILogger<SampleJob> logger) 
        {
            this.logger = logger;
        }
         
         
        public Task Execute(IJobExecutionContext context)
        {
            logger.LogInformation("测试任务执行:" + DateTime.Now);
            return Task.CompletedTask;
        }
    }
}
