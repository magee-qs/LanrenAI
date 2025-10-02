using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAuth
{
    public class CommonHelper
    {
        public static Stopwatch TimerStart()
        {
            Stopwatch watch = new Stopwatch();
            watch.Reset();
            watch.Start();
            return watch;
        }

        public static long TimerEnd(Stopwatch watch)
        {
            watch.Stop();
            return watch.ElapsedMilliseconds;
        }


        /// <summary>
        /// 生成日期编号
        /// 2000-09-25 11:45:40.345:9865
        /// </summary>
        /// <returns></returns>
        public static string CreaetDateNO()
        {
            Random random = new Random();
            string str = random.Next(1000, 10000).ToString();
            string code = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            code = code + str;
            return code;
        }

        /// <summary>
        /// 随机数
        /// </summary>
        /// <param name="length"></param> 
        /// <returns></returns>
        public static string Random(int length, bool lower =false )
        {
            Random rnd = new Random();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                var chars = lower ? charStrLower : charStr; 
                var ch = chars[rnd.Next(0, chars.Length)]  ;
                sb.Append(ch);
            }
            return sb.ToString();
        }

        public static string RandomNumber(int length)
        {
            Random rnd = new Random();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                sb.Append(rnd.Next(0, 9));
            }
            return sb.ToString();
        }

        /// <summary>
        /// 随机数+ 流水号(长度3-6位)
        /// </summary>
        /// <param name="length">随机数长度</param>
        /// <param name="serial">流水号长度</param>
        /// <returns></returns>
        public static string RandomSerial(int length, int serial = 0)
        {
            Random rnd = new Random();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                var ch = charStr[rnd.Next(0, charStr.Length)];
                sb.Append(ch);
            }

            if (serial > 0)
            {
                if (serial > 6)
                    serial = 6;
                if (serial < 3)
                {
                    serial = 3;
                }
                var serialMap = map[serial - 1];
                var num = rnd.Next(serialMap.min, serialMap.max);
                sb.Append(num);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 种子随机算法
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string RandSeed(int length = 15)
        {
            Random rd = new Random();
            StringBuilder sb = new StringBuilder();
            sb.Append(rd.Next(1, 9));
            for (int i = 0; i < length-1; i++)
            {
                sb.Append(rd.Next(0, 9));
            }
            return sb.ToString();
        }

        private class SerialMap
        {
            public int min;
            public int max;
        }

        private static List<SerialMap> map = new List<SerialMap>()
        {
            new SerialMap{ min = 0 , max = 10},
            new SerialMap{ min = 10 , max = 100},
            new SerialMap{ min = 100 , max = 1000},
            new SerialMap{ min = 1000 , max = 10000},
            new SerialMap{ min = 10000 , max = 100000},
            new SerialMap{ min = 100000 , max = 1000000},
        };


        private static string charStr = "0123456789qazwsxedcrfvtgbyhnujmiklopQAZWSXEDCRFVTGBYHNUJMIKLOP";

        private static string charStrLower = "0123456789qazwsxedcrfvtgbyhnujmiklop";


        /// <summary>
        /// 自动生成昵称
        /// </summary>
        /// <returns></returns>
        public static string GenerateNickName()
        {
            Random random = new Random();
            return adjNick[random.Next(adjNick.Length)] + adjName[random.Next(adjName.Length)];
        }

        private static string[] adjNick =  { "开朗的", "忧郁的", "活泼的", "沉稳的", "冷酷的", "热情的", "安静的" , "幽默的", "严肃的",
            "英俊的", "美丽的","可爱的","帅气的","萌萌的","强壮的","勇敢的"
        };

        private static string[] adjName = { "熊猫" , "老虎", "凤凰", "雄鹰" , "狐狸" , "狮子" ,"老虎" , "大象", "北极熊", "狼" , "猫", "巨龙"};
    }
}
