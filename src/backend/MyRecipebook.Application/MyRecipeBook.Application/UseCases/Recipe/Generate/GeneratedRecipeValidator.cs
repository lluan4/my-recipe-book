using FluentValidation;
using MyRecipeBook.Communication.Request;
using MyRecipeBook.Domain.ValueObjects;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace MyRecipeBook.Application.UseCases.Recipe.Generate
{
    public class GeneratedRecipeValidator : AbstractValidator<RequestGenerateRecipeJson>
    {
        private const string INGREDIENTS_FIELD = "Ingredients";
        public GeneratedRecipeValidator()
        {
            var maximum_number_ingredients = MyRecipeBookRuleConstants.MAXIMUM_INGREDIENTS_GENERATE_RECIPE;
            var minimum_number_ingredients = MyRecipeBookRuleConstants.MINIMUM_INGREDIENTS_GENERATE_RECIPE;

            RuleFor(r => r.Ingredients.Count)
                .InclusiveBetween(minimum_number_ingredients, maximum_number_ingredients)
                .WithMessage(ResourceMessageHelper.FieldOutOfRange(fieldName: INGREDIENTS_FIELD, firstValue: minimum_number_ingredients.ToString(), secondValue: maximum_number_ingredients.ToString()));

            RuleFor(r => r.Ingredients)
                .Must(ingredients => ingredients.Count == ingredients.Distinct().Count())
                .WithMessage(ResourceMessageHelper.FieldDuplicateValue(fieldName: INGREDIENTS_FIELD));

            RuleFor(r => r.Ingredients)
                .ForEach(rule => rule.Custom(
                    (value, context) => GetRule(value, context)
                    )
                )
                .WithMessage(ResourceMessageHelper.FieldDuplicateValue(fieldName: INGREDIENTS_FIELD));
        }

        private static void GetRule(string value, ValidationContext<IEnumerable<string>> context)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                context.AddFailure(INGREDIENTS_FIELD, ResourceMessageHelper.FieldEmpty(fieldName: INGREDIENTS_FIELD));
                return;
            }

            if (value.Count(c => c == ' ') > 3 || value.Count(c => c == '/') > 1)
            {
                context.AddFailure(INGREDIENTS_FIELD, ResourceMessageHelper.FieldPatternMismatch(fieldName: INGREDIENTS_FIELD, pattern: "value1, value2 or 3/4 value2"));
            }

        }

    }
}
