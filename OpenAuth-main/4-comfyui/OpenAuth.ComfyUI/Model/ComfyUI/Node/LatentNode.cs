using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAuth.ComfyUI.Model.ComfyUI.Node
{
    public class LatentNode : BaseNode
    {
        public int width { get; set; }

        public int height { get; set; }

        public int batch_size { get; set; }

        public object ToJson()
        {
            var obj = new
            {
                inputs = new { width, height, batch_size },
                class_type,
                _meta =  new { title}
            };

            return obj;
        }
    } 
}
