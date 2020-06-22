using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Tubumu.DataAnnotations;

namespace Tubumu.Modules.Admin.Models.Input
{
    /// <summary>
    /// 通过手机号 + 密码 + 验证码 注册 Input
    /// </summary>
    public class MobilePassswordValidationCodeRegisterInput
    {
        /// <summary>
        /// 手机号
        /// </summary>
        [Required(ErrorMessage = "请输入手机号")]
        [ChineseMobile(ErrorMessage = "请输入合法的手机号")]
        [DisplayName("手机号")]
        public string Mobile { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        [Required(ErrorMessage = "验证码不能为空")]
        [DisplayName("验证码")]
        public string ValidationCode { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Required(ErrorMessage = "请输入密码")]
        [StringLength(32, MinimumLength = 8, ErrorMessage = "密码请保持在 8-32 个字符之间")]
        [DataType(DataType.Password)]
        [DisplayName("密码")]
        public string Password { get; set; }

        /// <summary>
        /// 确认密码
        /// </summary>
        [Required(ErrorMessage = "确认密码不能为空")]
        [StringLength(32, MinimumLength = 8, ErrorMessage = "确认密码请保持在 8-32个字符之间")]
        [Tubumu.DataAnnotations.Compare("Password", ValidationCompareOperator.Equal, ValidationDataType.String, ErrorMessage = "请确认两次输入的密码一致")]
        [DataType(DataType.Password)]
        [DisplayName("确认密码")]
        public string PasswordConfirm { get; set; }
    }
}
