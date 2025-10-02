using Aop.Api.Domain;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using OpenAuth.ComfyUI.Controller.ComfyUI;
using OpenAuth.ComfyUI.Domain.ComfyUI;
using OpenAuth.ComfyUI.Model.ComfyUI;
using OpenAuth.ComfyUI.Model.ComfyUI.Node;
using OpenAuth.ComfyUI.Service.ComfyUI.TaskQueue;
using OpenAuth.Infrastructure.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAuth.ComfyUI.Service.ComfyUI.Node
{
    /// <summary>
    /// 任务参数转为位node参数
    /// </summary>
    public abstract class NodeService : INodeService
    {

        protected IAuthService authService;
        protected ITaskService taskService;
        protected IFlowService flowService;
        protected IWebHostEnvironment webHostEnvironment;
        protected ICacheContext cacheContext;
        protected AppSetting appSetting;
        protected object locker = new object();
        protected string prefixKey = "flow-json-template:";
        protected NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        protected NodeConfig nodeConfig;
        protected RedisPriorityTaskQueue _queue = RedisPriorityTaskQueue.Instance;
        protected IComfyUIServer ComfyUIServer;
        

        public NodeService()
        {
            authService = AutofacServiceProvider.GetService<IAuthService>();
            taskService = AutofacServiceProvider.GetService<ITaskService>();
            flowService = AutofacServiceProvider.GetService<IFlowService>();
            webHostEnvironment = AutofacServiceProvider.GetService<IWebHostEnvironment>();
            cacheContext = AutofacServiceProvider.GetService<ICacheContext>(); 
            appSetting = AutofacServiceProvider.GetService<AppSetting>();

            nodeConfig = new NodeConfig();
            var config = appSetting.ComfyUIConfig;
            nodeConfig.Runtime = config.timeout;
            nodeConfig.WaitTime = 3;
            ComfyUIServer = AutofacServiceProvider.GetService<IComfyUIServer>();
        }

        /// <summary>
        /// 获取表单数据
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        protected virtual TaskModel GetTaskModeFromParam(JObject json)
        { 
            var model = json.ToObject<TaskModel>();
            
            model.userId = authService.UserId;
            model.token = authService.Token;


            //启用翻译功能
            if (model.translacte)
            {
                //中文  to 英语
                if (!model.Keyword.IsEmpty())
                {
                    model.KeywordEN = BaiduTransformHelper.ToEnglish(model.Keyword).Result;
                }
                if (!model.Negtive.IsEmpty())
                {
                    model.NegtiveEN = BaiduTransformHelper.ToEnglish(model.Negtive).Result;
                }
            }

           
            return model;
        }
         
        /// <summary>
        /// 读取模板内容
        /// </summary>
        /// <param name="templatePath"></param>
        /// <returns></returns>
        protected virtual string GetJsonTemplate(string key, string templatePath)
        {
            string cacheKey = prefixKey + key;
            string jsonStirng = cacheContext.Get<string>(cacheKey);

            // 调试
            //jsonStirng = null;
            if (jsonStirng == null)
            {
                var path =  Path.Combine(FileHelper.GetBasePath(), "template", templatePath);
                jsonStirng = File.ReadAllText(path, Encoding.UTF8);
                cacheContext.Set(cacheKey, jsonStirng);
            }
            return jsonStirng;
        }

        /// <summary>
        /// 预处理表单参数
        /// </summary>
        /// <param name="jsonTemplate"></param>
        /// <param name="model"></param>
        /// <param name="jsonParam"></param>
        protected abstract string PrepareJsonParam(string jsonTemplate, TaskModel model, JObject jsonParam);

        /// <summary>
        /// 计算费用
        /// 生图基础费用 * 图片张数 * （1K倍数）
        /// </summary>
        /// <param name="jsonTemplate"></param>
        /// <param name="model"></param>
        /// <param name="jsonParam"></param>
        /// <returns></returns>
        protected virtual int CalCost(TaskModel model, JObject jsonParam, FlowEntity flow)
        {
            return model.Unit * flow.Cost;
        }
         
       

        protected string RandomSeed()
        {
            return CommonHelper.RandSeed(15);
        }


        public virtual async Task<AjaxResult> SaveTask(JObject json)
        {
            //获取表单数据
            var model = GetTaskModeFromParam(json);

            // 实例化参数
            model.NodeConfig = nodeConfig;

            //webId
            model.WebId = authService.WebId;

            //判断流程是否存在
            var flow = flowService.Find(model.FlowId);
            if (flow == null)
            {
                return AjaxResult.Error("工作流不存在");
            }

            //计算费用
            model.Cost = CalCost(model, json, flow);

            //保存任务
            taskService.SaveTask(model);

             
            //启动任务
            
            string template = "{}";
            try
            {
                template = GetJsonTemplate(flow.Id, flow.JsonTemplate);
            } catch (Exception ex) {
                logger.Error("读取模板参数出错", ex);
                throw new CommonException("读取模板参数出错", 500, ex);
            }

            try
            {
                logger.Debug("替换前原始参数");
                logger.Debug(template);
                //处理参数
                var jsonTemplate = PrepareJsonParam(template, model, json);
                logger.Debug("替换后参数");
                logger.Debug(jsonTemplate);

                //随机种子
                model.Seed = CommonHelper.RandSeed();
               
                // 替换种子参数 配置了随机种子时
                jsonTemplate = jsonTemplate.Replace("$seed$", model.Seed);


                // 出图数量
                if (model.Unit >= 1)
                {
                    // 替换种子参数 配置了随机种子时
                    jsonTemplate = jsonTemplate.Replace("$batchSize$", model.Unit.ToString());
                }

               

                //保存参数 
                model.FlowJson = jsonTemplate;
            } catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
                if (ex.GetType().IsAssignableFrom(typeof(CommonException)))
                {
                    throw;
                }
                else
                {
                    throw new CommonException("模板参数转换失败", 500, ex);
                }

                       
            } 

            //提交任务到队列
            await _queue.Enqueue(model.Id, model);

             
            return AjaxResult.OK(model, "");
        }

        /// <summary>
        /// 读取参数图片
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        protected string GetFileName(JObject json, string key)
        {
            if (json[key] == null || json[key].HasValues == false)
            {
                return "";
            }

            if (json[key] is JArray)
            {
                var file = json[key].FirstOrDefault();  
                return file["name"].ToStr(""); 
            }
            else
            {
                return json[key]["name"].ToStr("");
            }
               
        }

    }




    /// <summary>
    /// 任务参数转为位node参数
    /// </summary>
    public interface INodeService
    {
        /// <summary>
        /// 提交任务
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        Task<AjaxResult> SaveTask(JObject json);
    }
}
