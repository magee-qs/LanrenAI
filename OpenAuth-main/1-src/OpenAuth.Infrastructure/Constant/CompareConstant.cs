using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAuth
{
    public static class CompareConstant
    {
        private static List<string> _list = new List<string> { "==", ">", "<", ">=", "<=", "in", "between", "!=", "not in", "like" };

        public static string Char(CompareEnum enumType)
        {
            if (enumType == CompareEnum.Equal) return "==";
            if (enumType == CompareEnum.NotEqual) return "!=";
            if (enumType == CompareEnum.Less) return "<";
            if (enumType == CompareEnum.LessOrEqual) return "<=";
            if (enumType == CompareEnum.Greater) return ">";
            if (enumType == CompareEnum.GreaterOrEqual) return ">=";
            if (enumType == CompareEnum.In) return "in";
            if (enumType == CompareEnum.NotIn) return "not in";
            if (enumType == CompareEnum.Between) return "between";
            if (enumType == CompareEnum.Like) return "like";

            throw new Exception("未知的数值比较类型");
        }

    }

    public enum CompareEnum
    {
        Equal,
        NotEqual,
        Less,
        LessOrEqual,
        Greater,
        GreaterOrEqual,
        In,
        Between,
        NotIn,
        Like
    }
}
