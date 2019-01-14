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
        /// </summary>
        /// <remarks>code: 200 表示成功 其他如无特别提示表示失败</remarks>
        [JsonProperty(PropertyName = "code")]
        public int Code { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }

        /// <summary>
        /// Url
        /// </summary>
        [JsonProperty(PropertyName = "url", NullValueHandling = NullValueHandling.Ignore)]
        public string Url { get; set; }
    }

    /// <summary>
    /// Token ApiResult
    /// </summary>
    public class ApiTokenResult : ApiResult
    {
        /// <summary>
        /// Token
        /// </summary>
        [JsonProperty(PropertyName = "token", NullValueHandling = NullValueHandling.Ignore)]
        public string Token { get; set; }

        /// <summary>
        /// RefreshToken
        /// </summary>
        [JsonProperty(PropertyName = "refreshToken", NullValueHandling = NullValueHandling.Ignore)]
        public string RefreshToken { get; set; }
    }

    /// <summary>
    /// List ApiResult
    /// </summary>
    public class ApiListResult<T> : ApiResult
    {
        /// <summary>
        /// Lists
        /// </summary>
        [JsonProperty(PropertyName = "list", NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<T> List { get; set; }
    }

    /// <summary>
    /// Page ApiResult
    /// </summary>
    public class ApiPageResult<T> : ApiResult where T : class
    {
        /// <summary>
        /// Pages
        /// </summary>
        [JsonProperty(PropertyName = "page", NullValueHandling = NullValueHandling.Ignore)]
        public Page<T> Page { get; set; }
    }

    /// <summary>
    /// Tree ApiResult
    /// </summary>
    public class ApiTreeResult : ApiResult
    {
        /// <summary>
        /// Tree
        /// </summary>
        [JsonProperty(PropertyName = "tree")]
        public IEnumerable<TreeNode> Tree { get; set; }
    }

    /// <summary>
    /// Item ApiResult
    /// </summary>
    public class ApiItemResult<T> : ApiResult where T : class
    {

        /// <summary>
        /// Item
        /// </summary>
        [JsonProperty(PropertyName = "item", NullValueHandling = NullValueHandling.Ignore)]
        public T Item { get; set; }
    }

    /// <summary>
    /// Html ApiResult
    /// </summary>
    public class ApiHtmlResult : ApiResult
    {
        /// <summary>
        /// Html
        /// </summary>
        [JsonProperty(PropertyName = "html", NullValueHandling = NullValueHandling.Ignore)]
        public string Html { get; set; }
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
