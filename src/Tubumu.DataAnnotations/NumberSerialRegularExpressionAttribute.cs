using System.ComponentModel.DataAnnotations;

namespace Tubumu.DataAnnotations
{
    /// <summary>
    /// 纯数字，可以是0开头
    /// </summary>
    public class NumberSerialAttribute : RegularExpressionAttribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public NumberSerialAttribute(int length) : base(@"^\d{" + length + @"}$")
        {
        }
    }
}
