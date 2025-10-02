using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAuth.Infrastructure.Middleware
{
    public static class LogMessageExtension
    {
        public static string ToError(this LogMesasge logMesasge)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("ip:" + logMesasge.ip);
            sb.AppendLine("host:" + logMesasge.host);
            sb.AppendLine("browser:" + logMesasge.browser);
            sb.AppendLine("url:"+ logMesasge.url);
            sb.AppendLine("method:" + logMesasge.method);
            sb.AppendLine("startTime:" + logMesasge.startTime.ToString("yyyy-MM-dd HH:mm:ss:fff"));
            sb.AppendLine("endTime:" + logMesasge.endTime.ToString("yyyy-MM-dd HH:mm:ss:fff"));
            sb.AppendLine("costTime:" + logMesasge.costTime);
            sb.AppendLine("parameter:" + logMesasge.request);
            sb.AppendLine("controller:" + logMesasge.controller);
            sb.AppendLine("content:" + logMesasge.response);
            sb.AppendLine("error:" + logMesasge.message);
            sb.AppendLine("source:" + logMesasge.source);
            sb.AppendLine("trace:" + logMesasge.trace);
            sb.AppendLine();

            return sb.ToString();
        }

        public static string ToMessage(this LogMesasge logMesasge)
        {
            StringBuilder sb = new StringBuilder(); 
            sb.AppendLine("ip:" + logMesasge.ip);
            sb.AppendLine("host:" + logMesasge.host);
            sb.AppendLine("browser:" + logMesasge.browser);
            sb.AppendLine("url:" + logMesasge.url);
            sb.AppendLine("method:" + logMesasge.method);
            sb.AppendLine("startTime:" + logMesasge.startTime.ToString("yyyy-MM-dd HH:mm:ss:fff"));
            sb.AppendLine("endTime:" + logMesasge.endTime.ToString("yyyy-MM-dd HH:mm:ss:fff"));
            sb.AppendLine("costTime:" + logMesasge.costTime);
            sb.AppendLine("parameter:" + logMesasge.request);
            sb.AppendLine("controller:" + logMesasge.controller);
            sb.AppendLine("content:" + logMesasge.response);
           
            sb.AppendLine();

            return sb.ToString();
        }
    }
}
