using Newtonsoft.Json.Linq;
using OpenAuth.ComfyUI.Model.ComfyUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAuth.ComfyUI.Service.ComfyUI.Node
{
    public class SimpleKouTuService : NodeService, ISimpleKouTuService
    {
        protected override string PrepareJsonParam(string jsonTemplate, TaskModel model, JObject jsonParam)
        {
            //替换图像
            var fileList = jsonParam["fileList"] as JArray;
            if (fileList == null || fileList.Count == 0)
                throw new CommonException("没有上传图片", 500);

            var file = fileList[0] as JObject;
            var fileName = (string)file["name"];
            jsonTemplate = jsonTemplate.Replace("$LoadImage$", fileName);
            logger.Debug($"替换图片节点:{fileName}");

            return jsonTemplate;
        }
    }

    public interface ISimpleKouTuService : INodeService, IScopeDependency
    { }
}
