using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAuth.ComfyUI.Model.ComfyUI.Node
{
    /// <summary>
    /// checkpoint节点 
    /// </summary>
    public class CheckPointNode : BaseNode
    {
        public string ckpt_name; 

        /*{
            "inputs": {
            "ckpt_name": "anything-v5-PrtRE.safetensors"
            },
            "class_type": "CheckpointLoaderSimple",
            "_meta": {
            "title": "Checkpoint加载器(简易)"
            }
        }*/
        public object ToJson()
        {
            var obj = new
            {
                inputs = new 
                {
                    ckpt_name = ckpt_name
                },
                class_type = class_type, 
                _meta = new 
                {
                    title
                }
            };
            return obj;
        }
    }
}
