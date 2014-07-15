using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;

namespace xFilter.Expressions
{
    /// <summary>
    /// 对IQueryable的扩展方法
    /// </summary>
    public static class QueryableExtensions
    {        
        /// <summary>
        /// 动态排序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="propertyName">属性名</param>
        /// <param name="ascending">是否升序</param>
        /// <returns></returns>
        public static IQueryable<T> OrderByExtensions<T>(this IQueryable<T> source, string propertyName, bool ascending) where T : class
        {

            if (string.IsNullOrEmpty(propertyName))
            {
                return source;
            }

            var type = typeof(T);
            var property = type.GetProperty(propertyName);
            if (property == null)
                throw new ArgumentException("propertyName", "不存在");

            var param = Expression.Parameter(type, "p");
            Expression propertyAccessExpression = Expression.MakeMemberAccess(param, property);
            var orderByExpression = Expression.Lambda(propertyAccessExpression, param);

            var methodName = ascending ? "OrderBy" : "OrderByDescending";
            var resultExp = Expression.Call(typeof(Queryable), methodName, new Type[] { type, property.PropertyType },
                                            source.Expression, Expression.Quote(orderByExpression));//第三个类型为泛型的类型
            return source.Provider.CreateQuery<T>(resultExp);
        }


    }
}
