using Newtonsoft.Json.Linq;
using OpenAuth.ComfyUI.Model.ComfyUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAuth.ComfyUI.Service.ComfyUI.Node
{
    /// <summary>
    /// koloars图生图
    /// </summary>
    public class PicPicNodeService : NodeService, IPicPicNodeService
    {
        protected override string PrepareJsonParam(string jsonTemplate, TaskModel model, JObject jsonParam)
        {
            //替换图片
            var fileName = GetFileName(jsonParam, "image");
            if (fileName.IsEmpty())
                throw new CommonException("没有上传图片", 500);

            logger.Debug($"替换图片参数:fileName={fileName}");
            jsonTemplate = jsonTemplate.Replace("$LoadImage$", fileName);

            //获取尺寸
            var latentNode = NodeHelper.GetLatentNode(model.Scale, model.Width, model.Height);
            logger.Debug($"替换尺寸参数:width={latentNode.width},height={latentNode.height}");
            jsonTemplate = jsonTemplate.Replace("$width$", latentNode.width.ToString());
            jsonTemplate = jsonTemplate.Replace("$height$", latentNode.height.ToString());

            //替换正面词和负面词
            logger.Debug($"替换尺寸参数:keyword={model.Keyword}, negtive={model.Negtive}");
            jsonTemplate = jsonTemplate.Replace("$positive$", model.Keyword);
            jsonTemplate = jsonTemplate.Replace("$negtive$", model.Negtive);

            
            return jsonTemplate;
        }
    }

    /// <summary>
    /// koloars图生图
    /// </summary>
    public interface IPicPicNodeService : INodeService, IScopeDependency
    {
        
    }
}
