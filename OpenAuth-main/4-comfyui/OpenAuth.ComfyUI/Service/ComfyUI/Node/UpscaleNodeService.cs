using Newtonsoft.Json.Linq;
using OpenAuth.ComfyUI.Model.ComfyUI;
using SixLabors.ImageSharp.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAuth.ComfyUI.Service.ComfyUI.Node
{
    /// <summary>
    /// 图片放大
    /// </summary>
    public class UpscaleNodeService : NodeService, IUpscaleNodeService
    {
        protected override  string PrepareJsonParam(string jsonTemplate, TaskModel model, JObject jsonParam)
        {
            //替换图像
            var fileList = jsonParam["fileList"] as JArray;
            if (fileList == null || fileList.Count == 0)
                throw new CommonException("没有上传图片", 500);

            // [ { fileName: '', filePath: '', ...}] 
            var file = fileList[0] as JObject;
            var fileName = (string)file["name"];
            jsonTemplate = jsonTemplate.Replace("$LoadImage$", fileName);

            logger.Debug($"替换图片节点:{fileName}");

            //获取放大系数
            var scaleType = (string)jsonParam["upscale"];
            logger.Debug($"获取放大系数:{scaleType}");

            var json = JObject.Parse(jsonTemplate);

            logger.Debug("获取放大参数");
            if (scaleType == "4x")
            {
                
                var str = json["4x"].ToString();
                logger.Debug(str);
                return str;
            }
            else
            {
                var str = json["2x"].ToString();
                logger.Debug(str);
                return str;
            }  
        }
    }


    /// <summary>
    /// 图片放大
    /// </summary>
    public interface IUpscaleNodeService : INodeService, IScopeDependency
    { 
    }
}
