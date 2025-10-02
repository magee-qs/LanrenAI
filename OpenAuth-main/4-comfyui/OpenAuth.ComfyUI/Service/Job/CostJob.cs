using NLog;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAuth.ComfyUI.Service.Job
{
    /// <summary>
    /// 点卡方法自动任务
    /// </summary>
    public class CostJob : IJob
    {
        protected NLog.Logger Logger = LogManager.GetCurrentClassLogger();
        public CostJob()
        { 
        }
        

        /// <summary>
        /// 每月的月初根据会员类型方法点券
        /// 有效期当前月份
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task Execute(IJobExecutionContext context)
        {
            //throw new NotImplementedException();
            Logger.Debug("方法没有实现");
            return Task.CompletedTask;
        }
    }
}
