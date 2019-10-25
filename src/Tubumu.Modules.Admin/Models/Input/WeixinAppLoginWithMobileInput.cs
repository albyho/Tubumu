using System.ComponentModel.DataAnnotations;

namespace Tubumu.Modules.Admin.Models.Input
{
    /// <summary>
    /// 微信小程序登录 Input
    /// </summary>
    public class WeixinAppLoginWithMobileInput : WeixinAppLoginInput
    {
        /// <summary>
        /// 包括敏感数据在内的完整用户信息的加密数据
        /// </summary>
        public string EncryptedData { get; set; }

        /// <summary>
        /// 加密算法的初始向量
        /// </summary>
        public string IV { get; set; }
    }
}
