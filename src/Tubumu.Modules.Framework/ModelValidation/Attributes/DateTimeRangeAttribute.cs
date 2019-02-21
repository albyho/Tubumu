using System;
using System.ComponentModel.DataAnnotations;

namespace Tubumu.Modules.Framework.ModelValidation.Attributes
{
    /// <summary>
    /// DateTimeRangeAttribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class DateTimeRangeAttribute : ValidationAttribute
    {

        /// <summary>
        /// Maximum
        /// </summary>
        public object Maximum { get; }

        /// <summary>
        /// Minimum
        /// </summary>
        public object Minimum { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="minimum"></param>
        /// <param name="maximum"></param>
        public DateTimeRangeAttribute(DateTime minimum, DateTime maximum)
        {
            Minimum = minimum;
            Maximum = maximum;
        }

        /// <summary>
        /// FormatErrorMessage
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public override string FormatErrorMessage(string name)
        {
            return String.Empty;
        }

        /// <summary>
        /// IsValid
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool IsValid(object value)
        {
            return value == null;
        }
    }
}
