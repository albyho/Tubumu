﻿using System.ComponentModel.DataAnnotations;
using Tubumu.DataAnnotations;

namespace Tubumu.Modules.Admin.Models.Input
{
    /// <summary>
    /// 用户添加 Input
    /// </summary>
    public class UserInputAdd : UserInput
    {
        /// <summary>
        /// 登录密码(客户端请进行 MD5 加密(小写))
        /// </summary>
        [Required(ErrorMessage = "登录密码不能为空")]
        [StringLength(32, MinimumLength = 8, ErrorMessage = "登录密码请保持在 8-32 个字符之间")]
        public override string Password { get; set; }

        /// <summary>
        /// 确认密码(客户端请进行 MD5 加密(小写))
        /// </summary>
        [Required(ErrorMessage = "确认密码不能为空")]
        public override string PasswordConfirm { get; set; }
    }
}
