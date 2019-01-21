using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Tubumu.Modules.Framework.Extensions;

namespace Tubumu.Modules.Framework.ModelValidation.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = true, Inherited = true)]
    public class MutexAttribute : ValidationAttribute
    {
        public string OtherProperty { get; private set; }
        private readonly bool _canBeNull;
        private const string DefaultErrorMessage = "对{0}和{1}只能有一个输入";
        private const string DefaultNullErrorMessage = "对{0}和{1}需要有一个输入";

        public MutexAttribute(string originalProperty, bool canBeNull)
            : base(DefaultErrorMessage)
        {
            OtherProperty = originalProperty;
            _canBeNull = canBeNull;
        }

        public override string FormatErrorMessage(string name)
        {
            return String.Format(CultureInfo.CurrentUICulture, ErrorMessageString, name, OtherProperty);
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ValidationResult validationResult = null;
            var originalProperty = validationContext.ObjectType.GetProperty(OtherProperty);
            if(originalProperty == null)
            {
                throw new NullReferenceException(nameof(originalProperty));
            }
            var otherValue = originalProperty.GetValue(validationContext.ObjectInstance, null);

            var valueString = (value??String.Empty).ToString();
            var otherValueString = (otherValue??String.Empty).ToString();

            if (valueString.IsNullOrEmpty()&&otherValueString.IsNullOrEmpty())
            {
                if (_canBeNull)
                {
                    return ValidationResult.Success;
                }

                validationResult = new ValidationResult(String.Format(CultureInfo.CurrentUICulture, DefaultNullErrorMessage, validationContext.DisplayName, OtherProperty));
            }
            else if (!valueString.IsNullOrEmpty() && !otherValueString.IsNullOrEmpty())
            {
                validationResult = new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            }    

            if (validationResult != null)
                return validationResult;

            return ValidationResult.Success;
        }
    }
}
