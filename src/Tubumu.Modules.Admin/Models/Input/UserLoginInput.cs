using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Tubumu.Modules.Framework.ModelValidation.Attributes;

namespace Tubumu.Modules.Admin.Models.Input
{
    /// <summary>
    /// 账号 + 密码 + 验证码 登录 Input
    /// </summary>
    public class AccountPasswordValidationCodeLoginInput
    {
        /// <summary>
        /// 账号
        /// </summary>
        [Required(ErrorMessage = "请输入账号")]
        [SlugWithMobileEmail(ErrorMessage = "请输入合法的账号")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "账号请保持在2-20个字符之间")]
        [DisplayName("账号")]
        public string Account { get; set; }

        /// <summary>
        /// 密码
        /// <remark>客户端请进行 MD5 加密(小写)</remark>
        /// </summary>
        [Required(ErrorMessage = "请输入密码")]
        [StringLength(32, MinimumLength = 6, ErrorMessage = "密码请保持在6-32个字符之间")]
        [DataType(DataType.Password)]
        [DisplayName("密码")]
        public string Password { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        [Required(ErrorMessage = "验证码不能为空")]
        [DisplayName("验证码")]
        public string ValidationCode { get; set; }
    }

    /// <summary>
    /// 账号 + 密码 登录 Input
    /// </summary>
    public class AccountPasswordLoginInput
    {
        /// <summary>
        /// 账号
        /// </summary>
        [Required(ErrorMessage = "请输入账号")]
        [SlugWithMobileEmail(ErrorMessage = "请输入合法的账号")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "账号请保持在2-20个字符之间")]
        [DisplayName("账号")]
        public string Account { get; set; }

        /// <summary>
        /// 密码
        /// <remark>客户端请进行 MD5 加密(小写)</remark>
        /// </summary>
        [Required(ErrorMessage = "请输入密码")]
        [StringLength(32, MinimumLength = 6, ErrorMessage = "密码请保持在6-32个字符之间")]
        [DataType(DataType.Password)]
        [DisplayName("密码")]
        public string Password { get; set; }
    }

    /// <summary>
    /// 手机号 + 验证码 登录 Input
    /// </summary>
    public class MobileValidationCodeLoginInput
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
    }

    /// <summary>
    /// 手机号 + 密码 登录 Input
    /// </summary>
    public class MobilePasswordLoginInput
    {
        /// <summary>
        /// 手机号
        /// </summary>
        [Required(ErrorMessage = "请输入手机号")]
        [ChineseMobile(ErrorMessage = "请输入合法的手机号")]
        [DisplayName("手机号")]
        public string Mobile { get; set; }

        /// <summary>
        /// 密码
        /// <remark>客户端请进行 MD5 加密(小写)</remark>
        /// </summary>
        [Required(ErrorMessage = "请输入密码")]
        [StringLength(32, MinimumLength = 6, ErrorMessage = "密码请保持在6-32个字符之间")]
        [DataType(DataType.Password)]
        [DisplayName("密码")]
        public string Password { get; set; }
    }
}
