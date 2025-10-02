using Newtonsoft.Json.Linq;
using OpenAuth.ComfyUI.Model.ComfyUI;
using OpenAuth.ComfyUI.Model.ComfyUI.Node;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Aliyun.OSS.Model.SelectObjectRequestModel.OutputFormatModel;

namespace OpenAuth.ComfyUI.Service.ComfyUI.Node
{
    public class PicNodeService : NodeService, IPicNodeService
    {
        protected override string PrepareJsonParam(string jsonTemplate, TaskModel model, JObject jsonParam)
        {
            var fileName = (string)jsonParam["image"]["fileName"];
            var maskName = (string)jsonParam["mask"]["name"]; 

            logger.Debug($"原始图片:{fileName}, 遮罩图片:{maskName}");

            //替换原始图片
            jsonTemplate = jsonTemplate.Replace("$LoadImage&", fileName);
            //替换遮罩图片
            jsonTemplate = jsonTemplate.Replace("$LoadMask$", maskName);

            //替换关键字
            logger.Debug($"替换关键字: {model.KeywordEN}");
            jsonTemplate  = jsonTemplate.Replace("$keyword$", model.KeywordEN);

            //替换关键字
            logger.Debug($"替换负面词: {model.NegtiveEN}");
            jsonTemplate = jsonTemplate.Replace("$negtive$", model.NegtiveEN);

            return jsonTemplate;
        }
    }

    public interface IPicNodeService : INodeService, IScopeDependency
    { 
    }
}
