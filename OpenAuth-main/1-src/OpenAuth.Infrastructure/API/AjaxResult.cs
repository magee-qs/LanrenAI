using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAuth
{
    public class AjaxResult
    {
        public bool success { get; set; }

        public string message { get; set; }

        public int code { get; set; }

        public object data { get; set; }


        public AjaxResult()
        {
            success = false;
            message = string.Empty;
            code = 0;
        }

        public static AjaxResult OK(object result, string message)
        {
            AjaxResult ajaxResult = new AjaxResult
            {
                success = true,
                message = message,
                code = CommonConstant.Http_OK,
                data = result
            };
            return ajaxResult;
        }

        public static AjaxResult OK<T>(List<T> data, int total)
        {
            AjaxResult ajaxResult = new AjaxResult
            {
                success = true,
                message = "操作成功",
                code = CommonConstant.Http_OK,
                data = new { rows = data,  total}
            };
            return ajaxResult;
        }

        public static AjaxResult OK(string message)
        {
            AjaxResult ajaxResult = new AjaxResult
            {
                success = true,
                message = message,
                code = CommonConstant.Http_OK
            };
            return ajaxResult;
        }

        public static AjaxResult Error(string message)
        {
            AjaxResult ajaxResult = new AjaxResult
            {
                success = false,
                message = message,
                code = CommonConstant.Http_Error
            };
            return ajaxResult;
        }

        public static AjaxResult NoAuth(string message)
        {
            AjaxResult ajaxResult = new AjaxResult
            {
                success = false,
                message = message,
                code = CommonConstant.Http_NoAuth
            };
            return ajaxResult;
        }

        public static AjaxResult NoAuthor(string message)
        {
            AjaxResult ajaxResult = new AjaxResult
            {
                success = false,
                message = message,
                code = CommonConstant.Http_NoPerm
            };
            return ajaxResult;
        }

        public static AjaxResult NotFound()
        {
            AjaxResult ajaxResult = new AjaxResult
            {
                success = false,
                message = "访问资源不存在",
                code = CommonConstant.Http_NotFound
            };
            return ajaxResult;
        }

        public static AjaxResult Page<T>(List<T> data, int total)
        {
            return  OK(new { rows = data, total }, "success");
        }
    }
}
