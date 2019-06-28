using System.ComponentModel.DataAnnotations;

namespace Tubumu.Modules.Admin.Models.Input
{
    /// <summary>
    /// 微信小程序登录 Input
    /// </summary>
    public class WeixinAppLoginInput : ClientTypeInput
    {
        /// <summary>
        /// 微信登录 Code
        /// 用户换取 access_token 的 code ，仅在 ErrCode 为 0 时有效
        /// </summary>
        [Required(ErrorMessage = "微信登录 Code")]
        public string Code { get; set; }
    }
}
