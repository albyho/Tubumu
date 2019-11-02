using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Tubumu.DataAnnotations;

namespace Tubumu.Modules.Admin.Models.Input
{
    /// <summary>
    /// 用户 Id Input
    /// </summary>
    public class UserIdInput
    {
        /// <summary>
        /// 用户 Id
        /// </summary>
        [Required(ErrorMessage = "请输入用户 Id")]
        public int UserId { get; set; }
    }

    /// <summary>
    /// 用户添加或编辑 Input
    /// </summary>
    public abstract class UserInput
    {
        /// <summary>
        /// 主要分组
        /// </summary>
        [Required(ErrorMessage = "请选择主要分组")]
        [Guid(ErrorMessage = "主要分组不正确")]
        [DisplayName("主要分组")]
        public Guid GroupId { get; set; }

        /// <summary>
        /// 主要角色
        /// </summary>
        [DisplayName("主要角色")]
        public Guid? RoleId { get; set; }

        /// <summary>
        /// 用户状态: 1、待审 2、待审 3、待删
        /// </summary>
        [Required(ErrorMessage = "请选择用户状态")]
        //[Range(1, 3, ErrorMessage = "用户状态不正确")]
        [DisplayName("用户状态")]
        public UserStatus Status { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        [Required(ErrorMessage = "用户名不能为空")]
        [Remote("Admin.Membership.User.UserVerifyExistsUsername", ErrorMessage = "用户名已被使用", AdditionalFields = "UserId")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "用户名请保持在2-20个字符之间")]
        //[SlugWithPrefix("com",ErrorMessage = "用户名只能包含字母、数字、_和-，并以com开头")]
        [Slug(ErrorMessage = "用户名只能包含以字母开头的字母、数字、_和-")]
        [DisplayName("用户名")]
        public string Username { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        //[Required(ErrorMessage = "昵称不能为空")]
        [StringLength(20, ErrorMessage = "昵称请保持在20个字符以内")]
        [SlugWithChinese(ErrorMessage = "昵称只能包含中文字母、数字、_和-")]
        [DisplayName("昵称")]
        public string DisplayName { get; set; }

        /// <summary>
        /// 真实名称
        /// </summary>
        [StringLength(100, MinimumLength = 0, ErrorMessage = "真实名称请保持在20个字符之内")]
        [DisplayName("真实名称")]
        public string RealName { get; set; }

        /// <summary>
        /// 真实名称是否已经验证
        /// </summary>
        [DisplayName("真实名称已验证")]
        public bool RealNameIsValid { get; set; }

        /// <summary>
        /// 邮箱地址
        /// </summary>
        //[Required(ErrorMessage = "邮箱地址不能为空")]
        [Remote("Admin.Membership.User.UserVerifyExistsEmail", ErrorMessage = "邮箱地址已被使用", AdditionalFields = "UserId")]
        [StringLength(100, MinimumLength = 0, ErrorMessage = "邮箱地址请保持在100个字符之内")]
        [Email(ErrorMessage = "邮箱地址格式不正确")]
        [DataType(DataType.EmailAddress, ErrorMessage = "邮箱地址格式不正确")]
        [DisplayName("邮箱地址")]
        public string Email { get; set; }

        /// <summary>
        /// 邮箱是否已经验证
        /// </summary>
        [DisplayName("邮箱已验证")]
        public bool EmailIsValid { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        //[Required(ErrorMessage = "手机号码不能为空")]
        //[StringLength(100, MinimumLength = 0, ErrorMessage = "手机号码请保持在100个字符之内")]
        [Remote("Admin.Membership.User.UserVerifyExistsMobile", ErrorMessage = "手机号码已被使用", AdditionalFields = "UserId")]
        [ChineseMobile(ErrorMessage = "手机号码格式不正确")]
        [DisplayName("手机号码")]
        public string Mobile { get; set; }

        /// <summary>
        /// 手机号码是否已经验证
        /// </summary>
        [DisplayName("号码已验证")]
        public bool MobileIsValid { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [StringLength(4000, ErrorMessage = "描述请保持在4000个字符之间")]
        [DisplayName("描述")]
        public string Description { get; set; }

        /// <summary>
        /// 登录密码
        /// </summary>
        //[Required(ErrorMessage = "登录密码不能为空")]
        [StringLength(32, MinimumLength = 4, ErrorMessage = "登录密码请保持在4-32个字符之间")]
        [DataType(DataType.Password)]
        [DisplayName("登录密码")]
        public virtual string Password { get; set; }

        /// <summary>
        /// 确认密码
        /// </summary>
        //[Required(ErrorMessage = "确认密码不能为空")]
        [StringLength(32, MinimumLength = 4, ErrorMessage = "确认密码请保持在4-32个字符之间")]
        [Tubumu.DataAnnotations.Compare("Password", ValidationCompareOperator.Equal, ValidationDataType.String, ErrorMessage = "请确认两次输入的密码一致")]
        [DataType(DataType.Password)]
        [DisplayName("确认密码")]
        public virtual string PasswordConfirm { get; set; }

        /// <summary>
        /// 是否是开发人员
        /// </summary>
        [DisplayName("是否是开发人员")]
        public bool IsDeveloper { get; set; }

        /// <summary>
        /// 是否是测试人员
        /// </summary>
        [DisplayName("是否是测试人员")]
        public bool IsTester { get; set; }

        /// <summary>
        /// 附件分组 Id
        /// </summary>
        public IEnumerable<Guid> GroupIds { get; set; }

        /// <summary>
        /// 附加角色 Id
        /// </summary>
        public IEnumerable<Guid> RoleIds { get; set; }

        /// <summary>
        /// 拥有权限 Id
        /// </summary>
        public IEnumerable<Guid> PermissionIds { get; set; }
    }
}
