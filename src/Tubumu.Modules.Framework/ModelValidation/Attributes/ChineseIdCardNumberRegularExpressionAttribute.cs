using System;
using System.ComponentModel.DataAnnotations;

namespace Tubumu.Modules.Framework.ModelValidation.Attributes
{
    /// <summary>
    /// 身份证号码
    /// </summary>
    public class ChineseIdCardNumberRegularExpressionAttribute : RegularExpressionAttribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ChineseIdCardNumberRegularExpressionAttribute() : base(@"^(^\d{15}$|^\d{18}$|^\d{17}(\d|X|x))$")
        {
        }
    }
}
