using System.ComponentModel.DataAnnotations;

namespace Tubumu.Modules.Admin.Models.Input
{
    /// <summary>
    /// 微信移动端登录 Input
    /// </summary>
    public class WeixinMobileEndLoginInput
    {
        /// <summary>
        /// 微信登录 Code
        /// 用户换取access_token的code，仅在ErrCode为0时有效
        /// </summary>
        [Required(ErrorMessage = "微信登录 Code")]
        public string Code { get; set; }

        /// <summary>
        /// 回传数据
        /// 第三方程序发送时用来标识其请求的唯一性的标志，由第三方程序调用sendReq时传入，由微信终端回传，state字符串长度不能超过1K
        /// </summary>
        public string State { get; set; }
    }
}
