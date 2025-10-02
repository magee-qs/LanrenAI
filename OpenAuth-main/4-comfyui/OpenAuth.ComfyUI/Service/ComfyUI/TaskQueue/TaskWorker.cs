using NLog;
using OpenAuth.ComfyUI.Model.ComfyUI; 
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAuth.ComfyUI.Service.ComfyUI.TaskQueue
{
    public class TaskWorker
    {
        private readonly RedisPriorityTaskQueue _queue;

        private readonly CancellationTokenSource _cts;

        private ILogger logger = LogManager.GetCurrentClassLogger();
         
        private ITaskService _taskService;
         
        
        public TaskWorker(RedisPriorityTaskQueue queue, ITaskService taskService)
        {
            _queue = queue;
            _cts = new CancellationTokenSource(); 
            _taskService = taskService;
            logger.Info("构建task worker");
        }
         

        public void Start()
        {
            Task.Run( async() => 
            {
                while (!_cts.IsCancellationRequested)
                {
                    //logger.Debug($"队列:{await _queue.GetLength()}");
                    var task = await _queue.DeQueue();

                    if (task.HasValue)
                    {
                          await ProcessTask(task.Value.taskId, task.Value.taskData);
                    }
                    else
                    {
                        //logger.Debug("队列为空，稍等片刻");
                        // 队列为空，稍等片刻
                        await Task.Delay(1000, _cts.Token);

                    } 
                }
            });
        }

        private async Task ProcessTask(string taskId, TaskModel taskData)
        {
            try { 
                logger.Info($"处理队列任务: {taskId}: {taskData}");

                //模拟任务用时30秒
                //await Task.Delay(3 * 60 * 1000);
                await _taskService.Execute(taskData);

                logger.Info($"处理完成队列任务: {taskId}: {taskData}");
            }
            catch (Exception ex)
            {
                logger.Error($"处理队列任务报错[{taskId}] : {ex.Message}");
            } 
        }
    }
}
