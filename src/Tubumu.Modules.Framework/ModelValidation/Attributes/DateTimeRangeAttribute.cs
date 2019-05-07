using System;
using System.ComponentModel.DataAnnotations;
using Tubumu.Core.Extensions;

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
        public DateTime Maximum { get; }

        /// <summary>
        /// Minimum
        /// </summary>
        public DateTime Minimum { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="minimum"></param>
        /// <param name="maximum"></param>
        public DateTimeRangeAttribute(string minimum, string maximum)
        {
            Minimum = DateTime.Parse(minimum);
            Maximum = DateTime.Parse(maximum);
        }

        /// <summary>
        /// IsValid
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool IsValid(object value)
        {
            if (value == null) return true;
            var stringValue = value.ToString();
            if (stringValue.IsNullOrWhiteSpace()) return true;
            if (DateTime.TryParse(stringValue, out var nativeValue))
            {
                nativeValue = nativeValue.ToLocalTime();
                return nativeValue >= Minimum && nativeValue <= Maximum;
            }
            return false;
        }
    }
}
