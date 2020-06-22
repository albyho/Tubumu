using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Tubumu.DataAnnotations;

namespace Tubumu.Modules.Admin.Models.Input
{
    /// <summary>
    /// 客户端类型 Input
    /// </summary>
    public class ClientTypeInput
    {
        /// <summary>
        /// ClientTypeId: 1、PC 2、Web 3、Android 4、iOS 5、可扩充
        /// </summary>
        [Range(1, Int32.MaxValue, ErrorMessage = "请输入合法的客户端类型 Id (大于0)")]
        public int? ClientTypeId { get; set; }

        /// <summary>
        /// ClientAgent: 简单描述客户端软件名称版本等信息
        /// </summary>
        [StringLength(100, ErrorMessage = "客户端代理请保持在 100 个字符以内")]
        public string ClientAgent { get; set; }
    }

    /// <summary>
    /// 账号 + 密码 + 验证码 登录 Input
    /// </summary>
    public class AccountPasswordValidationCodeLoginInput : ClientTypeInput
    {
        /// <summary>
        /// 账号
        /// </summary>
        [Required(ErrorMessage = "请输入账号")]
        [SlugWithMobileEmail(ErrorMessage = "请输入合法的账号")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "账号请保持在 2-20 个字符之间")]
        [DisplayName("账号")]
        public string Account { get; set; }

        /// <summary>
        /// 密码(客户端请进行 MD5 加密(小写))
        /// </summary>
        [Required(ErrorMessage = "请输入密码")]
        [StringLength(32, MinimumLength = 8, ErrorMessage = "密码请保持在 8-32 个字符之间")]
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
    public class AccountPasswordLoginInput : ClientTypeInput
    {
        /// <summary>
        /// 账号
        /// </summary>
        [Required(ErrorMessage = "请输入账号")]
        [SlugWithMobileEmail(ErrorMessage = "请输入合法的账号")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "账号请保持在 8-20 个字符之间")]
        [DisplayName("账号")]
        public string Account { get; set; }

        /// <summary>
        /// 密码(客户端请进行 MD5 加密(小写))
        /// </summary>
        [Required(ErrorMessage = "请输入密码")]
        [StringLength(32, MinimumLength = 8, ErrorMessage = "密码请保持在 8-32 个字符之间")]
        [DataType(DataType.Password)]
        [DisplayName("密码")]
        public string Password { get; set; }
    }

    /// <summary>
    /// 手机号 + 验证码 登录 Input
    /// </summary>
    public class MobileValidationCodeLoginInput : ClientTypeInput
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
    public class MobilePasswordLoginInput : ClientTypeInput
    {
        /// <summary>
        /// 手机号
        /// </summary>
        [Required(ErrorMessage = "请输入手机号")]
        [ChineseMobile(ErrorMessage = "请输入合法的手机号")]
        [DisplayName("手机号")]
        public string Mobile { get; set; }

        /// <summary>
        /// 密码(客户端请进行 MD5 加密(小写))
        /// </summary>
        [Required(ErrorMessage = "请输入密码")]
        [StringLength(32, MinimumLength = 8, ErrorMessage = "密码请保持在 8-32 个字符之间")]
        [DataType(DataType.Password)]
        [DisplayName("密码")]
        public string Password { get; set; }
    }
}
