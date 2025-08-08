using CommonTestUtilities.Requests;
using MyRecipeBook.Application.UseCases.Recipe.Generate;
using MyRecipeBook.Domain.ValueObjects;
using MyRecipeBook.Exceptions.ExceptionsBase;
using Shouldly;
using System.Diagnostics.CodeAnalysis;

namespace Validators.Test.Recipe.Generate
{
    public class GenerateRecipeValidatorTest
    {
        private readonly GeneratedRecipeValidator _validator;

        public GenerateRecipeValidatorTest()
        {
            _validator = new GeneratedRecipeValidator();
        }

        [Fact]
        public void Success()
        {
            var request = RequestGenerateRecipeJsonBuilder.Build();

            var result = _validator.Validate(request);

            result.IsValid.ShouldBeTrue();
        }

        [Fact]
        public void Error_More_Maximum_Ingredients()
        {
            var request = RequestGenerateRecipeJsonBuilder
                .Build(MyRecipeBookRuleConstants.MAXIMUM_INGREDIENTS_GENERATE_RECIPE + 1);

            var result = _validator.Validate(request);

            result.IsValid.ShouldBeFalse();
            result.Errors.Count.ShouldBe(1);
            result.Errors[0].ErrorMessage
                .ShouldBe(
                ResourceMessageHelper.FieldOutOfRange("Ingredients",
                MyRecipeBookRuleConstants.MINIMUM_INGREDIENTS_GENERATE_RECIPE.ToString(),
                MyRecipeBookRuleConstants.MAXIMUM_INGREDIENTS_GENERATE_RECIPE.ToString()
                ));
        }

        [Fact]
        public void Error_Duplicated_Ingredient()
        {
            var request = RequestGenerateRecipeJsonBuilder.Build(MyRecipeBookRuleConstants.MAXIMUM_INGREDIENTS_GENERATE_RECIPE);
            request.Ingredients[1] = request.Ingredients[0];

            var result = _validator.Validate(request);

            result.IsValid.ShouldBeFalse();
            result.Errors.Count.ShouldBe(1);
            result.Errors[0].ErrorMessage
                .ShouldBe(ResourceMessageHelper.FieldDuplicateValue("Ingredients"));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("         ")]
        [InlineData("")]
        [SuppressMessage("Usage", "xUnit1012:Null should only be used for nullable parameters", Justification = "Because it is unit test")]
        public void Error_Empty_Ingredient(string ingredient)
        {
            var request = RequestGenerateRecipeJsonBuilder.Build(count: MyRecipeBookRuleConstants.MINIMUM_INGREDIENTS_GENERATE_RECIPE);
            request.Ingredients.Add(ingredient);

            var result = _validator.Validate(request);

            result.IsValid.ShouldBeFalse();
            result.Errors.Count.ShouldBe(1);
            result.Errors[0].ErrorMessage
                .ShouldBe(ResourceMessageHelper.FieldEmpty("Ingredients"));
        }

        [Fact]
        public void Error_Ingredient_Not_Following_Pattern()
        {
            var request = RequestGenerateRecipeJsonBuilder.Build(count: MyRecipeBookRuleConstants.MINIMUM_INGREDIENTS_GENERATE_RECIPE);

            request.Ingredients.Add("value1 value2 value3 value4 value5");

            var result = _validator.Validate(request);

            result.IsValid.ShouldBeFalse();
            result.Errors.Count.ShouldBe(1);
            result.Errors[0].ErrorMessage
                .ShouldBe(ResourceMessageHelper.FieldPatternMismatch("Ingredients", "value1, value2 or 3/4 value2"));
        }
    }
}
