using NLog;
using OpenAuth.ComfyUI.Model.ComfyUI;
using OpenAuth.Infrastructure.Cache;
using StackExchange.Redis; 

namespace OpenAuth.ComfyUI.Service.ComfyUI.TaskQueue
{
    /// <summary>
    /// 队列任务
    /// </summary>
    public class RedisPriorityTaskQueue
    {
        protected IDatabase redisDb;

        protected ILogger logger = LogManager.GetCurrentClassLogger();

        //普通速度
        private const string QueueKey = "priority_task_queue";

        //加速
        private const string QueueKey_VIP = "priority_task_queue_vip";
         
        public static RedisPriorityTaskQueue Instance =  new RedisPriorityTaskQueue();

        private RedisPriorityTaskQueue()
        {
            var cacheContext = AutofacServiceProvider.GetService<ICacheContext>();
            redisDb = cacheContext.RedisDb;
        }

        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="taskData"></param>
        /// <param name="priority"></param>
        /// <returns></returns>
        public async Task Enqueue(string taskId, TaskModel taskData, int priority = int.MaxValue)
        {
            var score = priority == int.MaxValue ? DateTime.UtcNow.Ticks : priority;

            await  redisDb.SortedSetAddAsync(QueueKey, new SortedSetEntry[]
            {
                new SortedSetEntry(SerializeTask(taskId, taskData), score)
            });
        }

        /// <summary>
        /// 清除任务
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public async Task<bool> Remove(string taskId)
        {
            var all = await redisDb.SortedSetRangeByScoreWithScoresAsync(QueueKey_VIP, double.NegativeInfinity, double.PositiveInfinity);

            foreach (var entry in all)
            {
                var item = DeserializeTask(entry.Element);
                if (item.taskId == taskId)
                {
                    await redisDb.SortedSetRemoveAsync(QueueKey, entry.Element);
                    return true;
                }
            }

            all = await redisDb.SortedSetRangeByScoreWithScoresAsync(QueueKey, double.NegativeInfinity, double.PositiveInfinity);

            foreach (var entry in all)
            {
                var item = DeserializeTask(entry.Element);
                if (item.taskId == taskId)
                {
                    await redisDb.SortedSetRemoveAsync(QueueKey, entry.Element);
                    return true;
                }
            }

            return false;
        }
        /// <summary>
        /// 提升优先级
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="taskData"></param>
        /// <param name="priority"></param>
        /// <returns></returns>
        private async Task EnqueueVIP(string taskId, TaskModel taskData, int priority = int.MaxValue)
        {
            var score = priority == int.MaxValue ? DateTime.UtcNow.Ticks : priority;

            await redisDb.SortedSetAddAsync(QueueKey_VIP, new SortedSetEntry[]
            {
                new SortedSetEntry(SerializeTask(taskId, taskData), score)
            });
        }

        /// <summary>
        /// 取出任务
        /// </summary>
        /// <returns></returns>
        public async Task<(string taskId, TaskModel taskData)?> DeQueue()
        {
            //优先取出VIP任务
            var entries = await redisDb.SortedSetRangeByRankWithScoresAsync(QueueKey_VIP, 0, 0, Order.Ascending);
            if (entries.Length == 0)
            {
                entries = await redisDb.SortedSetRangeByRankWithScoresAsync(QueueKey, 0, 0, Order.Ascending);
                if (entries.Length == 0)
                    return null;

                var entry = entries[0];
                await redisDb.SortedSetRemoveAsync(QueueKey, entry.Element);
                return DeserializeTask(entry.Element);
            }
            else
            {
                var entry = entries[0];
                await redisDb.SortedSetRemoveAsync(QueueKey_VIP, entry.Element);
                return DeserializeTask(entry.Element);
            } 
        }

        public async Task<bool> ChangePriority(string taskId)
        {
            var item = await GetTask(taskId);
            if (item == null)
                return false;

            
            await redisDb.SortedSetRemoveAsync(QueueKey, item.Value.Element);

            var data = DeserializeTask(item.Value.Element);

            await EnqueueVIP(data.taskId,  new TaskModel() { Keyword="修改后测试任务"});
             
            return true;
        }


        private async Task<SortedSetEntry?> GetTask(string taskId)
        {
            var all = await redisDb.SortedSetRangeByScoreWithScoresAsync(QueueKey, double.NegativeInfinity, double.PositiveInfinity);

            foreach (var entry in all)
            {
                var item = DeserializeTask(entry.Element);
                if (item.taskId == taskId)
                {
                    return entry;
                }
            }
            return null;
        }

      

        public async Task<long> GetLength()
        {
            return await redisDb.SortedSetLengthAsync(QueueKey);
        }
         

        private string SerializeTask(string taskId, TaskModel taskData)
        {
            return $"{taskId}:{taskData.ToJson()}";
        }

        private (string taskId, TaskModel taskData) DeserializeTask(RedisValue value)
        {
            var parts = value.ToString().Split(new[] { ':' }, 2);
            return (parts[0], parts.Length > 1 ? parts[1].ToObject<TaskModel>() : null);
        }
    }
}
