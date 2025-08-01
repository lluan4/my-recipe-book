
using MyRecipeBook.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyRecipeBook.Domain.Entities
{
    [Table("difficulties")]
    public class Difficulty : ReferenceEntityBase<RecipeDifficulty>
    {
    }
}