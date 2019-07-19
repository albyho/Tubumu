using System.ComponentModel.DataAnnotations;

namespace Tubumu.Modules.Admin.Models.Input
{
    /// <summary>
    /// 网页回调
    /// </summary>
    public class WeixinWebLoginInput : ClientTypeInput
    {
        /// <summary>
        /// 微信登录 Code
        /// </summary>
        [Required(ErrorMessage = "微信登录 Code")]
        public string Code { get; set; }

        /// <summary>
        /// 回传数据
        /// 第三方程序发送时用来标识其请求的唯一性的标志，由第三方程序调用 sendReq 时传入，由微信终端回传，state 字符串长度不能超过 1K
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// 跳转 Url
        /// </summary>
        public string RedirectUrl { get; set; }
    }
}
