using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace Tubumu.DataAnnotations
{
    /// <summary>
    /// 判断集合的元素数是否在某个范围
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class CollectionCountRangeAttribute : ValidationAttribute
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
        /// Constructor
        /// </summary>
        /// <param name="minimum"></param>
        /// <param name="maximum"></param>
        public CollectionCountRangeAttribute(int minimum, int maximum)
        {
            Minimum = minimum;
            Maximum = maximum;
        }

        /// <summary>
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
