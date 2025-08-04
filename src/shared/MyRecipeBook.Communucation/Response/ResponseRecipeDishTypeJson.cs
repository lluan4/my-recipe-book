namespace MyRecipeBook.Communication.Response
{
    public class ResponseRecipeDishTypeJson
    {
        public int RecipeId { get; set; }
        public int DishTypeId { get; set; }
        public ResponseDishTypesJson? DishType { get; set; }

    }
}
