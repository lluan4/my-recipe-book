using MyRecipeBook.Domain.Enums;

namespace MyRecipeBook.Domain.Dtos.Recipes
{
    public record GeneratedRecipeDto
    {
        public string Title { get; init; } = string.Empty;
        public IList<string> Ingredients { get; init; } = [];
        public IList<GeneratedInstructionDto> Instructions { get; init; } = [];
        public RecipeCookingTime CookingTime { get; init; }
        public RecipeDifficulty Difficulty { get; init; }
        public RecipeDishType DishType { get; init; } 
    }
}
