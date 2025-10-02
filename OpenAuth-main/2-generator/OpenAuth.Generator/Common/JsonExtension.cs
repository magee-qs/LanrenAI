using Newtonsoft.Json.Converters;
using Newtonsoft.Json;

namespace OpenAuth.Generator.Common
{
    public static class JsonExtension
    {
        public static string ToJson(this object data)
        {
            return ToJson(data, "yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// json序列化
        /// </summary>
        /// <param name="data"></param>
        /// <param name="datetimeformatter"></param>
        /// <returns></returns>
        public static string ToJson(this object data, string datetimeformatter)
        {
            var timeConverter = new IsoDateTimeConverter { DateTimeFormat = datetimeformatter };
            return JsonConvert.SerializeObject(data, timeConverter);
        }
    }
}
