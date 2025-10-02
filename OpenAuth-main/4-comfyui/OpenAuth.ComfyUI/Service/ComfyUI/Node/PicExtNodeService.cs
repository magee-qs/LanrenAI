using Newtonsoft.Json.Linq;
using OpenAuth.ComfyUI.Model.ComfyUI;
using OpenAuth.ComfyUI.Model.ComfyUI.Node;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAuth.ComfyUI.Service.ComfyUI.Node
{
    /// <summary>
    /// flux扩图
    /// </summary>
    public class PicExtNodeService : NodeService, IPicExtNodeService
    {
        protected override string PrepareJsonParam(string jsonTemplate, TaskModel model, JObject jsonParam)
        {
             
            var paramMode = jsonParam.ToObject<PicExtModel>();

            var fileName = GetFileName(jsonParam,"image");

            if (fileName.IsEmpty())
                throw new CommonException("image参数为空", 500);

            //替换参数
            logger.Debug($"替换参数原始图片:{fileName}");
            jsonTemplate = jsonTemplate.Replace("$LoadImage$", fileName);

            //替换尺寸参数
            logger.Debug($"替换尺寸参数: top = {paramMode.top}, left = {paramMode.left}, " +
                $"right = {paramMode.right} , buttom = {paramMode.bottom}, edge = {paramMode.edge}");

            jsonTemplate = jsonTemplate.Replace("$left$", paramMode.left.ToStr("0"));
            jsonTemplate = jsonTemplate.Replace("$right$", paramMode.right.ToStr("0"));
            jsonTemplate = jsonTemplate.Replace("$top$", paramMode.top.ToStr("0"));
            jsonTemplate = jsonTemplate.Replace("$buttom$", paramMode.bottom.ToStr("0"));
            jsonTemplate = jsonTemplate.Replace("$edge$", paramMode.edge.ToStr("0"));


            
            return jsonTemplate;

        }
    }

    /// <summary>
    /// flux扩图
    /// </summary>
    public interface IPicExtNodeService : INodeService, IScopeDependency
    { 
    }
}
