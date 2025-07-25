using MyRecipeBook.Domain.Enums;

namespace MyRecipeBook.Domain.Entities
{
    public class Instruction : EntityBase
    {
        public int Step { get; set; }
        public string Description { get; set; } = string.Empty;
        public long RecipeId { get; set; }
       
    }
}
