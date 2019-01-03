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
    }

    /// <summary>
    /// List ApiResult
    /// </summary>
    public class ApiListResult : ApiResult
    {
        /// <summary>
        /// Lists
        /// </summary>
        [JsonProperty(PropertyName = "list", NullValueHandling = NullValueHandling.Ignore)]
        public object List { get; set; }
    }

    /// <summary>
    /// Page ApiResult
    /// </summary>
    public class ApiPageResult : ApiResult
    {
        /// <summary>
        /// Pages
        /// </summary>
        [JsonProperty(PropertyName = "page", NullValueHandling = NullValueHandling.Ignore)]
        public object Page { get; set; }
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
        public List<TreeNode> Tree { get; set; }
    }

    /// <summary>
    /// Item ApiResult
    /// </summary>
    public class ApiItemResult : ApiResult
    {

        /// <summary>
        /// Item
        /// </summary>
        [JsonProperty(PropertyName = "item", NullValueHandling = NullValueHandling.Ignore)]
        public object Item { get; set; }
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
        public List<TreeNode> Children { get; set; }
    }

}
