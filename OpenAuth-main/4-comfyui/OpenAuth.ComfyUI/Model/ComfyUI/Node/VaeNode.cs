using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAuth.ComfyUI.Model.ComfyUI.Node
{
    public class VaeNode : BaseNode
    {
        public string sample_node { get; set; }

        public string vae_node { get; set; } 

       
          //        "14": {
          //  "inputs": {
          //    "samples": [
          //      "13",
          //      0
          //    ],
          //    "vae": [
          //      "10",
          //      2
          //    ]
          //  },
          //  "class_type": "VAEDecode",
          //  "_meta": {
          //    "title": "VAE解码"
          //  }
          //},
        public object ToJson()
        {
            var obj = new
            {
                inputs = new
                {
                    samples = new object[] { sample_node , 0},
                    vae = new object[] {vae_node, 2}
                },
                class_type = class_type,
                _meta = new
                {
                    title = title
                }
            };

            return obj;
        }
    }
}
