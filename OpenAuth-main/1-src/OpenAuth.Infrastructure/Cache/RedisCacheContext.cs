using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAuth.Infrastructure.Cache
{
    public class RedisCacheContext : ICacheContext
    {
        private ConnectionMultiplexer _conn { get; set; }

        private IDatabase Database { get; set; }

        public IDatabase RedisDb { get { return Database; } }

        public RedisCacheContext(AppSetting appSetting)
        {
            _conn = ConnectionMultiplexer.Connect(appSetting.RedisConfig.ConnectionString);
            Database = _conn.GetDatabase(appSetting.RedisConfig.GetDatabase());
        }

        public T Get<T>(string key)
        {
            RedisValue redisValue = Database.StringGet(key);
            if (!redisValue.HasValue)
                return default;

            if (typeof(T) == typeof(string))
                return (T)System.Convert.ChangeType(redisValue, typeof(T));

            return redisValue.ToString().ToObject<T>();
        }

        public bool Remove(string key)
        { 
            return Database.KeyDelete(key);
        }

        public bool Set<T>(string key, T value)
        {
            if (typeof(T) == typeof(string))
            {
                return Database.StringSet(key, value.ToString());
            }
            else
            {
                return Database.StringSet(key, value.ToJson());
            }
        }

        public bool Set<T>(string key, T value, TimeSpan timeSpan)
        {
            if (typeof(T) == typeof(string))
            {
                return Database.StringSet(key, value.ToString(), timeSpan);
            }
            else
            {
                return Database.StringSet(key, value.ToJson(), timeSpan);
            }
        }


        public List<T> ListGet<T>(string key)
        {
            var values = Database.ListRange(key);
            return Read<T>(values);
        }

        public bool ListSet<T>(string key, List<T> values)
        {
            foreach (var item in values)
            {
                if (item != null)
                {
                    return Database.ListRightPush(key, item.ToJson()) == values.Count;
                }
            }
            return false;

        }

        public bool ListSet<T>(string key, List<T> values, TimeSpan timeSpan)
        {
            var result = ListSet(key, values);
            Database.KeyExpire(key, timeSpan);
            return result;
        }

        private List<T> Read<T>(RedisValue[] redisValues)
        {
            if (redisValues.Length == 0)
                return new List<T>();

            List<T> list = new List<T>();
            foreach (var value in redisValues)
            {
                if (value.HasValue)
                {
                    list.Add(value.ToString().ToObject<T>());
                }
            }
            return list;
        }



    }
}
