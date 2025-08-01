using MyRecipeBook.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyRecipeBook.Domain.Entities
{
    [Table("dish_types")]
    public class DishType : ReferenceEntityBase<Enums.RecipeDishType> { }
}
