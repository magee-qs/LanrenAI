using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAuth
{
    public static class ExceptionExtension
    {
        public static string GetInnerMessage(this Exception ex)
        {
            string message = ex.Message;
            var innerException = ex.InnerException;
            while (innerException != null) 
            {
                message = innerException.Message;
                innerException = innerException.InnerException;
            }

            return message;
        }
    }
}
