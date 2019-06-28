using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Tubumu.Modules.Admin.Models.Input
{
    /// <summary>
    /// 用户操作日志 Input
    /// </summary>
    public class UserActionLogInput
    {
        /// <summary>
        /// 用户 Id
        /// </summary>
        [Required(ErrorMessage = "请输入用户 Id")]
        [Range(1, Int32.MaxValue, ErrorMessage = "请输入合法的用户 Id")]
        public int UserId { get; set; }

        /// <summary>
        /// 操作类型 Id: 1、登录 2、注销 3、App 进入前台 4、 App 进入后台
        /// </summary>
        [Required(ErrorMessage = "请输入操作类型 Id")]
        [Range(1, 4, ErrorMessage = "请输入合法的操作类型 Id (1 - 4)")]
        public int ActionTypeId { get; set; }

        /// <summary>
        /// 客户端类型 Id: 1、PC 2、Web 3、Android 4、iOS
        /// </summary>
        [Range(1, 4, ErrorMessage = "请输入合法的客户端类型 Id (1 - 4)")]
        public int? ClientTypeId { get; set; }

        /// <summary>
        /// 客户端代理
        /// </summary>
        [StringLength(100, ErrorMessage = "客户端代理请保持在 100 个字符以内")]
        public string ClientAgent { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(100, ErrorMessage = "备注请保持在 100 个字符以内")]
        public string Remark { get; set; }
    }
}
