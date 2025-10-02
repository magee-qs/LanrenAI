using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAuth.ComfyUI
{
    public class UserInfo : IUserInfo
    {
        public string Id { get; set; }

        public string Account { get; set; }

        public string UserName { get; set; } 

        public string UserLevel { get; set; }

        public string TenantId { get; set; }

        public int Status { get; set; }

        /// <summary>
        /// 角色
        /// </summary>
        public List<string> Perms { get; set; }


        /// <summary>
        /// 用户点数
        /// </summary>
        public int Cost { get; set; }

        public FeeModel Fee { get; set; }

    }

    public class FeeModel
    {
        public string UserLevel { get; set; }

        public string Title { get; set; }

        public int Cost { get; set; }

        public string Multiple { get; set; }

        public string Store { get; set; }

        public DateTime Expire { get; set; }

        
    }


    
}
