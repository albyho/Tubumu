using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Tubumu.Modules.Framework.ModelValidation.Attributes
{
    /// <summary>
    /// 判断集合的每个元素是否在某个范围
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
        /// c
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

            if (value is IEnumerable<int> list)
            {
                foreach (var item in list)
                {
                    return item >= Minimum && item <= Maximum;
                }
            }
            return false;
        }
    }
}
