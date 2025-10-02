using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAuth.Filters
{
    public class ResponseExceptionHandler : IExceptionHandler 
    {
        private ILogger<ResponseExceptionHandler> _logger;
        public ResponseExceptionHandler(ILogger<ResponseExceptionHandler> logger) { _logger = logger; }


        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            await HandlerException(httpContext, exception);

            WriteLog(exception);

            return true;
        }

        public async Task HandlerException(HttpContext context, Exception ex)
        {
            var result = HandlerExceptionMessage(ex);
            context.Response.StatusCode = 200;
            context.Response.ContentType = "application/json;charset=utf-8";

            
            await context.Response.WriteAsync(result.ToJson());
        }



        public virtual void WriteLog(Exception ex)
        {
            if (ex is ValidatorException)
                return;

            if (ex is AuthenticationException)
                return;

            //写入日志
            _logger.LogError(ex.Message, ex);

        }

        protected virtual AjaxResult HandlerExceptionMessage(Exception ex)
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
            else if (ex is DatabaseException)
            { 
                return AjaxResult.Error("数操作出错");
            }
            else if (ex is BizException)
            {
                return AjaxResult.Error("业务数据异常");
            }
            else if (ex is FlowException)
            {
                return AjaxResult.Error("工作流数据处理异常");
            }
            else if (ex is CommonException)
            {
                return AjaxResult.Error(ex.Message);
            }
            else
            {
                return AjaxResult.Error("服务处理异常");
            }
        }
    }
}
