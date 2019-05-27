using System.Collections.Generic;
using Newtonsoft.Json;

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
        [JsonProperty(PropertyName = "list")]
        public List<T> List { get; set; }

        /// <summary>
        /// 元素总数
        /// </summary>
        [JsonProperty(PropertyName = "totalItemCount")]
        public int TotalItemCount { get; set; }

        /// <summary>
        /// 总分页数
        /// </summary>
        [JsonProperty(PropertyName = "totalPageCount")]
        public int TotalPageCount { get; set; }
    }
}
