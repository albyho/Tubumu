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
        /// Constructor
        /// </summary>
        /// <param name="minimum"></param>
        /// <param name="maximum"></param>
        public CollectionElementRangeAttribute(int minimum, int maximum)
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
            if (value is null) return true;
            var isValid = true;
            if (value is IEnumerable<int> list && list != null)
            {
                foreach (var item in list)
                {
                    if(item < Minimum || item > Maximum)
                    {
                        isValid = false;
                        break;
                    }
                }
            }
            return isValid;
        }
    }
}
