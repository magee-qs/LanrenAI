using Microsoft.CodeAnalysis.CSharp.Syntax;
using Org.BouncyCastle.Ocsp;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAuth.Repository
{
    public static class QueryExtension
    {
        /// <summary>
        /// 根据实体对象生成查询语句
        /// </summary>
        /// <typeparam name="Entity"></typeparam>
        /// <param name="queryDTO"></param>
        /// <param name="sqlText"></param>
        /// <returns></returns>
        public static QueryResult ToEasyQuery<Entity>(this Entity queryDTO, string sqlText) where Entity : class, new() 
        {
            QueryResult result = new QueryResult() { SqlText = sqlText, Parameters = new List<DbParameter>() };

            if (queryDTO == null)
                return result;

            StringBuilder sb = new StringBuilder();
            var props =  queryDTO.GetType().GetProperties();
            foreach (var prop in props)
            {
                var value = prop.GetValue(queryDTO, null);
                var name  = prop.Name;
                if (value.IsEqual(value))
                    continue;

                if (name.StartsWith(QueryConstant.query_begin))
                {// 大于等于 

                }
                else if (name.StartsWith(QueryConstant.query_end))
                {//小于等于 

                }
                else if (name.StartsWith(QueryConstant.query_eq))
                {//等于 

                }
                else if (name.StartsWith(QueryConstant.query_in))
                {// in

                }
                else if (name.StartsWith(QueryConstant.query_nin))
                { // not in

                }
                else if (name.StartsWith(QueryConstant.query_left))
                {// left like 

                }
                else if (name.StartsWith(QueryConstant.query_right))
                {// right like
                  
                }else if(name.StartsWith(QueryConstant.query_like))
                {

                }

            }


            return result;

        }
    }


    public class QueryResult
    {
        public string SqlText { get; set; }
        
        public List<DbParameter> Parameters { get; set; }
    }


    /// <summary>
    /// 查询定量
    /// 变量命名 _begin_CreateTime =>  CreateTime >= '2025-05-25'
    /// 变量名  _end_CreateTime => CreateTime <= '2025-05-25'
    /// </summary>
    public class QueryConstant
    {
       
        public static readonly string query_begin = "_begin_";
        public static readonly string query_end = "_end_";
        public static readonly string query_eq = "_eq_";
        public static readonly string query_in = "_in_";
        public static readonly string query_nin = "_nin_";
        public static readonly string query_left = "_left_";
        public static readonly string query_right = "_right_";
        public static readonly string query_like = "_like_";

    }
}
