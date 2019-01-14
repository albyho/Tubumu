using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Tubumu.Modules.Framework.ModelValidation.Attributes;

namespace Tubumu.Modules.Admin.Models.Input
{
    /// <summary>
    /// 用户编辑 Input
    /// <remark>编辑用户密码要么都为空，要么必须输入合法的密码</remark>
    /// </summary>
    public class UserInputEdit : UserInput
    {
        /// <summary>
        /// 用户 Id
        /// </summary>
        /// <remarks>添加时为 null；编辑时未非 null</remarks>
        [DisplayName("用户 Id")]
        [Range(1,Int32.MaxValue,ErrorMessage = "请选择存在的用户")]
        public int UserId { get; set; }
    }

    /// <summary>
    /// 用户修改密码 Input
    /// </summary>
    public class UserEditPasswordInput
    {
        /// <summary>
        /// 用户 Id
        /// </summary>
        [DisplayName("用户 Id")]
        [Range(1,Int32.MaxValue,ErrorMessage = "请选择存在的用户")]
        public int UserId { get; set; }

        /// <summary>
        /// 登录密码
        /// </summary>
        [Required(ErrorMessage = "登录密码不能为空")]
        [StringLength(32, MinimumLength = 6, ErrorMessage = "登录密码请保持在 6-32 个字符之间")]
        [DataType(DataType.Password)]
        [DisplayName("登录密码")]
        public string Password { get; set; }

        /// <summary>
        /// 确认密码
        /// </summary>
        [Required(ErrorMessage = "确认密码不能为空")]
        [StringLength(32, MinimumLength = 6, ErrorMessage = "确认密码请保持在 6-32 个字符之间")]
        [Tubumu.Modules.Framework.ModelValidation.Attributes.Compare("Password", ValidationCompareOperator.Equal, ValidationDataType.String, ErrorMessage = "请确认两次输入的密码一致")]
        [DataType(DataType.Password)]
        [DisplayName("确认密码")]
        public string PasswordConfirm { get; set; }

    }

    /// <summary>
    /// 用户修改分组 Input
    /// </summary>
    public class UserEditGroupInput
    {
        /// <summary>
        /// 用户 Id"
        /// </summary>
        [DisplayName("用户 Id")]
        [Range(1, Int32.MaxValue, ErrorMessage = "请选择存在的用户")]
        public int UserId { get; set; }

        /// <summary>
        /// 分组 Id
        /// </summary>
        [DisplayName("分组 Id")]
        [Range(1, Int32.MaxValue, ErrorMessage = "请选择用户分组")]
        public int GroupId { get; set; }
    }
}
