using MyRecipeBook.Domain.Enums;

namespace MyRecipeBook.Domain.Entities
{
    public class RecipesDishTypes : JunctionEntityBase
    {
        public long RecipeId { get; set; }
        public Recipe Recipe { get; set; } = default!;

        public DishTypeEnum DishTypeId { get; set; }
        public DishTypes DishType { get; set; } = default!;

    }
}
