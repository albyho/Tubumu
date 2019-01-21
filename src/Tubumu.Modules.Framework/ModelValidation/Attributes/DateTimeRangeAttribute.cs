using System;
using System.ComponentModel.DataAnnotations;

namespace Tubumu.Modules.Framework.ModelValidation.Attributes
{
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Field | AttributeTargets.Property, AllowMultiple=false)]
    public class DateTimeRangeAttribute : ValidationAttribute
    {

        public object Maximum { get; }
        public object Minimum { get; }

        public DateTimeRangeAttribute(DateTime minimum, DateTime maximum)
        {
            Minimum = minimum;
            Maximum = maximum;
        }

        public override string FormatErrorMessage(string name)
        {
            return String.Empty;
        }

        public override bool IsValid(object value)
        {
            return value == null;
        }
    }
}
