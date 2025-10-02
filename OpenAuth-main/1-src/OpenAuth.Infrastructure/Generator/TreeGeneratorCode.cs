using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAuth
{
    public class TreeGeneratorCode : ITreeGeneratorCode
    {
        private string split = "";

        private int length = 2;

        private string pattern = "0";

        public TreeGeneratorCode(string split, int length)
        {
            this.split = split;
            this.length = length;
            pattern = length.ToPattern();
        }

        public string GetCode(string current, string parent)
        {
            if (current.IsEmpty())
            {
                if (parent.IsEmpty())
                {
                    return 1.ToString(pattern);
                }
                else
                {
                    return parent + split + 1.ToString(pattern);
                }
            }
            else
            {
                var holder = current.Substring(0, current.Length - length);
                var inc = current.Substring(current.Length - length);
                var num = inc.ToInt(0) + 1;
                return holder + split + num.ToString(pattern);
            }
        }

        public int GetLevel(string current)
        {
            if (current.IsEmpty())
                return 0;

            if (split.Length == 0)
            {
                return current.Length / length;
            }
            else
            {
                return current.Split(split).Count();
            }
        }
    }

    public interface ITreeGeneratorCode
    {
        string GetCode(string current, string parent);

        int GetLevel(string current);
    }
}
