using MyRecipeBook.Domain.Enums;

namespace MyRecipeBook.Domain.Entities
{
    public class Recipe : EntityBase
    {
        public string Title { get; set; } = string.Empty;
        public Enums.CookingTime? CookingTimeId { get; set; }
        public Enums.Difficulty? DifficultyId { get; set; }
        public IList<Ingredient> Ingredients { get; set; } = [];
        public IList<Instruction> Instructions { get; set; } = [];
        public IList<RecipesDishTypes> RecipeDishTypes { get; set; } = [];
        public long UserId { get; set; }
    }
}
