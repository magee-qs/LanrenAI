using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAuth.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<GlobalExceptionFilter> _logger;
        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            var ex = context.Exception;

            if (ex is AuthenticationException)
            {

                if ((ex as AuthenticationException).Code == 401)
                {
                    context.HttpContext.Response.StatusCode = 200;
                    context.Result = new JsonResult(AjaxResult.NoAuth(ex.Message));
                }
                else
                {
                    context.HttpContext.Response.StatusCode = 200;
                    context.Result = new JsonResult(AjaxResult.NoAuthor(ex.Message));
                }
                context.ExceptionHandled = true;
            }
            else if (ex is ValidatorException)
            {
                context.HttpContext.Response.StatusCode = 200;
                var message = (ex as ValidatorException).GetDefaultErrorMessage();
                context.Result = new JsonResult(AjaxResult.Error(message));
                context.ExceptionHandled = true;
            }
            else if (ex is DatabaseException)
            {
                context.HttpContext.Response.StatusCode = 200;
                context.Result = new JsonResult(AjaxResult.Error("数操作出错"));
                context.ExceptionHandled = true;
            }
            else if (ex is BizException)
            {
                context.HttpContext.Response.StatusCode = 200;
                context.Result = new JsonResult(AjaxResult.Error("业务数据异常"));
                context.ExceptionHandled = true;
            }
            else if (ex is FlowException)
            {
                context.HttpContext.Response.StatusCode = 200;
                context.Result = new JsonResult(AjaxResult.Error("工作流数据处理异常"));
                context.ExceptionHandled = true;
            }
            else if (ex is CommonException)
            {
                context.HttpContext.Response.StatusCode = 200;
                context.Result = new JsonResult(AjaxResult.Error(ex.Message));
                context.ExceptionHandled = true;
            }
            else
            {

                context.HttpContext.Response.StatusCode = 200;
                context.Result = new JsonResult(AjaxResult.Error("服务处理异常"));
                context.ExceptionHandled = true;
            }

            WriteLog(ex.Message, ex);

        }

        private void WriteLog(string message, Exception ex)
        {
            if (ex is ValidatorException)
                return;

            if (ex is AuthenticationException)
                return;

            //写入日志
            _logger.LogError(ex.Message, ex);


        }
    }
}
