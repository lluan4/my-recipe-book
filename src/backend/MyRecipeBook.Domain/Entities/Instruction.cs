using System.ComponentModel.DataAnnotations.Schema;

namespace MyRecipeBook.Domain.Entities
{
    [Table("instructions")]
    public class Instruction : EntityBase
    {
        public int Step { get; set; }
        public string Description { get; set; } = string.Empty;
        public long RecipeId { get; set; }

    }
}
