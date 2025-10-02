using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAuth
{
    public interface IGeneratorCode
    {
        string GetCode(string current);
    }


    public class GeneratorCode : IGeneratorCode
    {
        private List<IGeneratorCodeRule> rules;
        private string split = "";
        private int length = 0;

        public GeneratorCode(List<IGeneratorCodeRule> rules, string split)
        {
            this.rules = rules;
            this.split = split;

            length = 0;
            foreach (var rule in rules)
            {
                length += rule.GetLength();
            }
            length += split.Length * (rules.Count - 1);
        }

        public string GetCode(string current)
        {
            List<string> list = GetCodeList(current);
            List<string> codeList = new List<string>();
            if (list == null || list.Count == 0)
            {
                codeList = GetDefaultCode();
            }
            else
            {
                for (int i = 0; i < rules.Count; i++)
                {
                    var rule = rules[i];
                    var item = list[i];
                    string code = rule.GetCurrent(item);
                    codeList.Add(code);
                }
            }

            string str = ToCode(codeList);
            return str;
        }

        /// <summary>
        /// 将流程号分组
        /// </summary>
        /// <param name="current"></param>
        /// <returns></returns>
        private List<string> GetCodeList(string current)
        {
            List<string> list = new List<string>();
            if (current.IsEmpty())
                return new List<string>();

            //当前流水号与指定规则不匹配
            if (current.Length != this.length)
                return new List<string>();

            int startIndex = 0;
            for (int i = 0; i < rules.Count; i++)
            {
                var rule = rules[i];
                int currentLength = rule.GetLength();
                int endLength = startIndex + currentLength;

                string code = current.Substring(startIndex, currentLength);
                list.Add(code);
                //索引递增
                startIndex += currentLength;
                //分隔符号
                startIndex += split.Length;
            }
            return list;
        }

        private List<string> GetDefaultCode()
        {
            List<string> list = new List<string>();
            foreach (var rule in rules)
            {
                string code = rule.GetDefault();
                list.Add(code);
            }
            return list;
        }

        private string ToCode(List<string> list)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var code in list)
            {
                sb.Append(code).Append(split);
            }

            string str = SubLastString(sb, split.Length);
            return str;
        }

        private string SubLastString(StringBuilder sb, int subLength)
        {
            string str = string.Empty;
            if (sb.Length == 0)
                str = sb.ToString();

            sb.Remove(sb.Length - subLength, subLength);
            str = sb.ToString();
            return str;
        }
    }


    public interface IGeneratorCodeRule
    {
        /// <summary>
        /// 获取流水号长度
        /// </summary>
        /// <returns></returns>
        int GetLength();

        /// <summary>
        /// 获取默认值
        /// </summary>
        /// <returns></returns>
        string GetDefault();

        /// <summary>
        /// 当前字段设置为自动增长时 +1 ，否则返回当前值
        /// </summary>
        /// <param name="current"></param>
        /// <returns></returns>
        string GetCurrent(string current);
    }

    /// <summary>
    /// 自增流水号
    /// </summary>
    public class AutoGenenratorRule : IGeneratorCodeRule
    {
        private int length;
        private int defVal;

        private string pattern = "0000";

        public AutoGenenratorRule(int length, int defVal)
        {
            this.length = length;
            this.defVal = defVal;
            pattern = ToPattern();
        }

        /// <summary>
        /// 当前字段设置为自动增长时 +1 ，否则返回当前值
        /// </summary>
        /// <param name="current"></param>
        /// <returns></returns>
        public string GetCurrent(string current)
        {
            var num = current.ToInt(defVal);
            num++;
            return num.ToString(pattern);
        }

        public string GetDefault()
        {
            return defVal.ToString(pattern);
        }

        public int GetLength()
        {
            return length;
        }

        private string ToPattern()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                sb.Append("0");
            }
            return sb.ToString();
        }
    }

    public class DateGeneratorCodeRule : IGeneratorCodeRule
    {
        private string format = "yyyyMMdd";

        private int length;

        public DateGeneratorCodeRule(string format)
        {
            this.format = format;
            this.length = format.Length;
        }

        public string GetCurrent(string current)
        {
            if (current.IsEmpty())
            {
                return GetDefault();
            }
            else
            {
                return current;
            }
        }

        public string GetDefault()
        {
            DateTime date = DateTime.Now;
            return date.ToString(format);
        }

        public int GetLength()
        {
            return length;
        }
    }

    public class FixGeneratorCodeRule : IGeneratorCodeRule
    {
        private string fix = "";
        private int length;

        public FixGeneratorCodeRule(string fix, int length)
        {
            this.fix = fix;
            this.length = length;
        }

        public string GetCurrent(string current)
        {
            return fix;
        }

        public string GetDefault()
        {
            return fix;
        }

        public int GetLength()
        {
            return length;
        }
    }
}
