using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Tubumu.Modules.Admin.Models
{
    /// <summary>
    /// 公告信息
    /// </summary>
    [Serializable]
    public class Bulletin
    {
        /// <summary>
        /// 公告 Id
        /// </summary>
        public Guid BulletinId { get; set; }

        /// <summary>
        /// 公告标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 公告内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 发布时间
        /// </summary>
        public DateTime? PublishDate { get; set; }

        /// <summary>
        /// 是否显示
        /// </summary>
        public bool IsShow { get; set; }
    }
}
