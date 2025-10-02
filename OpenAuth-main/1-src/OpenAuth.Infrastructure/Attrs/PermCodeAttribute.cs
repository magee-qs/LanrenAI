using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAuth
{
    /// <summary>
    /// 权限操作
    /// </summary>
    public class PermCodeAttribute : Attribute
    {
        public string Code { get; }
        public PermCodeAttribute(string permCode)
        {
            Code = permCode;
        }
    }
}
