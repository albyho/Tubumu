using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Tubumu.Modules.Framework.ModelValidation.Attributes;

namespace Tubumu.Modules.Admin.Models.Input
{
    /// <summary>
    /// 验证用户名是否已经被使用 Input
    /// </summary>
    public class ValidateUsernameExistsInput
    {
        /// <summary>
        /// 用户 Id
        /// </summary>
        [Range(1, Int32.MaxValue, ErrorMessage = "请输入合法的用户 Id")]
        public int? UserId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        [Required(ErrorMessage = "用户名不能为空")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "用户名请保持在2-20个字符之间")]
        //[SlugWithPrefix("com",ErrorMessage = "用户名只能包含字母、数字、_和-，并以com开头")]
        [Slug(ErrorMessage = "用户名只能包含以字母开头的字母、数字、_和-")]
        [DisplayName("用户名")]
        public string Username { get; set; }
    }

    /// <summary>
    /// 验证手机号是否已经被使用 Input
    /// </summary>
    public class ValidateMobileExistsInput
    {
        /// <summary>
        /// 用户 Id
        /// </summary>
        [Range(1, Int32.MaxValue, ErrorMessage = "请输入合法的用户 Id")]
        public int? UserId { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        [Required(ErrorMessage = "手机号不能为空")]
        [ChineseMobile(ErrorMessage = "手机号码格式不正确")]
        public string Mobile { get; set; }
    }

    /// <summary>
    /// 验证邮箱是否已经被使用 Input
    /// </summary>
    public class ValidateEmailExistsInput
    {
        /// <summary>
        /// 用户 Id
        /// </summary>
        [Range(1, Int32.MaxValue, ErrorMessage = "请输入合法的用户 Id")]
        public int? UserId { get; set; }

        /// <summary>
        /// 邮箱地址
        /// </summary>
        [Required(ErrorMessage = "邮箱地址不能为空")]
        [StringLength(100, MinimumLength = 0, ErrorMessage = "邮箱地址请保持在100个字符之内")]
        [Email(ErrorMessage = "邮箱地址格式不正确")]
        public string Email { get; set; }
    }
}
