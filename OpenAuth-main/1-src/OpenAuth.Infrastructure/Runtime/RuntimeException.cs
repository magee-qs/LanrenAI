using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAuth
{
    /// <summary>
    /// 运行异常
    /// </summary>
    public class CommonException : Exception
    {
        private int _code;

        public CommonException(string message, int code)
            : base(message)
        {
            _code = code;
        }

        public CommonException(string message, int code, Exception innerException)
            : base(message, innerException)
        {

            _code = code;
        }

        public int Code { get { return _code; } }
    }

    /// <summary>
    /// 登录状态异常
    /// </summary>
    public class AuthenticationException : CommonException
    {
        public AuthenticationException(string message, int code)
            : base(message, code)
        {

        }
    }
    /// <summary>
    /// 权限异常
    /// </summary>
    public class PermissionException : CommonException
    {
        public PermissionException(string message, int code)
            : base(message, code)
        {
        }
    }

    /// <summary>
    /// 数据库操作异常
    /// </summary>
    public class DatabaseException : CommonException
    {
        public DatabaseException(string message, int code)
            : base(message, code)
        {
        }

        public DatabaseException(string message, int code, Exception innerException)
            : base(message, code, innerException)
        {

        }
    }

    /// <summary>
    /// 业务处理异常
    /// </summary>

    public class BizException : CommonException
    {
        public BizException(string message)
            : base(message, 500)
        { 
        }

        public BizException(string message, int code)
           : base(message, code)
        {
        }

        public BizException(string message, int code, Exception innerException)
            : base(message, code, innerException)
        {

        }
    }

    /// <summary>
    /// 定时任务异常
    /// </summary>
    public class QuartzJobException : CommonException
    {
        public QuartzJobException(string message)
           : base(message, 500)
        {
        }
        public QuartzJobException(string message, Exception ex)
            : base(message, 500, ex)
        { 
        }
        
    }

    /// <summary>
    /// 工作流异常
    /// </summary>
    public class FlowException : CommonException
    {
        public FlowException(string message, int code)
          : base(message, code)
        {
        }

        public FlowException(string message, int code, Exception innerException)
            : base(message, code, innerException)
        {

        }
    }

    /// <summary>
    /// 验证类异常
    /// </summary>
    public class ValidatorException : CommonException
    {
        private List<ValidationFailure> _errors;
        public ValidatorException(List<ValidationFailure> errors, int code)
           : base("数据验证未通过", code)
        {
            _errors = errors;
        }

        public ValidatorException(List<ValidationFailure> errors, string message, int code, Exception innerException)
            : base(message, code, innerException)
        {
            _errors = errors;
        }

        public List<string> GetErorrMessage()
        {
            if (_errors == null || _errors.Count == 0)
                return default;

            List<string> result = new List<string>();
            foreach (ValidationFailure failure in _errors)
            {
                result.Add(failure.ErrorMessage);
            }

            return result;
        }


        public string GetDefaultErrorMessage()
        {
            string _message = "验证未通过";
            if (_errors == null || _errors.Count == 0)
                return _message;

            _message = _errors.FirstOrDefault().ErrorMessage;
            return _message;
        }
    }


}
