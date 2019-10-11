using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

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
        public int Code { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
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
        public T Data { get; set; }

        /// <summary>
        /// Url
        /// </summary>
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
        public Guid Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 子节点
        /// </summary>
        public ICollection<TreeNode> Children { get; set; }
    }
}
