using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tubumu.Modules.Framework.Models;

namespace Tubumu.Modules.Framework.Extensions
{
    /// <summary>
    /// QueryableExtensions
    /// </summary>
    public static class QueryableExtensions
    {
        /// <summary>
        /// WhereIn
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="query"></param>
        /// <param name="selector"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static IQueryable<TEntity> WhereIn<TEntity, TValue>
          (
            this IQueryable<TEntity> query,
            Expression<Func<TEntity, TValue>> selector,
            IEnumerable<TValue> values
          )
        {
            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            if (!values.Any()) return query;

            ParameterExpression p = selector.Parameters.Single();

            IEnumerable<Expression> equals = values.Select(value => (Expression)Expression.Equal(selector.Body, Expression.Constant(value, typeof(TValue))));

            Expression body = equals.Aggregate((accumulate, equal) => Expression.Or(accumulate, equal));

            return query.Where(Expression.Lambda<Func<TEntity, bool>>(body, p));
        }

        /// <summary>
        /// WhereIn
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="query"></param>
        /// <param name="selector"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static IQueryable<TEntity> WhereIn<TEntity, TValue>
          (
            this IQueryable<TEntity> query,
            Expression<Func<TEntity, TValue>> selector,
            params TValue[] values
          )
        {
            return WhereIn(query, selector, (IEnumerable<TValue>)values);
        }

        /// <summary>
        /// LeftJoin
        /// </summary>
        /// <typeparam name="TOuter"></typeparam>
        /// <typeparam name="TInner"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="outer"></param>
        /// <param name="inner"></param>
        /// <param name="outerKeySelector"></param>
        /// <param name="innerKeySelector"></param>
        /// <param name="resultSelector"></param>
        /// <returns></returns>
        public static IQueryable<TResult> LeftJoin<TOuter, TInner, TKey, TResult>(
            this IQueryable<TOuter> outer,
            IQueryable<TInner> inner,
            Expression<Func<TOuter, TKey>> outerKeySelector,
            Expression<Func<TInner, TKey>> innerKeySelector,
            Expression<Func<TOuter, TInner, TResult>> resultSelector)
        {
            MethodInfo groupJoin = typeof(Queryable).GetMethods()
                                                     .Single(m => m.ToString() == "System.Linq.IQueryable`1[TResult] GroupJoin[TOuter,TInner,TKey,TResult](System.Linq.IQueryable`1[TOuter], System.Collections.Generic.IEnumerable`1[TInner], System.Linq.Expressions.Expression`1[System.Func`2[TOuter,TKey]], System.Linq.Expressions.Expression`1[System.Func`2[TInner,TKey]], System.Linq.Expressions.Expression`1[System.Func`3[TOuter,System.Collections.Generic.IEnumerable`1[TInner],TResult]])")
                                                     .MakeGenericMethod(typeof(TOuter), typeof(TInner), typeof(TKey), typeof(LeftJoinIntermediate<TOuter, TInner>));
            MethodInfo selectMany = typeof(Queryable).GetMethods()
                                                      .Single(m => m.ToString() == "System.Linq.IQueryable`1[TResult] SelectMany[TSource,TCollection,TResult](System.Linq.IQueryable`1[TSource], System.Linq.Expressions.Expression`1[System.Func`2[TSource,System.Collections.Generic.IEnumerable`1[TCollection]]], System.Linq.Expressions.Expression`1[System.Func`3[TSource,TCollection,TResult]])")
                                                      .MakeGenericMethod(typeof(LeftJoinIntermediate<TOuter, TInner>), typeof(TInner), typeof(TResult));

            var groupJoinResultSelector = (Expression<Func<TOuter, IEnumerable<TInner>, LeftJoinIntermediate<TOuter, TInner>>>)
                                          ((oneOuter, manyInners) => new LeftJoinIntermediate<TOuter, TInner> { OneOuter = oneOuter, ManyInners = manyInners });

            MethodCallExpression exprGroupJoin = Expression.Call(groupJoin, outer.Expression, inner.Expression, outerKeySelector, innerKeySelector, groupJoinResultSelector);

            var selectManyCollectionSelector = (Expression<Func<LeftJoinIntermediate<TOuter, TInner>, IEnumerable<TInner>>>)
                                               (t => t.ManyInners.DefaultIfEmpty());

            ParameterExpression paramUser = resultSelector.Parameters.First();

            ParameterExpression paramNew = Expression.Parameter(typeof(LeftJoinIntermediate<TOuter, TInner>), "t");
            MemberExpression propExpr = Expression.Property(paramNew, "OneOuter");

            LambdaExpression selectManyResultSelector = Expression.Lambda(new Replacer(paramUser, propExpr).Visit(resultSelector.Body) ?? throw new InvalidOperationException(), paramNew, resultSelector.Parameters.Skip(1).First());

            MethodCallExpression exprSelectMany = Expression.Call(selectMany, exprGroupJoin, selectManyCollectionSelector, selectManyResultSelector);

            return outer.Provider.CreateQuery<TResult>(exprSelectMany);
        }

        private class LeftJoinIntermediate<TOuter, TInner>
        {
            public TOuter OneOuter { get; set; }
            public IEnumerable<TInner> ManyInners { get; set; }
        }

        private class Replacer : ExpressionVisitor
        {
            private readonly ParameterExpression _oldParam;
            private readonly Expression _replacement;

            public Replacer(ParameterExpression oldParam, Expression replacement)
            {
                _oldParam = oldParam;
                _replacement = replacement;
            }

            public override Expression Visit(Expression exp)
            {
                if (exp == _oldParam)
                {
                    return _replacement;
                }

                return base.Visit(exp);
            }
        }

        /// <summary>
        /// Order
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="propertyName"></param>
        /// <param name="descending"></param>
        /// <param name="anotherLevel"></param>
        /// <returns></returns>
        public static IOrderedQueryable<T> Order<T>(this IQueryable<T> source, string propertyName, bool descending, bool anotherLevel)
        {
            ParameterExpression param = Expression.Parameter(typeof(T), String.Empty); // I don't care about some naming
            MemberExpression property = Expression.PropertyOrField(param, propertyName);
            LambdaExpression sort = Expression.Lambda(property, param);
            MethodCallExpression call = Expression.Call(
                typeof(Queryable),
                (!anotherLevel ? "OrderBy" : "ThenBy") + (descending ? "Descending" : String.Empty),
                new[] { typeof(T), property.Type },
                source.Expression,
                Expression.Quote(sort));
            return (IOrderedQueryable<T>)source.Provider.CreateQuery<T>(call);
        }

        /// <summary>
        /// Order
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="propertyName"></param>
        /// <param name="descending"></param>
        /// <returns></returns>
        public static IOrderedQueryable<T> Order<T>(this IQueryable<T> source, string propertyName, bool descending)
        {
            return Order(source, propertyName, descending, false);
        }

        /// <summary>
        /// OrderBy
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string propertyName)
        {
            return Order(source, propertyName, false, false);
        }

        /// <summary>
        /// OrderByDescending
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> source, string propertyName)
        {
            return Order(source, propertyName, true, false);
        }

        /// <summary>
        /// ThenBy
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> source, string propertyName)
        {
            return Order(source, propertyName, false, true);
        }

        /// <summary>
        /// ThenByDescending
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static IOrderedQueryable<T> ThenByDescending<T>(this IOrderedQueryable<T> source, string propertyName)
        {
            return Order(source, propertyName, true, true);
        }

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <typeparam name="T">泛型类型参数</typeparam>
        /// <param name="sourceQuery">查询集</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="topQuery">跳过记录集</param>
        /// <returns></returns>
        public static async Task<Page<T>> GetPageAsync<T>(this IQueryable<T> sourceQuery, PagingInfo pagingInfo, ICollection<T> topQuery = null) where T : class
        {
            if (sourceQuery == null)
            {
                throw new ArgumentNullException(nameof(sourceQuery));
            }

            sourceQuery = sourceQuery.AsNoTracking();

            var page = new Page<T>();

            // 跳过记录集无记录
            if (topQuery.IsNullOrEmpty())
            {
                page.List = await sourceQuery.Skip(pagingInfo.PageIndex * pagingInfo.PageSize).Take(pagingInfo.PageSize).ToListAsync();
                if (!pagingInfo.IsExcludeMetaData)
                {
                    page.TotalItemCount = await sourceQuery.CountAsync();
                    page.TotalPageCount = (int)Math.Ceiling(page.TotalItemCount / (double)pagingInfo.PageSize);
                }
            }
            else
            {
                // 跳过的记录数
                int topItemCount = topQuery.Count;
                // 跳过的页数，比如一页显示10条，跳过4条、14条或24条，则跳过的页数为0、1或2
                int skipPage = (int)Math.Floor((double)topItemCount / pagingInfo.PageSize);

                // 如果目标页数在跳过的页数范围内，直接从topItems获取
                if (skipPage > pagingInfo.PageIndex)
                {
                    page.List = topQuery.Skip(pagingInfo.PageIndex * pagingInfo.PageSize).Take(pagingInfo.PageSize).ToList();
                    if (!pagingInfo.IsExcludeMetaData)
                    {
                        page.TotalItemCount = await sourceQuery.CountAsync() + topItemCount;
                        page.TotalPageCount = (int)Math.Ceiling(page.TotalItemCount / (double)pagingInfo.PageSize);
                    }
                }
                else
                {
                    int topSkipCount = skipPage * pagingInfo.PageSize;
                    int topTakeCount = topItemCount % pagingInfo.PageSize;
                    var topItems = topQuery.Skip(topSkipCount).Take(topTakeCount);

                    int sourceSkipCount = (pagingInfo.PageIndex - skipPage) * pagingInfo.PageSize;
                    int sourceTakeCount = pagingInfo.PageSize - topTakeCount;
                    var sourceItems = await sourceQuery.Skip(sourceSkipCount).Take(sourceTakeCount).ToListAsync();

                    page.List = topItems.Concat(sourceItems).ToList();
                    if (!pagingInfo.IsExcludeMetaData)
                    {
                        // 查询集记录数
                        int sourceItemCount = await sourceQuery.CountAsync();
                        page.TotalItemCount = sourceItemCount + topItemCount;
                        page.TotalPageCount = (int)Math.Ceiling(page.TotalItemCount / (double)pagingInfo.PageSize);
                    }
                }
            }

            return page;
        }
    }
}
