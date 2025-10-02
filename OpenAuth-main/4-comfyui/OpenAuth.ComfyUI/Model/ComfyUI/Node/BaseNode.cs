using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAuth.ComfyUI.Model.ComfyUI.Node
{
    public abstract class BaseNode
    {
        public string title { get; set; }

        public string class_type { get; set; }
    }
}
