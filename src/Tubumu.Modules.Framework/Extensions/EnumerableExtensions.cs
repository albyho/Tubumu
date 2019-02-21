using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tubumu.Modules.Framework.Models;

namespace Tubumu.Modules.Framework.Extensions
{
    /// <summary>
    /// EnumerableExtensions
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// 对枚举器的每个元素执行指定的操作
        /// </summary>
        /// <typeparam name="T">枚举器类型参数</typeparam>
        /// <param name="source">枚举器</param>
        /// <param name="action">要对枚举器的每个元素执行的委托</param>
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            if (source == null || action == null)
            {
                return;
            }
            foreach (var item in source)
            {
                action(item);
            }
        }

        /// <summary>
        /// 指示指定的枚举器是否为 null 或没有任何元素
        /// </summary>
        /// <typeparam name="T">枚举器类型参数</typeparam>
        /// <param name="source">要测试的枚举器</param>
        /// <returns>true:枚举器是null或者没有任何元素 false:枚举器不为null并且包含至少一个元素</returns>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> source)
        {
            return source == null || !source.Any();
        }

        /// <summary>
        /// 判断指定的集合是否为 null 或没有任何元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty<T>(this ICollection<T> source)
        {
            return source == null || source.Count == 0;
        }

        /// <summary>
        /// 判断指定的数组是否为 null 或没有任何元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty<T>(this T[] source)
        {
            return source == null || source.Length == 0;
        }

        /// <summary>
        /// 对String型序列的每个元素进行字符串替换操作
        /// </summary>
        /// <param name="source">源序列</param>
        /// <param name="oldValue">查找字符串</param>
        /// <param name="newValue">替换字符串</param>
        /// <returns>新的String型序列</returns>
        public static IEnumerable<string> Replace(IEnumerable<string> source, string oldValue, string newValue)
        {
            return source.Select(format => format.Replace(oldValue, newValue));
        }

        /// <summary>
        /// 将序列转化为ReadOnlyCollection
        /// </summary>
        /// <typeparam name="T">类型参数</typeparam>
        /// <param name="source">源序列</param>
        /// <returns></returns>
        public static IList<T> ToReadOnlyCollection<T>(this IEnumerable<T> source)
        {
            return new ReadOnlyCollection<T>(source.ToList());
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
            if(sourceQuery == null)
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
