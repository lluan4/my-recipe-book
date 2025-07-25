using MyRecipeBook.Communication.Enums;

namespace MyRecipeBook.Communication.Request
{
    public class RequestRecipeJson
    {
        public string Title { get; set; } = string.Empty;
        public CookingTime? CookingTimeId { get; set; }
        public Difficulty? DifficultyId { get; set; }
        public IList<string> Ingredients { get; set; } = [];
        public IList<RequestInstructionJson> Instructions { get; set; } = [];
        public IList<DishType> DishTypes { get; set; } = [];
    }
}
