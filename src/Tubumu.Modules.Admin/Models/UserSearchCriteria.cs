using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using Tubumu.Modules.Framework.Models;

namespace Tubumu.Modules.Admin.Models
{
    /// <summary>
    /// 用户搜索条件
    /// </summary>
    public class UserSearchCriteria
    {
        /// <summary>
        /// 分页信息
        /// </summary>
        public PagingInfo PagingInfo { get; set; }

        /// <summary>
        /// 分支 Id
        /// </summary>
        public List<Guid> GroupIds { get; set; }

        /// <summary>
        /// 用户状态
        /// </summary>
        public UserStatus? Status { get; set; }

        /// <summary>
        /// 创建开始时间
        /// </summary>
        public DateTime? CreationDateBegin { get; set; }

        /// <summary>
        /// 创建结束时间
        /// </summary>
        public DateTime? CreationDateEnd { get; set; }

        /// <summary>
        /// 关键字
        /// </summary>
        [StringLength(100, ErrorMessage = "搜索关键字长度请保持在100个字符以内")]
        public string Keyword { get; set; }
    }
}
