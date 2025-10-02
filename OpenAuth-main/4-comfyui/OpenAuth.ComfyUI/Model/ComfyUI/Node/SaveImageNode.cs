using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAuth.ComfyUI.Model.ComfyUI.Node
{
    public class SaveImageNode : BaseNode
    {
        public string prefix { get; set; }

        public string image_node { get; set; }
         

        public object ToJson()
        {
            var obj = new
            {
                inputs = new
                {
                    filename_prefix = prefix,
                    images = new object[] { image_node, 0 }
                },
                class_type,
                _meta = new { title }
            };
            return obj;
        }
    }
}
