using System.ComponentModel.DataAnnotations.Schema;

namespace MyRecipeBook.Domain.Entities
{
	[Table("recipes")]
	public class Recipe:EntityBase
	{
		public string Title { get; set; } = string.Empty;
		public Enums.RecipeCookingTime? CookingTimeId { get; set; }
		public CookingTime CookingTime { get; set; } = null!;
		public Enums.RecipeDifficulty? DifficultyId { get; set; }
		public Difficulty Difficulty { get; set; } = null!;
		public IList<Ingredient> Ingredients { get; set; } = [];
		public IList<Instruction> Instructions { get; set; } = [];
		public IList<RecipeDishType> RecipeDishTypes { get; set; } = [];
		public string? ImageIdentifier { get; set; }
		public long UserId { get; set; }
	}
}
