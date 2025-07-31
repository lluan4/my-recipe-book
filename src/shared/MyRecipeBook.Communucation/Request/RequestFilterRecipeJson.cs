using MyRecipeBook.Communication.Enums;

namespace MyRecipeBook.Communication.Request
{
    public class RequestFilterRecipeJson
    {
        public string? RecipeTitle_Ingredient { get; set; }
        public IList<RecipeCookingTime> CookingTimes { get; set; } = [];
        public IList<RecipeDifficulty> Difficulties { get; set; } = [];
        public IList<RecipeDishType> DishTypes { get; set; } = [];
    }
}
