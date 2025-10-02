using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAuth.ComfyUI.Service.ComfyUI.TaskQueue
{
    public class TaskQueueService
    {
        public static TaskWorker TaskWorker { get { return _taskWork; } }

        private static TaskWorker _taskWork;

        private static NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        public static void Init()
        {
            _logger.Info("任务队列初始化");
            // 启动任务队列
            RedisPriorityTaskQueue _queue = RedisPriorityTaskQueue.Instance;
            ITaskService taskService =  AutofacServiceProvider.GetService<ITaskService>(); 
            _taskWork = new TaskWorker(_queue, taskService);

            _taskWork.Start();
        }
    }
}
