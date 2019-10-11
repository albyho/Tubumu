using System.Collections.Generic;

namespace Tubumu.Modules.Core.Models
{
    /// <summary>
    /// 分页
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Page<T>
    {
        /// <summary>
        /// 列表
        /// </summary>
        public List<T> List { get; set; }

        /// <summary>
        /// 元素总数
        /// </summary>
        public int TotalItemCount { get; set; }

        /// <summary>
        /// 总分页数
        /// </summary>
        public int TotalPageCount { get; set; }
    }
}
