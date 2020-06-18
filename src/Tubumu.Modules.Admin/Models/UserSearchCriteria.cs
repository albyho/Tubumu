using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Tubumu.Modules.Core.Models;

namespace Tubumu.Modules.Admin.Models
{
    /// <summary>
    /// 用户搜索条件
    /// </summary>
    public class UserPageSearchCriteria
    {
        /// <summary>
        /// 分页信息
        /// </summary>
        [Required(ErrorMessage = "请输入分页信息")]
        public PagingInfo PagingInfo { get; set; }

        /// <summary>
        /// 分支 Id
        /// </summary>
        public List<Guid> GroupIds { get; set; }

        /// <summary>
        /// 用户状态
        /// </summary>
        public UserStatus[] Statuses { get; set; }

        /// <summary>
        /// 创建时间起始
        /// </summary>
        public DateTime? CreationTimeBegin { get; set; }

        /// <summary>
        /// 创建时间结束
        /// </summary>
        public DateTime? CreationTimeEnd { get; set; }

        /// <summary>
        /// 关键字
        /// </summary>
        [StringLength(100, ErrorMessage = "搜索关键字长度请保持在100个字符以内")]
        public string Keyword { get; set; }
    }
}
