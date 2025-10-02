using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAuth.ComfyUI.Model.ComfyUI.Node
{
    public class KSamplerNode : BaseNode
    {
        public long seed { get; set; }
        public int steps { get; set; }
        public int cfg { get; set; } 
        public string sampler_name { get; set; } 
        public string scheduler { get; set; }
        public int denoise { get; set; }

        /// <summary>
        /// 模型
        /// </summary>
        public string model_node { get; set; }

        public string positive_node { get; set; }

        public string negative_node { get; set; }

        public string latent_image_node { get; set; }
         
         
          //       "13": {
          //  "inputs": {
          //    "seed": 1099339614842396,
          //    "steps": 20,
          //    "cfg": 8,
          //    "sampler_name": "euler",
          //    "scheduler": "normal",
          //    "denoise": 1,
          //    "model": [
          //      "10",
          //      0
          //    ],
          //    "positive": [
          //      "11",
          //      0
          //    ],
          //    "negative": [
          //      "12",
          //      0
          //    ],
          //    "latent_image": [
          //      "16",
          //      0
          //    ]
          //  },
          //  "class_type": "KSampler",
          //  "_meta": {
          //    "title": "K采样器"
          //  }
          //},
        public object ToJson()
        {
            var obj = new 
            {
                inputs = new 
                {
                    seed = seed,
                    steps = steps,
                    cfg = cfg,
                    sampler_name = sampler_name,
                    scheduler = scheduler,
                    denoise = denoise,
                    model = new object[] { model_node , 0},
                    positive = new object[] { positive_node, 0 },
                    negative = new object[] { negative_node, 0 },
                    latent_image = new object[] { latent_image_node , 0 },
                },
                class_type = class_type,
                _meta = new { title = title }
            };
            return obj;
        }

    }
}
