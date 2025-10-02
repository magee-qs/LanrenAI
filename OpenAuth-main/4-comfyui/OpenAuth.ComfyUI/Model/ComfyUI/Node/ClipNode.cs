using Aop.Api.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAuth.ComfyUI.Model.ComfyUI.Node
{
    /// <summary>
    /// 文本输入
    /// </summary>
    public class ClipNode : BaseNode
    {
        public string text { get; set; }

        /// <summary>
        /// 连接节点
        /// </summary>
        public string model_node { get; set; }
         
         
        //      "12": {
        //  "inputs": {
        //    "text": "",
        //    "clip": [
        //      "10",
        //      1
        //    ]
        //  },
        //  "class_type": "CLIPTextEncode",
        //  "_meta": {
        //    "title": "CLIP文本编码器"
        //  }
        //},
        public object ToJson()
        {
            var obj = new
            {
                inputs = new 
                {
                    text = text,
                    clip = new object[] { model_node, 1}
                },
                class_type  ,
                _meta = new 
                {
                    title = title
                }
            };

            return obj;
        }
    }
}
