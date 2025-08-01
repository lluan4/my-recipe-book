using System.ComponentModel.DataAnnotations.Schema;

namespace MyRecipeBook.Domain.Entities
{
    [Table("cooking_times")]
    public class CookingTime : ReferenceEntityBase<Enums.RecipeCookingTime> { }
}
