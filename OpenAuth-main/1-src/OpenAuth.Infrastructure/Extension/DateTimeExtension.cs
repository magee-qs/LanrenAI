using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAuth
{
    public static class DateTimeExtension
    {
        /// <summary>
        /// 本地时间
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static long ToUnixTime(this DateTime dateTime)
        {
            DateTime unixTime = new DateTime(1970, 1, 1, 0, 0, 0);
            return (long)(dateTime - unixTime).TotalSeconds;
        }
        /// <summary>
        /// 本地时间
        /// </summary>
        /// <param name="unixTime"></param>
        /// <returns></returns>
        public static DateTime ToUnixTime(this long unixTime)
        {
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0);
            return dateTime.AddSeconds(unixTime);
        }

        /// <summary>
        /// 当前日期最小值
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime ToMin(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day);
        }
        
        /// <summary>
        /// 当前日期最大值
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime ToMax(this DateTime date)
        {
            return  new DateTime(date.Year, date.Month, date.Day);
             
        }

        /// <summary>
        /// 转为为整形日期 20251005
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static int ToDate(this DateTime date)
        {
            var str = date.ToString("yyyyMMdd");
            return str.ToInt(0);
        }

        /// <summary>
        /// 转换为日期 20251005 => 2025-10-05
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime ToDate(this int date)
        {
            var dateStr = date.ToString();
            string year = dateStr.SubStr(0, 4);
            string month = dateStr.SubStr(4, 2);
            string day = dateStr.SubStr(6,2);

            string _date = year + "-" + month + "-" + day;

            return DateTime.Parse(_date);
        }

        public static DateTime ToFisrtDay(this DateTime date)
        {
            string str = date.ToString("yyyy-MM") + "-01";
            return DateTime.Parse(str);
        }

        public static DateTime ToLastDay(this DateTime date)
        {
            var firtDay  = date.ToFisrtDay();
            return firtDay.AddDays(-1).AddMonths(1);
        }
    }
}
