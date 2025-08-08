using MyRecipeBook.Communication.Request;
using MyRecipeBook.Communication.Response;
using MyRecipeBook.Domain.Extension;
using MyRecipeBook.Domain.Services.GeminiApi;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace MyRecipeBook.Application.UseCases.Recipe.Generate
{
    public class GeneratedRecipeUseCase : IGeneratedRecipeUseCase
    {
        private readonly IGenerateRecipeAI _generator;

        public GeneratedRecipeUseCase(IGenerateRecipeAI generator)
        {
            _generator = generator;
        }

        public async Task<ResponseGeneratedRecipeJson> Execute(RequestGenerateRecipeJson request)
        {
            Validate(request);

            var response = await _generator.Generate(request.Ingredients);

            return new ResponseGeneratedRecipeJson
            {
                Title = response.Title,
                Ingredients = response.Ingredients,
                CookingTime = (Communication.Enums.RecipeCookingTime)response.CookingTime,
                Difficulty = (Communication.Enums.RecipeDifficulty)response.Difficulty,
                Instructions = response.Instructions.Select(c => new ResponseGeneratedInstructionJson
                {
                    Step = c.Step,
                    Description = c.Description
                }).ToList()
            };
        }

        private static void Validate(RequestGenerateRecipeJson request)
        {
            var result = new GeneratedRecipeValidator().Validate(request);

            if (result.IsValid.isFalse())
                throw new ErrorOnValidationException(result.Errors.Select(x => x.ErrorMessage).ToList());
                          
        }
    }
}
