using System;

namespace Tubumu.Modules.Admin.Models
{
    /// <summary>
    /// 验证码类型
    /// </summary>
    public enum MobileValidationCodeType
    {
        /// <summary>
        /// 注册
        /// </summary>
        Register,

        /// <summary>
        /// 重置密码
        /// </summary>
        ResetPassword,

        /// <summary>
        /// 修改手机号
        /// </summary>
        ChangeMobile,

        /// <summary>
        /// 登录
        /// </summary>
        Login,

        /// <summary>
        /// 绑定手机号
        /// </summary>
        Bind
    }

    /// <summary>
    /// 手机验证码
    /// </summary>
    public class MobileValidationCode
    {
        /// <summary>
        /// 手机号
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        public string ValidationCode { get; set; }

        /// <summary>
        /// 验证码类型
        /// </summary>
        public MobileValidationCodeType Type { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime ExpirationDate { get; set; }

        /// <summary>
        /// 完成验证时间
        /// </summary>
        public DateTime? FinishVerifyDate { get; set; }

        /// <summary>
        /// 验证次数
        /// </summary>
        public int VerifyTimes { get; set; }

        /// <summary>
        /// 允许验证的最大次数
        /// </summary>
        public int MaxVerifyTimes { get; set; }
    }
}
