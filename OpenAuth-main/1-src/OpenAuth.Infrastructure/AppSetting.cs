using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAuth
{
    /// <summary>
    /// 运行参数
    /// </summary>
    public class AppSetting
    { 
        /// <summary>
        /// 文件上传路径
        /// </summary>
        public  UploadConfig UploadConfig { get; set; } 

        /// <summary>
        /// redis配置
        /// </summary>
        public  RedisConfig RedisConfig { get;  set; }

        /// <summary>
        /// oss参数
        /// </summary>
        public  OssConfig OssConfig { get; set; }

        /// <summary>
        /// token验证参数
        /// </summary>
        public  AuthConfig AuthConfig { get;  set; }


        public  string[] CorsUrlConfig { get;  set; }

        public  DbConnectionConfig DbConnectionConfig { get;  set; }

        public AlipayConfig AlipayConfig { get; set; }
         
        public ComfyUIConfig ComfyUIConfig { get; set; }

        public BaiduTransformConfig BaiduTransformConfig { get; set; }  

        public SMSConfig SMSConfig { get; set; }
    }


    /// <summary>
    /// redis连接参数
    /// </summary>
    public class RedisConfig
    {
        public string ConnectionString { get; set; }

        public string Database { get; set; } 

        public int GetDatabase()
        {
            return System.Convert.ToInt32(Database);
        }
    }

    /// <summary>
    /// 阿里云oss配置参数
    /// </summary>
    public class OssConfig
    {
        public string accessKey { get; set; }

        public string secretKey { get; set; }

        public string host { get; set; }

        public string bucket { get; set; }

        public string endpoint { get; set; }
    }

    public class AuthConfig
    {
        public string SecretKey { get; set; }

        public string ExpireTime { get; set; }

    }

    public class DbConnectionConfig
    {
        public string WriteConnection { get; set; }

        public string[] ReadConnection { get; set; }
    }

    public class UploadConfig
    {
        /// <summary>
        /// 默认目录 wwwroot
        /// </summary>
        public string Path { get; set; }

        public string[] Exclude { get; set; }

        public string UploadType { get; set; }

        /// <summary>
        /// 自定义目录 例如D:\wwworrt\files
        /// </summary>
        public string UploadPath { get; set; }
    }

    public class AlipayConfig
    {
        public string appId { get; set; }

        public string privateKey { get; set; }

        public string publicKey { get; set; }

        public string gatewayUrl { get; set; }

        public string signType { get; set; }

        public string charset { get; set; }

        public string notifyUrl { get; set; }

        public string returnUrl { get; set; }

        public string encryptKey { get; set; }

        public string encryptType { get; set; }

        public string format { get; set; }

        public string timeout { get; set; }

        public string version { get; set; }

        public string sellerId { get; set; }
    }

    public class ComfyUIConfig
    {
        public string url { get; set; }

        public int timeout { get; set; }

        public int retry { get; set; }

        public string path { get; set; }

        public string uploadImage { get; set; }

        public string uploadMask { get; set; }
    }

    public class BaiduTransformConfig
    {
        public string AppId { get; set; }

        public string SecretKey { get; set; }

        public string Url { get; set; }
    }


    public class SMSConfig
    {
        public string Endpoint { get; set; }

        public string Region { get; set; } 
        public Dictionary<string,string> Template { get; set;  }

        public string accessKey { get; set; }

        public string secretKey { get; set; }
    }
}
