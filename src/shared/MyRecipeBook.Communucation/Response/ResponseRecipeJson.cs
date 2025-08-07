using MyRecipeBook.Communication.Enums;

namespace MyRecipeBook.Communication.Response
{
    public class ResponseGeneratedRecipeJson
    {
        public string Title { get; set; } = string.Empty;
        public IList<string> Ingredients { get; set; } = [];
        public IList<ResponseGeneratedInstructionJson> Instructions { get; set; } = [];
        public RecipeCookingTime CookingTime { get; set; }
        public RecipeDifficulty Difficulty { get; set; }

    }
}
