using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Quartz.Logging.OperationName;

namespace OpenAuth.ComfyUI.Service.ComfyUI.Node
{
    public class NodeServiceProvider
    {
        public static INodeService Instance(string serviceName)
        {
            var serviceType = Type.GetType(serviceName);

            var service =  AutofacServiceProvider.GetService(serviceType) as INodeService;
            return service;
        }

        /// <summary>
        /// 通过json参数获取值
        /// </summary>
        /// <param name="jsonParam"></param>
        /// <returns></returns>
        public static INodeService Instance(JObject jsonParam)
        {
            var serviceName = jsonParam["service"].ToStr("");
            if (serviceName == "")
            {
                throw new CommonException("缺少service参数", 500);
            }

            // TextNode => OpenAuth.ComfyUI.Service.ComfyUI.Node.ITextNodeService
            var fullName = "OpenAuth.ComfyUI.Service.ComfyUI.Node.I" + serviceName + "Service";
            var service = Instance(fullName);
            if (service == null)
                throw new CommonException("服务未实现",500);

            return service;
        }
    }
}
