using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace Tubumu.Modules.Framework.ModelValidation.Attributes
{
    /// <summary>
    /// CollectionElementRangeAttribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class CollectionElementRangeAttribute : ValidationAttribute
    {
        /// <summary>
        /// Minimum
        /// </summary>
        public int Minimum { get; }

        /// <summary>
        /// Maximum
        /// </summary>
        public int Maximum { get; }

        /// <summary>
        /// CollectionElementRangeAttribute
        /// </summary>
        /// <param name="minimum"></param>
        /// <param name="maximum"></param>
        public CollectionElementRangeAttribute(int minimum, int maximum)
        {
            Minimum = minimum;
            Maximum = maximum;
        }

        /// <summary>s
        /// IsValid
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool IsValid(object value)
        {
            if (value is ICollection list)
            {
                return list.Count >= Minimum && list.Count <= Maximum;
            }
            return false;
        }
    }
}
