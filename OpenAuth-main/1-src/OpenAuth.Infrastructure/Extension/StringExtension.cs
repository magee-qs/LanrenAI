using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OpenAuth
{
    public static class StringExtension
    {
        public static bool _windows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

        /// <summary>
        /// 调整windows和linux目录斜杠
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string ReplacePath(this string path)
        {
            if (string.IsNullOrEmpty(path))
                return "";

            if (_windows)
                return path.Replace("/", "\\");
            return path.Replace("\\", "/");
        }


        public static bool IsPhoneNumber(this string phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber))
                return false;

            if (phoneNumber.Length != 11)
                return false;

            if (new Regex(@"^1[3578][01379]\d{8}$").IsMatch(phoneNumber)
               || new Regex(@"^1[34578][01256]\d{8}").IsMatch(phoneNumber)
               || new Regex(@"^(1[012345678]\d{8}|1[345678][0123456789]\d{8})$").IsMatch(phoneNumber)
               )
                return true;
            return false;
        }

        /// <summary>
        /// 截取字符串
        /// </summary>
        public static string SubStr(this string str, int start, int length)
        {
            if (str.IsEmpty()) return string.Empty;

            return str.Substring(start, length);
        }
        /// <summary>
        /// 截取字符串
        /// </summary>
        public static string SubStr(this string str, int start)
        {
            if (str.IsEmpty()) return string.Empty;

            return str.Substring(start);
        }


        public static string ToPattern(this int number, string pattern = "0")
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < number; i++)
            {
                sb.Append(pattern);
            }
            return sb.ToString();
        }
    }
}
