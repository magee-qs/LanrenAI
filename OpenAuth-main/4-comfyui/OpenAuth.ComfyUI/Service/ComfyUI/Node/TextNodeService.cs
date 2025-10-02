using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using Newtonsoft.Json.Linq;
using OpenAuth.ComfyUI.Domain.ComfyUI;
using OpenAuth.ComfyUI.Model.ComfyUI;
using OpenAuth.ComfyUI.Model.ComfyUI.Node;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAuth.ComfyUI.Service.ComfyUI.Node
{
    public class TextNodeService : NodeService, ITextNodeService
    {
        protected override string PrepareJsonParam(string jsonTemplate, TaskModel model, JObject jsonParam)
        {
            //获取尺寸
            var latentNode  = NodeHelper.GetLatentNode(model.Scale, model.Width, model.Height);
            logger.Debug($"替换尺寸参数:width={latentNode.width},height={latentNode.height}");
            jsonTemplate = jsonTemplate.Replace("$width$", latentNode.width.ToString());
            jsonTemplate = jsonTemplate.Replace("$height$", latentNode.height.ToString());

            //替换正面词和负面词
            logger.Debug($"替换尺寸参数:keyword={model.Keyword}, negtive={model.Negtive}");
            jsonTemplate = jsonTemplate.Replace("$positive$", model.Keyword);
            jsonTemplate = jsonTemplate.Replace("$negtive$", model.Negtive);
            return jsonTemplate;
        }

        //默认边长
        private int length = 1024;

    }

    /// <summary>
    /// 文生图
    /// </summary>
    public interface ITextNodeService : INodeService, IScopeDependency
    { 
    }
}
