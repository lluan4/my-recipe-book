using FluentValidation;
using MyRecipeBook.Communication.Request;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace MyRecipeBook.Application.UseCases.Recipe.Filter
{
    public class FilterRecipeValidator : AbstractValidator<RequestFilterRecipeJson>
    {
        public FilterRecipeValidator()
        {
            RuleForEach(r => r.CookingTimes).IsInEnum().WithMessage(ResourceMessageHelper.FieldNotSupported(fieldName: "CookingTime"));
            RuleForEach(r => r.Difficulties).IsInEnum().WithMessage(ResourceMessageHelper.FieldNotSupported(fieldName: "Difficulties"));
            RuleForEach(r => r.DishTypes).IsInEnum().WithMessage(ResourceMessageHelper.FieldNotSupported(fieldName: "DishTypes"));
        }
    }
}
