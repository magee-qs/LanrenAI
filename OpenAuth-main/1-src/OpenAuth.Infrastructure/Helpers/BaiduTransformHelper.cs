using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using System.Web;

namespace OpenAuth
{
    /// <summary>
    /// 百度翻译工具类
    /// </summary>
    public class BaiduTransformHelper
    {
        private static BaiduTransformConfig config = ConfigHelper.AppSetting.BaiduTransformConfig;

        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// 翻译内容
        /// </summary>
        /// <param name="inputStr">输入文本</param>
        /// <param name="from">源语言</param>
        /// <param name="to">目标语言</param>
        /// <returns></returns>
        public static async Task<string> Transform(string inputStr, string from, string to)
        {
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("ContentType", "text/html;charset=UTF-8");
            httpClient.Timeout = TimeSpan.FromSeconds(30);

            //处理输入字符串 替换中文标点符号
            inputStr = ReplaceChar(inputStr);

            string url = GetQueryUrl(inputStr, from, to);
            HttpResponseMessage response = await httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string str =  await  response.Content.ReadAsStringAsync();

                var json = str.ToJObject();
                var result = json["trans_result"] as JArray;
                var data = result[0];
                //var filename = image["filename"].ToString();
                var dst = data["dst"].ToString();
                var src = data["src"].ToString();
                logger.Debug("百度翻译 from:" + src);
                logger.Debug("百度翻译 to:" + dst);
                return dst;
            }
            else
            {
                logger.Error("百度api出错:" + response.ReasonPhrase, response);
                return string.Empty;
            } 
        }
         

        public static async Task<string> ToEnglish(string inputStr)
        {
            return await Transform(inputStr, TransformLang.Chinese, TransformLang.English);
        }


        public static async Task<string> ToChinese(string inputStr)
        {
            return await Transform(inputStr, TransformLang.English, TransformLang.Chinese);
        }

        /// <summary>
        /// 构建查询字符串
        /// </summary>
        /// <param name="inputStr"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        private static string GetQueryUrl(string inputStr, string from, string to)
        {
             
            Random rd = new Random();
            string salt = rd.Next(100000).ToString(); 
            string sign = EncryptString(config.AppId + inputStr + salt + config.SecretKey);

            StringBuilder sb = new StringBuilder();
            sb.Append(config.Url).Append("?")
                .Append("q=").Append(HttpUtility.UrlEncode(inputStr))
                .Append("&from=").Append(from)
                .Append("&to=").Append(to)
                .Append("&appid=").Append(config.AppId)
                .Append("&salt=").Append(salt)
                .Append("&sign=").Append(sign);

            return sb.ToString();
        }

        private static string EncryptString(string str)
        {
            MD5 md5 = MD5.Create();
            // 将字符串转换成字节数组
            byte[] byteOld = Encoding.UTF8.GetBytes(str);
            // 调用加密方法
            byte[] byteNew = md5.ComputeHash(byteOld);
            // 将加密结果转换为字符串
            StringBuilder sb = new StringBuilder();
            foreach (byte b in byteNew)
            {
                // 将字节转换成16进制表示的字符串，
                sb.Append(b.ToString("x2"));
            }
            // 返回加密的字符串
            return sb.ToString();
        }


        private static string ReplaceChar(string inputStr)
        {
            if (inputStr.IsEmpty())
            {
                return string.Empty;
            }

            return inputStr.Replace("，", ",").Replace("。",".");
        }
    }

    public class TransformLang
    {
        public static readonly string Chinese = "zh";

        public static readonly string English = "en";
    }
}
