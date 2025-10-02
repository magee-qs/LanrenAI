using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAuth
{
    public class FieldConstant
    {
        /// <summary>
        /// 软删除
        /// </summary>
        public const string SoftDelete = "SoftDelete";
        /// <summary>
        /// 多租户
        /// </summary>
        public const string MustTenant = "MustTenant";

        /// <summary>
        /// 租户字段
        /// </summary>
        public const string TenantId = "TenantId";


        /// <summary>
        /// 软删除字段
        /// </summary>
        public const string IsDelete = "IsDeleted";



        public const string CreateByProperty = "CreateBy";

        public const string CreateTimeProperty = "CreateTime";

        public const string UpdateByProperty = "UpdateBy";

        public const string UpdateTimeProperty = "UpdateTime";


    }
}
