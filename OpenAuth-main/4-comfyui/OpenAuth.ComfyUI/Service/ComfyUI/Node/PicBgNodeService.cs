using Newtonsoft.Json.Linq;
using OpenAuth.ComfyUI.Model.ComfyUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAuth.ComfyUI.Service.ComfyUI.Node
{
    public class PicBgNodeService : NodeService, IPicBgNodeService
    {
        protected override string PrepareJsonParam(string jsonTemplate, TaskModel model, JObject jsonParam)
        {
            var fileName = GetFileName(jsonParam, "image");

            if (fileName.IsEmpty())
                throw new CommonException("image参数为空", 500);

            //替换参数
            logger.Debug($"替换参数原始图片:{fileName}");
            jsonTemplate = jsonTemplate.Replace("$LoadImage$", fileName);


            var prodName = jsonParam["prodName"].ToStr("");
            if (prodName == "")
                throw new CommonException("缺少产品关键字描述", 500);

            //翻译
            prodName = BaiduTransformHelper.ToEnglish(prodName).Result;

             
            jsonTemplate = jsonTemplate.Replace("$prodName$", prodName.ToStr(""));


            //替换正面词和负面词
            logger.Debug($"替换尺寸参数:keyword={model.Keyword}, negtive={model.Negtive}");
            jsonTemplate = jsonTemplate.Replace("$keyword$", model.KeywordEN);
            jsonTemplate = jsonTemplate.Replace("$negative$", model.NegtiveEN);

            return jsonTemplate;
        }
    }

    public interface IPicBgNodeService : INodeService, IScopeDependency
    { 
    }
}
