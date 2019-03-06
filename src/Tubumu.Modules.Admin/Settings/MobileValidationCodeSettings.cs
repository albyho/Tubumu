using System;
using System.Collections.Generic;
using System.Text;
using Tubumu.Modules.Admin.Models;

namespace Tubumu.Modules.Admin.Settings
{
    /// <summary>
    /// MobileValidationCodeSettings
    /// </summary>
    public class MobileValidationCodeSettings
    {
        /// <summary>
        /// 验证码长度
        /// </summary>
        public int CodeLength { get; set; }

        /// <summary>
        /// 请求间隔，单位秒
        /// </summary>
        public int RequestInterval { get; set; }

        /// <summary>
        /// 到期时长，单位秒
        /// </summary>
        public int Expiration { get; set; }

        /// <summary>
        /// 最大验证次数
        /// </summary>
        public int MaxVerifyTimes { get; set; }
    }
}
