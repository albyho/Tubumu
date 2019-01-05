using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Tubumu.Modules.Framework.ModelValidation.Attributes;

namespace Tubumu.Modules.Admin.Models.Input
{
    /// <summary>
    /// 发送短信 Input
    /// </summary>
    public class SendSmsInput
    {
        /// <summary>
        /// 手机号码
        /// <remark>多个手机号以半角逗号分隔</remark>
        /// </summary>
        [Required(ErrorMessage = "请输入手机号码")]
        [ChineseMobilePeriod(ErrorMessage = "请输入以半角逗号分隔的手机号码")]
        public string Mobile {get;set;}

        /// <summary>
        /// 内容
        /// </summary>
        [Required(ErrorMessage = "请输入内容")]
        public string Content {get;set;}
    }
}
