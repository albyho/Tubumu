using System.ComponentModel.DataAnnotations;

namespace Tubumu.Modules.Framework.ModelValidation.Attributes
{
    /// <summary>
    /// 身份证号码
    /// </summary>
    public class ChineseIdCardNumberAttribute : RegularExpressionAttribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ChineseIdCardNumberAttribute() : base(@"^(^\d{15}$|^\d{18}$|^\d{17}(\d|X|x))$")
        {
        }
    }
}
