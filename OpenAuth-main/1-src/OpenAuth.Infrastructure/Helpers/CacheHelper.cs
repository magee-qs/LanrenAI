using OpenAuth;
using OpenAuth.Infrastructure.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAuth
{
    public class CacheHelper
    {
        /// <summary>
        /// 加载缓存数据
        /// 非加锁操作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheContext"></param>
        /// <param name="key"></param>
        public static List<T> GetData<T>(ICacheContext cacheContext, string key, Func<List<T>> queryAction)
        {
            var list = cacheContext.ListGet<T>(key);
            if (list == null || list.Count == 0)
            {
                //查询数据
                list = queryAction();
                if (list != null && list.Count > 0)
                {
                    //写入缓存
                    cacheContext.ListSet(key, list);
                }
            }
            return list;
        }

        /// <summary>
        /// 加载缓存数据
        /// 非加锁操作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheContext"></param>
        /// <param name="key"></param>
        public static List<T> GetData<T>(ICacheContext cacheContext, string key, Func<List<T>> queryAction, TimeSpan timeSpan)
        {
            var list = cacheContext.ListGet<T>(key);
            if (list == null || list.Count == 0)
            {
                //查询数据
                list = queryAction();
                if (list != null && list.Count > 0)
                {
                    //写入缓存
                    cacheContext.ListSet(key, list, timeSpan);
                }
            }
            return list;
        }
    }
}
