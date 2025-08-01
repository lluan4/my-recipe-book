using System.ComponentModel.DataAnnotations.Schema;

namespace MyRecipeBook.Domain.Entities
{
    [Table("recipes_dish_types")]
    public class RecipeDishType : JunctionEntityBase
    {
        public long RecipeId { get; set; }
        public Recipe Recipe { get; set; } = default!;

        public Enums.RecipeDishType DishTypeId { get; set; }
        public DishType DishType { get; set; } = default!;

    }
}
