using NLog;
using OpenAuth.ComfyUI.Model.ComfyUI.Node;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace OpenAuth.ComfyUI.Service.ComfyUI.Node
{
    public class NodeHelper
    {
        protected static NLog.Logger logger = LogManager.GetCurrentClassLogger();
        // 1099339614842396
        public static long seed()
        { 
            
            long min = 1099339614842396L;
            long max = 1199999999999996L;

            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                byte[] buffer = new byte[8];
                rng.GetBytes(buffer);
                long rand = BitConverter.ToInt64(buffer, 0);

                return Math.Abs(rand %(max - min) + min);
            }
        }

        public static LatentNode GetLatentNode(string scale, int? width, int? height)
        {
            LatentNode node = new LatentNode()
            {
                class_type = "EmptyLatentImage",
                batch_size = 1,
                title = "空Latent图像"
            };
            switch (scale)
            {
                // 1: 1
                case "1:1":
                    node.width = 1024;
                    node.height = 1024;
                    break;
                // 16: 9
                case "16:9":
                    node.width = 1280;
                    node.height = 720;
                    break;
                // 9:16
                case "9:16":
                    node.width = 720;
                    node.height = 1280;
                    break;
                // 4:3
                case "4:3":
                    node.width = 1024;
                    node.height = 768;
                    break;
                // 3:4
                case "3:4":
                    node.width = 768;
                    node.height = 1024;
                    break;
                case "define":
                    node.width = width.ToInt(1024);
                    node.height = height.ToInt(1024);
                    break;
                default:
                    node.width = 768;
                    node.height = 1024;
                    break;
            }

            //node.width = 512;
            //node.height = 512;
            return node;
        }


        /// <summary>
        /// 保持comfyui心跳连接
        /// </summary>
        public static void PingComfyUI(int minute)
        {
            var config = ConfigHelper.AppSetting.ComfyUIConfig; 
            HttpClient client = new HttpClient();
            Task.Run(async () =>
            {
                while (true)
                {
                    try
                    {
                        string url = config.url + "/api/history/5a00d295-7741-4c3e-9b88-cc4f1054ccd";
                        logger.Debug("comfyui心跳检测");
                        var response = await client.GetAsync(url);
                        if (response.IsSuccessStatusCode)
                        {
                            //正常
                            logger.Debug("comfyui心跳检测正常");
                        }
                        else
                        {
                            //异常 发送短信通知
                            logger.Error("comfyui服务异常:" + response.ReasonPhrase);
                        }
                       
                    }
                    catch (Exception ex)
                    {
                        logger.Error("心跳检测抛出异常:" + ex.Message, ex);
                    }

                    //每分钟发送一次心跳
                    await Task.Delay(minute * 60 * 1000);

                }
                
            });
        }
    }
}
