using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OpenAuth
{
    public static class LambdaEntension
    {

        /// <summary>
        /// 创建lambda表达式：p=>true
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Expression<Func<T, bool>> True<T>()
        {
            return p => true;
        }

        /// <summary>
        /// 创建lambda表达式：p=>false
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Expression<Func<T, bool>> False<T>()
        {

            return p => false;
        }

        public static Expression<Func<T, TKey>> GetExpression<T, TKey>(this string propertyName)
        {
            return propertyName.GetExpression<T, TKey>(typeof(T).GetExpressionParameter());
        }

        public static ParameterExpression GetExpressionParameter(this Type type)
        {

            return Expression.Parameter(type, "p");
        }

        public static Expression<Func<T, TKey>> GetExpression<T, TKey>(this string propertyName, ParameterExpression parameter)
        {
            if (typeof(TKey).Name == "Object")
                return Expression.Lambda<Func<T, TKey>>(Expression.Convert(Expression.Property(parameter, propertyName), typeof(object)), parameter);
            return Expression.Lambda<Func<T, TKey>>(Expression.Property(parameter, propertyName), parameter);
        }

        public static Expression<Func<T, object>> GetExpression<T>(this string propertyName)
        {
            return propertyName.GetExpression<T, object>(typeof(T).GetExpressionParameter());
        }

        public static List<T> ToPage<T>(this IQueryable<T> query, Page page)
        {
            if (page.current <= 0)
            {
                page.current = 1;
            }
             
            IQueryable<T> orderBy = query.ToOrderBy(page);

            return Queryable.Skip(orderBy, (page.current- 1) * page.rows).Take(page.rows).ToList(); 
        }

        public static IQueryable<T> ToOrderBy<T>(this IQueryable<T> query, Page page)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            if (page.sorters != null)
            {
                foreach (var sorter in page.sorters)
                {
                    dict.Add(sorter.sidx, sorter.sord.ToUpper());
                }
            }

            if (dict.Count == 0)
            {
                if (!page.sord.IsEmpty())
                {
                    dict.Add(page.sidx, page.sord.ToUpper());
                }
            }

            if (dict.Count == 0)
            {
                dict.Add("Id", "ASC");
            }

            IOrderedQueryable<T> orderBy = null;

            //构建默认排序
            KeyValuePair<string, string> keyValuePair = dict.Last();
            var orderByExpression = keyValuePair.Key.GetExpression<T>();
            orderBy = keyValuePair.Value == "ASC"? orderBy = query.OrderBy(orderByExpression)
                : query.OrderByDescending(orderByExpression);

            foreach (var entry in dict)
            {
                if (entry.Key == keyValuePair.Key)
                {
                    continue;
                }

                orderByExpression = entry.Key.GetExpression<T>();

                orderBy = entry.Value == "ASC"? orderBy.ThenBy(orderByExpression) 
                    : orderBy.ThenByDescending(orderByExpression);

            }

            return orderBy;
        }

         
        public static Expression<Func<T, bool>> And<T>(List<ExpressionParameters> listExpress)
        {
            return listExpress.Compose<T>(Expression.And);
        }

        public static Expression<Func<T, bool>> Or<T>(this List<ExpressionParameters> listExpress)
        {
            return listExpress.Compose<T>(Expression.Or);
        }

        private static Expression<Func<T, bool>> Compose<T>(this List<ExpressionParameters> listExpress, Func<Expression, Expression, Expression> merge)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T), "p");
            Expression<Func<T, bool>> expression = null;
            foreach (ExpressionParameters exp in listExpress)
            {
                if (expression == null)
                {
                    expression = exp.Field.GetExpression<T, bool>(parameter);
                }
                else
                {
                    expression = expression.Compose(exp.Field.GetExpression<T, bool>(parameter), merge);
                }
            }
            return expression;
        }

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.And);
        }
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.Or);
        }

        public static Expression<T> Compose<T>(this Expression<T> first, Expression<T> second, Func<Expression, Expression, Expression> merge)
        {
            // build parameter map (from parameters of second to parameters of first)  
            var map = first.Parameters.Select((f, i) => new { f, s = second.Parameters[i] }).ToDictionary(p => p.s, p => p.f);
            // replace parameters in the second lambda expression with parameters from the first  
            var secondBody = ParameterRebinder.ReplaceParameters(map, second.Body);
            // apply composition of lambda expression bodies to parameters from the first expression  
            return Expression.Lambda<T>(merge(first.Body, secondBody), first.Parameters);
        }

        public class ExpressionParameters
        {
            public string Field { get; set; }
            public LinqExpressionType ExpressionType { get; set; }
            public object Value { get; set; }
            // public 
        }

        public class ParameterRebinder : ExpressionVisitor
        {

            private readonly Dictionary<ParameterExpression, ParameterExpression> map;
            public ParameterRebinder(Dictionary<ParameterExpression, ParameterExpression> map)
            {
                this.map = map ?? new Dictionary<ParameterExpression, ParameterExpression>();
            }

            public static Expression ReplaceParameters(Dictionary<ParameterExpression, ParameterExpression> map, Expression exp)
            {
                return new ParameterRebinder(map).Visit(exp);
            }
            protected override Expression VisitParameter(ParameterExpression p)
            {
                ParameterExpression replacement;
                if (map.TryGetValue(p, out replacement))
                {
                    p = replacement;
                }
                return base.VisitParameter(p);
            }
        }

    }
}
