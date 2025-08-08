using MyRecipeBook.Domain.Dtos.Recipes;

namespace MyRecipeBook.Domain.Services.GeminiApi
{
    public interface IGenerateRecipeAI
    {
        Task<GeneratedRecipeDto> Generate(IList<string> ingredients);
    }
}
