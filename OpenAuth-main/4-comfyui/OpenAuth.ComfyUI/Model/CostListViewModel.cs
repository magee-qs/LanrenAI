using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAuth.ComfyUI.Model
{
    public class CostListViewModel : EntityString
    {
        public string UserId { get; set; }

        public string UserName { get; set; }

        public string Telephone { get; set; }

        public int Total { get; set; }

        public int Cost { get; set; }

        public int Leave { get; set; }

        public int Exipre { get; set; }

        public string Content { get; set; }

        public int State { get; set; }

        public DateTime CreateTime { get; set; }

        public string CreateBy { get; set; }

        public string CreateUser { get; set; }


    }
}
