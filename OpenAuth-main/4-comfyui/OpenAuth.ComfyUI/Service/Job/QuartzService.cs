using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAuth.ComfyUI.Service.Job
{
    public class QuartzService : IHostedService, IDisposable
    {
        private readonly ILogger<QuartzService> _logger;
        private IScheduler _scheduler;
        public QuartzService(ILogger<QuartzService> logger, IScheduler scheduler) 
        {
            _logger = logger;
            _scheduler = scheduler;
        }  
        public void Dispose()
        {
             
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("开启定时任务服务");
            return _scheduler.Start(); 
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("关闭定时任务服务");
            return _scheduler.Shutdown();
        }
    }
}
