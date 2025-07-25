using FluentValidation;
using FluentValidation.Validators;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace MyRecipeBook.Application.SharedValidators
{
    public class StringValidator<T> : PropertyValidator<T, string>
    {
        private readonly int _maxLength;
        private readonly string _fieldName;

        public StringValidator(string fieldName, int maxLength = 255)
        {
            _maxLength = maxLength;
            _fieldName = fieldName;
        }

        public override bool IsValid(ValidationContext<T> context, string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                context.MessageFormatter
                    .AppendArgument("ErrorMessage", ResourceMessageHelper.FieldEmpty(_fieldName));

                return false;
            }

            if (value.Length > _maxLength)
            {
                context.MessageFormatter.AppendArgument("ErrorMessage",
                    ResourceMessageHelper.FieldMustHaveMaxLength(_fieldName, _maxLength));
                return false;
            }

            return true;
        }

        public override string Name => "StringValidator";

        protected override string GetDefaultMessageTemplate(string errorCode) => "{ErrorMessage}";
    }
}
