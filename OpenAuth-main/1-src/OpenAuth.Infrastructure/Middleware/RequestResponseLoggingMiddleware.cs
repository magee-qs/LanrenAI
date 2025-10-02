using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenAuth.Filters;
using OpenAuth.Infrastructure.Middleware;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace OpenAuth.Middleware
{
    /// <summary>
    /// 请求与返回中间件
    /// </summary>
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestResponseLoggingMiddleware> _log;
         

        private static readonly List<string> ExcludeUrls = ["/index/**", "/swagger/**"];

        /// <summary>
        /// 
        /// </summary> 
        public RequestResponseLoggingMiddleware(RequestDelegate next, ILogger<RequestResponseLoggingMiddleware> log)
        {
            _next = next;
            _log = log; 
        }


        private bool IsExcludePath(string path)
        {
            foreach (var url in ExcludeUrls)
            {
                if (url.EndsWith("/**"))
                {
                    //通配符
                    var _url = url.Replace("/**", "");
                    if (path.StartsWith(_url)) 
                    {
                        return true;
                    }
                }
                else
                {
                    //指定路径
                    if (path == url)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            #region 这里可以加入正则验证context.Path。 过滤不需要记录日志的api

            var path = context.Request.Path.ToString().ToLower();

            if (IsExcludePath(path))
            {
                await _next(context);
                return;
            } 
            #endregion

            // 启用耗时 日志记录
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var message = new LogMesasge();

            
            var request = context.Request;
            message.url = request.Path.ToString(); 
            message.host = request.Headers["host"].ToStr("");
            message.ip =   context.Request.Headers["REMOTE_ADDR"].ToStr("");
            message.method = request.Method; 
            message.startTime = DateTime.Now;
            message.trace = context.TraceIdentifier;
             
            // 获取请求body内容
            if (request.Method.ToLower().Equals("post"))
            {
                // 启用倒带功能，就可以让 Request.Body 可以再次读取
                request.EnableBuffering();
                // 文件上传 记录文件信息
                if (path.Contains("/upload"))
                {
                    var content = string.Join(",", request.Form.Files.Select(item => item.FileName));
                    message.request = $"收到上传文件:{content}"; 
                }
                else
                {
                    var sr = new StreamReader(request.Body, Encoding.UTF8);
                    //string content = sr.ReadToEnd();  //.Net Core 3.0 默认不再支持
                    var content = sr.ReadToEndAsync().Result;
                    message.request = content;
                    request.Body.Position = 0;
                }
            }
            else if (request.Method.ToLower().Equals("get"))
            {
                message.request = request.QueryString.Value;
            }

            // 获取Response.Body内容
            var originalBodyStream = context.Response.Body;

            Exception ex = null;
            
            using (var responseBody = new MemoryStream())
            {
                context.Response.Body = responseBody;
                ex = await CatchNext(context);

                message.response = await GetResponse(context.Response);
                message.endTime = DateTime.Now;
                message.status = context.Response.StatusCode;
                
                await responseBody.CopyToAsync(originalBodyStream);
            }

            // 响应完成记录时间和存入日志
            context.Response.OnCompleted(() =>
            {
                try
                {
                    stopwatch.Stop();

                    if (ex != null)
                    {
                        message.message = ex.GetInnerMessage();
                        message.source = ex.Source;
                        message.trace = ex.StackTrace;
   
                    }
                    message.costTime = stopwatch.ElapsedMilliseconds; 
                    
                    if (ex == null)
                    {
                        _log.LogInformation(message.ToMessage());
                    }
                    else
                    {
                        //记录错误日志
                        _log.LogError("发现异常");
                        _log.LogError(message.ToError());
                    }
                   
                    return Task.CompletedTask;
                }
                catch (Exception ex)
                {
                    return Task.FromException(ex);
                }
            });
        }

        private async Task<Exception> CatchNext(HttpContext context)
        {
            try
            {
                await _next(context);
                return null;
            }
            catch (Exception ex)
            { 
                
                await HandlerException(context, ex);
                return ex;
            }  
        }


        private static Task HandlerException(HttpContext context, Exception ex )
        {
            var result = HandlerExceptionMessage(ex);
            context.Response.StatusCode = 200;
            context.Response.ContentType = "application/json;charset=utf-8";
             
            return  context.Response.WriteAsync(result.ToJson());
        }
         

        /// <summary>
        /// 获取响应内容
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        private static async Task<string> GetResponse(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            var text = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);

            // 处理格式化数据
            text = text.Replace(" ", "").Replace("\r\n", " ").Replace("\\"," ");
            return text;
        }


        private static AjaxResult HandlerExceptionMessage(Exception ex)
        {
            if (ex is AuthenticationException)
            {
                if ((ex as AuthenticationException).Code == 401)
                {
                    return AjaxResult.NoAuth(ex.Message);
                }
                else
                {
                    return AjaxResult.NoAuthor(ex.Message);
                }
            }
            else if (ex is ValidatorException)
            {

                var message = (ex as ValidatorException).GetDefaultErrorMessage();
                return AjaxResult.Error(message);
            } 
            else 
            {
                var type1 = typeof(CommonException);
                var type2 = ex.GetType();
                if (type1.IsAssignableFrom(type2))
                {
                    return AjaxResult.Error(ex.Message);
                }
                else 
                {
                    return AjaxResult.Error("服务异常");
                }
                    
            }
        } 
    }
}
