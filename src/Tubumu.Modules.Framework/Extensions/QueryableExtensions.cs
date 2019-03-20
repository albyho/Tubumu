using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tubumu.Core.Extensions;
using Tubumu.Modules.Framework.Models;

namespace Tubumu.Modules.Framework.Extensions
{
    /// <summary>
    /// QueryableExtensions
    /// </summary>
    public static class QueryableExtensions
    {
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
