using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAuth.Infrastructure.Cache
{
    public interface ICacheContext 
    {
        IDatabase RedisDb { get; }
        T Get<T>(string key);

        bool Set<T>(string key, T value);

        bool Set<T>(string key, T value, TimeSpan timeSpan);


        List<T> ListGet<T>(string key);

        bool ListSet<T>(string key, List<T> values);

        bool ListSet<T>(string key, List<T> values, TimeSpan timeSpan);


        bool Remove(string key);
    }
}
