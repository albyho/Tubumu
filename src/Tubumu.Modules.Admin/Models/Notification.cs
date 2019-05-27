using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Tubumu.Core.Json;
using Tubumu.Modules.Core.Models;
using Tubumu.Modules.Framework.ModelValidation.Attributes;

namespace Tubumu.Modules.Admin.Models
{
    /// <summary>
    /// 通知信息
    /// </summary>
    public class NotificationBase
    {
        /// <summary>
        /// 通知 Id
        /// </summary>
        [JsonProperty(PropertyName = "notificationId")]
        public int NotificationId { get; set; }

        /// <summary>
        /// 发送自
        /// </summary>
        [JsonProperty(PropertyName = "fromUser")]
        public UserInfoWarpper FromUser { get; set; }

        /// <summary>
        /// 发送至
        /// </summary>
        [JsonProperty(PropertyName = "toUser")]
        [JsonConverter(typeof(NullValueJsonConverter<int>), "UserId", 0)]
        public UserInfoWarpper ToUser { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [JsonProperty(PropertyName = "creationTime")]
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        /// <summary>
        /// Url
        /// </summary>
        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }
    }

    /// <summary>
    /// 通知信息
    /// </summary>
    public class Notification : NotificationBase
    {
        /// <summary>
        /// 消息
        /// </summary>
        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }
    }

    /// <summary>
    /// 通知信息
    /// </summary>
    public class NotificationUser : Notification
    {

        /// <summary>
        /// 读取时间
        /// </summary>
        [JsonProperty(PropertyName = "readTime", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? ReadTime { get; set; }

        /// <summary>
        /// 删除时间
        /// </summary>
        [JsonProperty(PropertyName = "deleteTime", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? DeleteTime { get; set; }
    }

    /// <summary>
    /// 通知 Id Input
    /// </summary>
    public class NotificationIdInput
    {
        /// <summary>
        /// 通知 Id
        /// </summary>
        [Required(ErrorMessage = "请输入通知 Id")]
        public int NotificationId { get; set; }
    }

    /// <summary>
    /// 通知 Id Input
    /// </summary>
    public class NotificationIdListInput
    {
        /// <summary>
        /// 通知Id
        /// </summary>
        [CollectionCountRange(1, Int32.MaxValue, ErrorMessage = "请输入通知 Id 集")]
        public int[] NotificationIds { get; set; }
    }

    /// <summary>
    /// 通知 Input
    /// </summary>
    public class NotificationInput
    {
        /// <summary>
        /// 通知 Id (添加时为 null；编辑时未非 null)
        /// </summary>
        [Range(1, Int32.MaxValue, ErrorMessage = "请输入通知 Id")]
        public int? NotificationId { get; set; }

        /// <summary>
        /// 发送自 (内部赋值)
        /// </summary>
        public int? FromUserId { get; set; }

        /// <summary>
        /// 发送至 ( null 则发送至所有人)
        /// </summary>
        public int? ToUserId { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [Required(ErrorMessage = "请输入通知标题")]
        [StringLength(100, ErrorMessage = "通知标题请保持在100个字符以内")]
        public string Title { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        [Required(ErrorMessage = "请输入通知内容")]
        [StringLength(1000, ErrorMessage = "通知内容请保持在1000个字符以内")]
        public string Message { get; set; }

        /// <summary>
        /// Url
        /// </summary>
        [StringLength(200, ErrorMessage = "URL保持在200个字符以内")]
        public string Url { get; set; }
    }

    /// <summary>
    /// 通知搜索条件
    /// </summary>
    public class NotificationPageSearchCriteria
    {
        /// <summary>
        /// 分页信息
        /// </summary>
        [Required(ErrorMessage = "请输入分页信息")]
        public PagingInfo PagingInfo { get; set; }

        /// <summary>
        /// 是否已读
        /// </summary>
        public bool? IsReaded { get; set; }

        /// <summary>
        /// 发送自
        /// </summary>
        public int? FromUserId { get; set; }

        /// <summary>
        /// 发送至
        /// </summary>
        public int? ToUserId { get; set; }

        /// <summary>
        /// 关键字
        /// </summary>
        public string Keyword { get; set; }

        /// <summary>
        /// 创建时间开始
        /// </summary>
        public DateTime? CreationTimeBegin { get; set; }

        /// <summary>
        /// 创建时间结束
        /// </summary>
        public DateTime? CreationTimeEnd { get; set; }
    }
}
