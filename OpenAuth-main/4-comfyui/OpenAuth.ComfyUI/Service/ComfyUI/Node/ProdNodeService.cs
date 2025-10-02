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
    /// 产品精修
    /// </summary>
    public class ProdNodeService : NodeService, IProdNodeService
    {
        protected override string PrepareJsonParam(string jsonTemplate, TaskModel model, JObject jsonParam)
        {
            var fileName =  GetFileName(jsonParam, "image");

            if (fileName.IsEmpty())
                throw new CommonException("image参数为空", 500);

            //替换参数
            logger.Debug($"替换参数原始图片:{fileName}");
            jsonTemplate = jsonTemplate.Replace("$LoadImage$", fileName);

            return jsonTemplate;
        }
    }
    /// <summary>
    /// 产品精修
    /// </summary>
    public interface IProdNodeService : INodeService, IScopeDependency
    { 
    }
}
