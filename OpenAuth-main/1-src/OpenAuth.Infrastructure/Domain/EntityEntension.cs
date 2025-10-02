using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAuth
{
    public static class EntityEntension
    {
        /// <summary>
        /// 格式化输出entity
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static string ToEntityString<T>(this T entity) where T : class, new()
        {
            StringBuilder sb = new StringBuilder();
            if (entity == null)
                return sb.ToString();


            var props = entity.GetType().GetProperties();
            for (int i = 0; i < props.Length; i++)
            {
                if (i > 0)
                    sb.Append(", ");

                sb.Append(props[i].Name);
                sb.Append("=");
                try
                {
                    var value = props[i].GetValue(entity).ToStr("");
                    sb.Append(value);
                }
                catch (Exception ex)
                {
                    sb.Append("");
                }

            }

            return sb.ToString();
        }
    }
}
