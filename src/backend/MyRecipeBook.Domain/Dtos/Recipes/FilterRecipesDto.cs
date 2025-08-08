using MyRecipeBook.Domain.Enums;

namespace MyRecipeBook.Domain.Dtos.Recipes
{
    public record FilterRecipesDto
    {
        public string? RecipeTitle_Ingredient {  get; init; }
        public IList<RecipeCookingTime> CookingTimes { get; init; } = [];
        public IList<RecipeDifficulty> Difficulties { get; init; } = [];
        public IList<RecipeDishType> DishTypes { get; init; } = [];
    }
}
