using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Tubumu.Modules.Framework.Models
{
    /// <summary>
    /// ApiResultCode
    /// </summary>
    public enum ApiResultCode
    {
        /// <summary>
        /// 成功
        /// </summary>
        Success = 200,

        /// <summary>
        /// 默认错误码
        /// </summary>
        DefaultError = 400
    }

    /// <summary>
    /// ApiResult
    /// </summary>
    public class ApiResult
    {
        /// <summary>
        /// 错误码
        /// <remarks>code: 200 表示成功 其他如无特别提示表示失败</remarks>
        /// </summary>
        [JsonProperty(PropertyName = "code")]
        public int Code { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }
    }

    /// <summary>
    /// Url ApiResult
    /// </summary>
    public class ApiResultUrl : ApiResult
    {
        /// <summary>
        /// Url
        /// </summary>
        [JsonProperty(PropertyName = "url", NullValueHandling = NullValueHandling.Ignore)]
        public string Url { get; set; }
    }

    /// <summary>
    /// Data ApiResult
    /// </summary>
    public class ApiResultData<T> : ApiResult
    {
        /// <summary>
        /// Data
        /// </summary>
        [JsonProperty(PropertyName = "data", NullValueHandling = NullValueHandling.Ignore)]
        public T Data { get; set; }
    }

    /// <summary>
    /// Data and Url ApiResult
    /// </summary>
    public class ApiResultDataUrl<T> : ApiResult
    {
        /// <summary>
        /// Data
        /// </summary>
        [JsonProperty(PropertyName = "data", NullValueHandling = NullValueHandling.Ignore)]
        public T Data { get; set; }

        /// <summary>
        /// Url
        /// </summary>
        [JsonProperty(PropertyName = "url", NullValueHandling = NullValueHandling.Ignore)]
        public string Url { get; set; }
    }

    /// <summary>
    /// 树节点
    /// </summary>
    [Serializable]
    public class TreeNode
    {
        /// <summary>
        /// Id
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// 子节点
        /// </summary>
        [JsonProperty(PropertyName = "children", NullValueHandling = NullValueHandling.Ignore)]
        public ICollection<TreeNode> Children { get; set; }
    }
}
