using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Gateways.NET.ViewModels
{
    public static class QueryHelper
    {
        /// <summary>
        /// Apply pagination pattern to a queryable source
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="query"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static IQueryable<TEntity> ApplyPagging<TEntity>(IQueryable<TEntity> query, int page, int pageSize)
            where TEntity : class
        {
            return query.Skip(pageSize * (page - 1)).Take(pageSize);
        }

        /// <summary>
        /// Apply a sort order to a queryable source
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="query"></param>
        /// <param name="orders"></param>
        /// <returns></returns>
        public static IQueryable<TEntity> ApplyOrder<TEntity>(IQueryable<TEntity> query, IEnumerable<OrderColumn> orders)
            where TEntity : class
        {
            if (orders?.Any() == true)
            {
                IOrderedQueryable<TEntity> orderQuery = null;

                var ordersToArray = orders.ToArray();

                for (int i = 0; i < ordersToArray.Length; i++)
                {
                    var expression = BuildExpression<TEntity>(ordersToArray[i].Name);
                    if (i == 0)
                    {
                        orderQuery = ordersToArray[i].Ascendant ? query.OrderBy(expression) : query.OrderByDescending(expression);
                    }
                    else
                    {
                        orderQuery = ordersToArray[i].Ascendant ? orderQuery.ThenBy(expression) : orderQuery.ThenByDescending(expression);
                    }
                }

                if (orderQuery != null)
                    query = orderQuery;
            }

            return query;
        }

        /// <summary>
        /// Apply filters to a queryable source
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="query"></param>
        /// <param name="criterias"></param>
        /// <returns></returns>
        public static IQueryable<TEntity> ApplyFilters<TEntity>(IQueryable<TEntity> query, IEnumerable<Expression<Func<TEntity, bool>>> criterias)
            where TEntity : class
        {
            if (criterias?.Any() == false)
                return query;

            foreach (var filter in criterias)
                query = query.Where(filter);

            return query;
        }

        /// <summary>
        /// Builds a generic expression from a property name
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="propName"></param>
        /// <returns></returns>
        private static Expression<Func<TEntity, object>> BuildExpression<TEntity>(string propName) where TEntity : class
        {
            var parameter = Expression.Parameter(typeof(TEntity), "e");
            var propertyOrField = propName.Split(".")
                .Aggregate((Expression)parameter, Expression.PropertyOrField);
            var unaryExpression = Expression.MakeUnary(ExpressionType.Convert, propertyOrField, typeof(object));

            return Expression.Lambda<Func<TEntity, object>>(unaryExpression, parameter);
        }
    }
}
