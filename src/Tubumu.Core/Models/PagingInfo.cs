using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Tubumu.Modules.Core.Models
{
    /// <summary>
    /// 分页信息
    /// </summary>
    public class PagingInfo
    {
        /// <summary>
        /// 分页索引（从 0 开始）
        /// </summary>
        [JsonIgnore]
        public int PageIndex => PageNumber >= 1 ? PageNumber - 1 : 0;

        /// <summary>
        /// 页码（从 1 开始）
        /// </summary>
        [Range(1, Int32.MaxValue, ErrorMessage = "请输入 PageNumber")]
        public int PageNumber { get; set; }

        /// <summary>
        /// 每页元素数
        /// </summary>
        [Range(1, Int32.MaxValue, ErrorMessage = "请输入 PageSize")]
        public int PageSize { get; set; }

        /// <summary>
        /// 跳过数量（将少读取跳过的数量）
        /// </summary>
        public int SkipTop { get; set; }

        /// <summary>
        /// 排序信息
        /// </summary>
        public SortInfo SortInfo { get; set; }

        /// <summary>
        /// 排序信息（多个排序字段。优先级低于 SortInfo 属性。）
        /// </summary>
        public SortInfo[] SortInfos { get; set; }

        /// <summary>
        /// 是否排除元数据（如果不是第一页，没有必要重复获取 TotalItemCount 和 TotalPageCount）
        /// </summary>
        public bool IsExcludeMetaData { get; set; }
    }
}
