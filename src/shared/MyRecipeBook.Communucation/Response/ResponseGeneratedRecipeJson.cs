namespace MyRecipeBook.Communication.Response
{
	public class ResponseRecipeJson
	{
		public string Id { get; set; } = string.Empty;
		public string Title { get; set; } = string.Empty;
		public IList<ResponseIngredientJson> Ingredients { get; set; } = [];
		public IList<ResponseInstructionJson> Instructions { get; set; } = [];
		public IList<ResponseDishTypesJson> DishTypes { get; set; } = [];
		public ResponseCookingTimeJson? CookingTime { get; set; }
		public ResponseDifficultyJson? Difficulty { get; set; }
		public string? ImageUrl { get; set; }
	}
}
