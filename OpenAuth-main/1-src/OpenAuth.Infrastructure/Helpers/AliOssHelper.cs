using Aliyun.OSS;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace OpenAuth.Infrastructure.Helpers
{
    /// <summary>
    /// alioss 存储
    /// </summary>
    public class AliOssHelper
    {
        private static NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        private static OssConfig config = ConfigHelper.AppSetting.OssConfig;


        public static OssClient GetClient()
        {
            return new OssClient(config.endpoint, config.accessKey, config.secretKey);
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="fileName">存储在OSS上的文件名</param>
        /// <param name="filePath">本地文件路径</param>
        /// <returns></returns>
        public static void Upload(string fileName, string filePath)
        {
            try
            {
                var client = GetClient(); 
                fileName = NormalizeDirectoryPath(fileName);
                client.PutObject(config.bucket, fileName, filePath); 
            }
            catch (Exception ex)
            {
                Logger.Error($"上传文件失败:{ex.Message}",ex);
                throw new CommonException("oss文件保存失败", 500);
            }
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="fileName">存储在OSS上的文件名</param>
        /// <param name="stream">文件流</param>
        /// <returns></returns>
        public static void  Upload(string fileName, Stream stream)
        {
            try
            {
                var client = GetClient();
                fileName = NormalizeDirectoryPath(fileName);
                var putObjectRequest = new PutObjectRequest(config.bucket, fileName, stream);
                client.PutObject(putObjectRequest); 
            }
            catch (Exception ex)
            {
                Logger.Error($"上传文件失败:{ex.Message}", ex);
                throw new CommonException("oss文件保存失败", 500);
            }
        }


        public static void download(string fileName, string filePath)
        {
            try
            {
                var client = GetClient();

                var obj = client.GetObject(config.bucket, fileName);
                using (var requestStream = obj.Content)
                {
                    using (var fs = File.Open(filePath, FileMode.Create))
                    {
                        byte[] buf = new byte[1024];
                        var len = 0;
                        while ((len = requestStream.Read(buf, 0, 1024)) != 0)
                        {
                            fs.Write(buf, 0, len);
                        }
                    }
                } 
            }
            catch (Exception ex)
            {
                Logger.Error($"下载文件失败:{ex.Message}", ex);
                throw new CommonException("oss文件下载失败", 500);
            }
        }

        /// <summary>
        /// 从OSS获取文件流
        /// </summary>
        /// <param name="objectName">OSS上的文件名</param>
        /// <returns>文件流</returns>
        public static Stream GetFileStream(string fileName)
        {
            try
            {
                var client = GetClient();

                // 获取文件流
                var obj = client.GetObject(config.bucket, fileName);
                return obj.Content;
            }
            catch (Exception ex)
            {
                Logger.Error($"下载文件失败:{ex.Message}");
                return null;
            }
        }

        public static PolicyToken GetPolicyToken()
        {
            
            var expire = DateTime.Now.AddMinutes(60);
            //policy
            var policy = new PolicyConfig()
            {
                conditions = new List<List<object>>()
                {
                    new List<object>()
                    {
                        "content-length-range",
                         0 ,
                         // 10M受限
                         1024 * 1024 * 10
                    }
                }
            };
            
            // 将 Policy 转换为 JSON 字符串
            var policyJson = JsonConvert.SerializeObject(policy);
            var policyBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(policyJson));
            var signature = ComputeSignature(config.secretKey, policyBase64);
            var expireTime = ToUnixTime(expire);

            return new PolicyToken()
            {
                accessKeyId = config.accessKey,  
                host = config.host, 
                policy = policyBase64, 
                signature = signature, 
                expire = expireTime
            };
        }

        private static string ToUnixTime(DateTime dtime)
        {
            const long ticksOf1970 = 621355968000000000;
            var expires = ((dtime.ToUniversalTime().Ticks - ticksOf1970) / 10000000L)
                .ToString(CultureInfo.InvariantCulture);

            return expires;
        }

        private static string ComputeSignature(string key, string data)
        {
            using (var algorithm = new HMACSHA1())
            {
                algorithm.Key = Encoding.UTF8.GetBytes(key.ToCharArray());
                return System.Convert.ToBase64String(
                    algorithm.ComputeHash(Encoding.UTF8.GetBytes(data.ToCharArray())));
            }
        }

        public static string GetFullPath(string path)
        {
            if (path.IsEmpty())
            {
                return null;
            }
            return config.host + "/" + path;
        }

        private static string NormalizeDirectoryPath(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                return string.Empty;

            return filePath.TrimStart('/');
        }
         
    }

    public class PolicyToken
    {
        public string accessKeyId { get; set; }
        public string policy { get; set; }
        public string signature { get; set; }
        public string dir { get; set; }
        public string host { get; set; }
        public string expire { get; set; }
        public string callback { get; set; }

    }

    public class PolicyConfig
    {
        public string expiration { get; set; }
        public List<List<Object>> conditions { get; set; }
    }


}
