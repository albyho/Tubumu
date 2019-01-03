using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Tubumu.Modules.Framework.ModelValidation.Attributes;


namespace Tubumu.Modules.Admin.Models.Input
{
    /// <summary>
    /// 获取手机验证码 Input
    /// </summary>
    public class GetMobileValidationCodeInput
    {
        /// <summary>
        /// 手机号
        /// </summary>
        [Required(ErrorMessage = "请输入手机号码")]
        [ChineseMobile(ErrorMessage = "请输入正确的手机号码")]
        [DisplayName("手机号码")]
        public string Mobile { get; set; }

        /// <summary>
        /// 验证类型
        /// <remark>Type: 0 注册 1 重置密码 2 更换手机号 3 短信登录(如果没注册，则自动注册) 4 绑定手机号</remark>
        /// </summary>
        [DisplayName("验证类型")]
        public MobileValidationCodeType Type { get; set; }
    }

    public class VerifyMobileValidationCodeInput
    {
        [Required(ErrorMessage = "请输入手机号码")]
        [ChineseMobile(ErrorMessage = "请输入正确的手机号码")]
        [DisplayName("手机号码")]
        public string Mobile { get; set; }

        /// <summary>
        /// 验证类型
        /// <remark>Type: 0 注册 1 重置密码 2 更换手机号 3 短信登录(如果没注册，则自动注册) 4 绑定手机号</remark>
        /// </summary>
        [DisplayName("验证类型")]
        public MobileValidationCodeType Type { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        [Required(ErrorMessage = "请输入短信验证码")]
        [StringLength(10, ErrorMessage = "短信验证码最多支持10位")]
        public string ValidationCode { get; set; }
    }
}
