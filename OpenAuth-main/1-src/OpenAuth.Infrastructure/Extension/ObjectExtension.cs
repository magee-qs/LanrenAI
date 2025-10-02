using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OpenAuth
{
    public static class ObjectExtension
    {
        public static bool IsEmpty(this object obj)
        {
            if (obj == null) return true;

            string str = obj.ToString().Trim();
            if (string.IsNullOrEmpty(str)) return true;

            // null 判断为空
            if (str.ToLower() == "null") return true;

            return false;
        }

        public static bool IsNull(this object obj)
        {
            if (obj == null) return true;
            return false;
        }

        /// <summary>
        /// 判断是否数值类型
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsNumber(this object obj)
        {
            if (obj.IsEmpty())
                return false;
            string str = obj.ToStr("");
            bool isInt = Regex.IsMatch(str, @"^\d+");
            if (isInt) return true;

            bool isDouble = Regex.IsMatch(str, "^-?\\d+(\\.\\d+)?$");
            return isDouble;
        }

        /// <summary>
        /// 判断两个值是否相等
        /// </summary>
        /// <param name="obj1"></param>
        /// <param name="obj2"></param>
        /// <returns></returns>
        public static bool IsEqual(this object obj1, object obj2)
        {
            return obj1.ToStr(string.Empty).Equals(obj2.ToStr(string.Empty));
        }


        /// <summary>
        /// 转化为字符串并去掉首尾空格
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defVal"></param>
        /// <returns></returns>
        public static string ToStr(this object obj, string defVal)
        {
            if (obj == null) return defVal;

            return obj.ToString().Trim();
        }


        public static int ToInt(this object obj, int defVal)
        {
            if (obj == null) return defVal;

            string str = obj.ToString().Trim();

            int result = defVal;
            if (int.TryParse(str, out result))
            {
                return result;
            }
            else
            {
                return defVal;
            }
        }

        public static double ToDouble(this object obj, double defVal)
        {
            if (obj == null) return defVal;

            string str = obj.ToString().Trim();

            double result = defVal;
            if (double.TryParse(str, out result))
            {
                return result;
            }
            else
            {
                return defVal;
            }
        }

        public static float ToFloat(this object obj, float defVal)
        {
            if (obj == null) return defVal;

            string str = obj.ToString().Trim();

            float result = defVal;
            if (float.TryParse(str, out result))
            {
                return result;
            }
            else
            {
                return defVal;
            }
        }

        public static decimal ToDecimal(this object obj, decimal defVal)
        {
            if (obj == null) return defVal;

            string str = obj.ToString().Trim();

            decimal result = defVal;
            if (decimal.TryParse(str, out result))
            {
                return result;
            }
            else
            {
                return defVal;
            }
        }

        public static long ToLong(this object obj, long defVal)
        {
            if (obj == null) return defVal;

            string str = obj.ToString().Trim();

            long result = defVal;
            if (long.TryParse(str, out result))
            {
                return result;
            }
            else
            {
                return defVal;
            }
        }

        public static long? ToLong(this object obj, long? defVal)
        {
            if (obj == null) return defVal;

            string str = obj.ToString().Trim();

            long result = defVal == null ? defVal.Value : 0L;
            if (long.TryParse(str, out result))
            {
                return result;
            }
            else
            {
                return defVal;
            }
        }

        public static DateTime ToDateTime(this object obj, DateTime defVal)
        {
            if (obj == null) return defVal;

            string str = obj.ToString().Trim();

            DateTime result = defVal;
            if (DateTime.TryParse(str, out result))
            {
                return result;
            }
            else
            {
                return defVal;
            }
        }

        public static bool ToBool(this object obj, bool defVal)
        {
            if (obj == null) return defVal;

            string str = obj.ToString().Trim();

            bool result = defVal;
            if (bool.TryParse(str, out result))
            {
                return result;
            }
            else
            {
                return defVal;
            }
        }

        /// <summary>
        /// 中国式四舍五入
        /// </summary>
        /// <param name="data"></param>
        /// <param name="digit"></param>
        /// <returns></returns>
        public static decimal ToRound(this decimal data, int digit)
        {
            return Math.Round(data, digit, MidpointRounding.AwayFromZero);
        }

        /// <summary>
        /// 中国式四舍五入
        /// </summary>
        /// <param name="data"></param>
        /// <param name="digit"></param>
        /// <returns></returns>
        public static double ToRound(this double data, int digit)
        {
            return Math.Round(data, digit, MidpointRounding.AwayFromZero);
        }


        public static int RandNumber(int start, int end)
        {
            Random r = new Random();
            return r.Next(start, end);
        }


       
    }
}
