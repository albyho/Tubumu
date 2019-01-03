using System;
using Newtonsoft.Json;

namespace Tubumu.Modules.Framework.Models
{
    /// <summary>
    /// 排序信息
    /// </summary>
    public class SortInfo
    {
        /// <summary>
        /// 排序方向
        /// </summary>
        [JsonProperty(PropertyName = "sortDir")]
        public SortDir SortDir { get; set; }

        /// <summary>
        /// 排序字段
        /// </summary>
        [JsonProperty(PropertyName = "sort")]
        public String Sort { get; set; }
    }

    /// <summary>
    /// 排序方向
    /// </summary>
    public enum SortDir
    {
        /// <summary>
        /// 正序
        /// </summary>
        ASC,

        /// <summary>
        /// 倒序
        /// </summary>
        DESC
    }
}
