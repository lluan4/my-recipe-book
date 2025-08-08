using CommonTestUtilities.Dtos;
using CommonTestUtilities.GeminiAI;
using CommonTestUtilities.Requests;
using MyRecipeBook.Application.UseCases.Recipe.Generate;
using MyRecipeBook.Domain.Dtos.Recipes;
using MyRecipeBook.Domain.ValueObjects;
using MyRecipeBook.Exceptions.ExceptionsBase;
using Shouldly;

namespace UseCases.Test.Recipe.Generate
{
    public class GenerateRecipeUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            var dto = GeneratedRecipeDtoBuilder.Build();

            var request = RequestGenerateRecipeJsonBuilder.Build();

            var useCase = CreateUseCase(dto);

            var result = await useCase.Execute(request);

            result.ShouldNotBeNull();
            result.Title.ShouldBe(dto.Title);
            result.CookingTime.ShouldBe((MyRecipeBook.Communication.Enums.RecipeCookingTime)dto.CookingTime);
            result.Difficulty.ShouldBe((MyRecipeBook.Communication.Enums.RecipeDifficulty)dto.Difficulty);
        }

        [Fact]
        public async Task Error_Duplcated_Ingredients()
        {
            var dto = GeneratedRecipeDtoBuilder.Build();

            var request = RequestGenerateRecipeJsonBuilder.Build(count: MyRecipeBookRuleConstants.MAXIMUM_INGREDIENTS_GENERATE_RECIPE - 1);
            request.Ingredients.Add(request.Ingredients[0]);

            var useCase = CreateUseCase(dto);

            var act = async () => await useCase.Execute(request);

            var ex = await Should.ThrowAsync<ErrorOnValidationException>(act);

            ex.ShouldSatisfyAllConditions(
                () => ex.GetErrorMessages().Count.ShouldBe(1),
                () => ex.GetErrorMessages().ShouldContain(ResourceMessageHelper.FieldDuplicateValue("Ingredients"))
            );
        }

        private static GeneratedRecipeUseCase CreateUseCase(GeneratedRecipeDto dto)
        {
            var generateRecipeAI = GenerateRecipeAIBuilder.Build(dto);

            return new GeneratedRecipeUseCase(generateRecipeAI);
        }
    }
}
