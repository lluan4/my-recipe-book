using GenerativeAI;
using GenerativeAI.Types;
using MyRecipeBook.Application.SharedValidators;
using MyRecipeBook.Domain.Dtos.Recipes;
using MyRecipeBook.Domain.Enums;
using MyRecipeBook.Domain.Extension;
using MyRecipeBook.Domain.Services.GeminiApi;
using MyRecipeBook.Exceptions.ExceptionsBase;


namespace MyRecipeBook.Infrastructure.Services.GeminiApi
{
    public class GeminiService : IGenerateRecipeAI
    {
        private const string CHAT_MODEL = "models/gemini-1.5-flash";
        private readonly IGenerativeAI _geminiApi;

        public GeminiService(IGenerativeAI geminiApi)
        {
            _geminiApi = geminiApi;
        }

        public async Task<GeneratedRecipeDto> Generate(IList<string> ingredients)
        {
            var model = _geminiApi.CreateGenerativeModel(CHAT_MODEL);

            var systemInstruction = ResourceGeminiAI.STARTING_GENERATE_RECIPE;

            var chat = model.StartChat(systemInstruction: systemInstruction);

            var ingredientsText = string.Join(";", ingredients);

            var response = await chat.GenerateContentAsync(ingredientsText);

            var responseText = response.Text();

            var responseList = responseText?
                .Split("\n")
                .Where(item => string.IsNullOrEmpty(item).isFalse())
                .Select(item => item.Replace("[", "").Replace("]", ""))
                .ToList();

           
            if (responseList == null || responseList.Count < 4)
            {
                throw new ErrorOnValidationException(new List<string> 
                { 
                    "Resposta da IA incompleta. Não foi possível gerar a receita com todos os dados necessários." 
                });
            }

            var step = 1;

            var recipeData = new GeneratedRecipeDataModel
            {
                Title = responseList[0]?.Trim() ?? string.Empty,
                CookingTimeValue = responseList[1]?.Trim() ?? string.Empty,
                DifficultyValue = responseList[2]?.Trim() ?? string.Empty,
                DishTypeValue = responseList[3]?.Trim() ?? string.Empty,
                Ingredients = responseList[4]?.Split(";").Where(i => !string.IsNullOrWhiteSpace(i)).Select(i => i.Trim()).ToList() ?? new List<string>(),
                Instructions = responseList[5]?.Split("@").Select(instruction => new GeneratedInstructionDto
                {
                    Description = instruction.Trim(),
                    Step = step++
                }
                ).ToList() ?? new List<GeneratedInstructionDto>()
            };

            await ValidateGeneratedRecipeData(recipeData);

            var cookingTime = Enum.Parse<RecipeCookingTime>(recipeData.CookingTimeValue);
            var difficulty = Enum.Parse<RecipeDifficulty>(recipeData.DifficultyValue);
            var dishType = Enum.Parse<RecipeDishType>(recipeData.DishTypeValue);

            return new GeneratedRecipeDto
            {
                Title = recipeData.Title,
                CookingTime = cookingTime,
                Difficulty = difficulty,
                DishType = dishType,
                Ingredients = recipeData.Ingredients,
                Instructions = recipeData.Instructions.Select(i => new GeneratedInstructionDto
                {
                    Step = i.Step,
                    Description = i.Description.Trim().CapitalizeFirstLetterExtension()
                }).ToList()
            };
        }

        private static async Task ValidateGeneratedRecipeData(GeneratedRecipeDataModel recipeData)
        {
            var validator = new GeneratedRecipeDataValidator();
            var result = await validator.ValidateAsync(recipeData);

            if (!result.IsValid)
            {
                var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();
                throw new ErrorOnValidationException(errorMessages);
            }
        }
    }
}
