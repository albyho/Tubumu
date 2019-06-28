using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Tubumu.Modules.Framework.ModelValidation.Attributes
{
    /// <summary>
    /// CompareAttribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = true, Inherited = true)]
    public class CompareAttribute : ValidationAttribute
    {
        private const string DefaultErrorMessage = "对{0}和{1}属性或值进行{2}型的{3}比较失败";

        /// <summary>
        /// OtherProperty
        /// </summary>
        public string OtherProperty { get; }

        /// <summary>
        /// ValueToCompare
        /// </summary>
        public object ValueToCompare { get; }

        /// <summary>
        /// Operator
        /// </summary>
        public ValidationCompareOperator Operator { get; }

        /// <summary>
        /// DataType
        /// </summary>
        public ValidationDataType DataType { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="originalProperty"></param>
        /// <param name="compareOperator"></param>
        /// <param name="dataType"></param>
        public CompareAttribute(string originalProperty, ValidationCompareOperator compareOperator, ValidationDataType dataType)
            : this(originalProperty, null, compareOperator, dataType)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="originalProperty"></param>
        /// <param name="valueToCompare"></param>
        /// <param name="compareOperator"></param>
        /// <param name="dataType"></param>
        public CompareAttribute(string originalProperty, object valueToCompare, ValidationCompareOperator compareOperator, ValidationDataType dataType)
            : base(DefaultErrorMessage)
        {
            OtherProperty = originalProperty;
            ValueToCompare = valueToCompare;
            Operator = compareOperator;
            DataType = dataType;
        }

        /// <summary>
        /// FormatErrorMessage
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public override string FormatErrorMessage(string name)
        {
            return String.Format(CultureInfo.CurrentUICulture, ErrorMessageString,
                name, OtherProperty, DataType.ToString(), Operator.ToString());
        }

        /// <summary>
        /// IsValid
        /// </summary>
        /// <param name="value"></param>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ValidationResult validationResult = null;
            if (!string.IsNullOrEmpty(OtherProperty))
            {
                var originalProperty = validationContext.ObjectType.GetProperty(OtherProperty);
                if (originalProperty == null)
                {
                    throw new NullReferenceException(nameof(OtherProperty));
                }
                var otherValue = originalProperty.GetValue(validationContext.ObjectInstance, null);
                if (!Compare(Operator, DataType, value, otherValue))
                    validationResult = new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            }

            if (validationResult == null && ValueToCompare != null && !Compare(Operator, DataType, value, ValueToCompare))
                validationResult = new ValidationResult(FormatErrorMessage(validationContext.DisplayName));

            if (validationResult != null)
                return validationResult;

            return ValidationResult.Success;
        }

        private static bool Compare(ValidationCompareOperator compareOperator, ValidationDataType dataType, object value, object otherValue)
        {
            int num = 0;
            try
            {
                switch (dataType)
                {
                    case ValidationDataType.String:
                        num = string.Compare(value != null ? value.ToString() : String.Empty, otherValue != null ? otherValue.ToString() : String.Empty, false, CultureInfo.CurrentCulture);
                        break;

                    case ValidationDataType.Integer:
                        num = ((int)value).CompareTo(otherValue);
                        break;

                    case ValidationDataType.Double:
                        num = ((double)value).CompareTo(otherValue);
                        break;

                    case ValidationDataType.Date:
                        num = ((DateTime)value).CompareTo(otherValue);
                        break;

                    case ValidationDataType.Time:
                        num = ((TimeSpan)value).CompareTo(otherValue);
                        break;

                    case ValidationDataType.Currency:
                        num = ((decimal)value).CompareTo(otherValue);
                        break;
                }
            }
            catch
            {
                return false;
            }

            switch (compareOperator)
            {
                case ValidationCompareOperator.Equal:
                    return (num == 0);

                case ValidationCompareOperator.NotEqual:
                    return (num != 0);

                case ValidationCompareOperator.GreaterThan:
                    return (num > 0);

                case ValidationCompareOperator.GreaterThanEqual:
                    return (num >= 0);

                case ValidationCompareOperator.LessThan:
                    return (num < 0);

                case ValidationCompareOperator.LessThanEqual:
                    return (num <= 0);
            }
            return true;

        }
    }
}
