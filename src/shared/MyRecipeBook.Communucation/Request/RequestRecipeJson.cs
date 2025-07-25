using MyRecipeBook.Communication.Enums;

namespace MyRecipeBook.Communication.Request
{
    public class RequestRecipeJson
    {
        public string Title { get; set; } = string.Empty;
        public CookingTimeEnum? CookingTimeId { get; set; }
        public DifficultyEnum? DifficultyId { get; set; }
        public IList<string> Ingredients { get; set; } = [];
        public IList<RequestInstructionJson> Instructions { get; set; } = [];
        public IList<DishTypeEnum> DishTypes { get; set; } = [];
    }
}
