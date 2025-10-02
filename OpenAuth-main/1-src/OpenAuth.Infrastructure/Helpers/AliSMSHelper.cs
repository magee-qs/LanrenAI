using AlibabaCloud.SDK.Dysmsapi20170525.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tea;

namespace OpenAuth
{
    /// <summary>
    /// 阿里云短信
    /// </summary>
    public class AliSMSHelper
    {
        private static NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        
        /// <summary>
        /// 使用凭据初始化账号Client
        /// </summary>
        /// <returns></returns>
        public static AlibabaCloud.SDK.Dysmsapi20170525.Client CreateClient()
        {
            SMSConfig smsConfig = ConfigHelper.AppSetting.SMSConfig;
            
            var config = new AlibabaCloud.OpenApiClient.Models.Config()
            { 
                AccessKeyId = smsConfig.accessKey,
                AccessKeySecret = smsConfig.secretKey,
                Endpoint = smsConfig.Endpoint,
                RegionId = smsConfig.Region
            }; 
             
            return new AlibabaCloud.SDK.Dysmsapi20170525.Client(config);
        }

        /// <summary>
        /// 发送信息
        /// </summary>
        /// <param name="code"></param>
        /// <param name="telephone"></param>
        /// <returns></returns>
        public static void SendMessage(string code, string telephone, string templateKey)
        {
            var client = CreateClient();
            SMSConfig smsConfig = ConfigHelper.AppSetting.SMSConfig;
            var template = smsConfig.Template[templateKey];
            if (template.IsEmpty())
            {
                Logger.Error("短信模板不存在");
                throw new CommonException("短信模板不存在", 500); 
            }
            AlibabaCloud.SDK.Dysmsapi20170525.Models.SendSmsRequest sendSmsRequest = new AlibabaCloud.SDK.Dysmsapi20170525.Models.SendSmsRequest
            {
                PhoneNumbers = telephone,
                SignName = "麦吉",
                TemplateCode = "SMS_321470808",
                //TemplateParam = "{\"code\":\"1234\"}",
                TemplateParam =  new { code = code }.ToJson()
            };

            AlibabaCloud.TeaUtil.Models.RuntimeOptions runtime = new AlibabaCloud.TeaUtil.Models.RuntimeOptions();
            try
            {
                AlibabaCloud.SDK.Dysmsapi20170525.Models.SendSmsResponse resp = client.SendSmsWithOptions(sendSmsRequest, runtime);
                Logger.Info("短信发送成功");
                Logger.Info(AlibabaCloud.TeaUtil.Common.ToJSONString(resp)); 
            }
            catch (TeaException error)
            {
                // 此处仅做打印展示，请谨慎对待异常处理，在工程项目中切勿直接忽略异常。
                // 错误 message
                Logger.Error(error.Message);
                // 诊断地址
                Logger.Error(error.Data["Recommend"]);
                AlibabaCloud.TeaUtil.Common.AssertAsString(error.Message);

                throw new CommonException("短信发送失败", 500);
            }
            catch (Exception _error)
            {
                TeaException error = new TeaException(new Dictionary<string, object>
                {
                    { "message", _error.Message }
                });
                // 此处仅做打印展示，请谨慎对待异常处理，在工程项目中切勿直接忽略异常。
                // 错误 message
                Logger.Error(error.Message);
                // 诊断地址
                Logger.Error(error.Data["Recommend"]);
                AlibabaCloud.TeaUtil.Common.AssertAsString(error.Message);

                throw new CommonException("短信发送失败", 500);
            }
        }
    }

    public class  SMSTemplateEnum
    {
        public static readonly string Loign = "login";
    }
}
