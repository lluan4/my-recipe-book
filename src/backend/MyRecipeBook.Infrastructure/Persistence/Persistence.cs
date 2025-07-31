namespace MyRecipeBook.Infrastructure.Persistence
{
    public static class DbNames
    {
        public static class Tables
        {
            public const string Recipes = "Recipes";
            public const string Ingredients = "Ingredients";
            public const string Instructions = "Instructions";
            public const string CookingTime = "CookingTime";
            public const string Difficulty = "Difficulty";
            public const string DishTypes = "DishTypes";
            public const string RecipesDishTypes = "RecipesDishTypes";
            public const string Users = "Users";
        }

        public static class Schemas
        {
            public const string Default = ""; 
        }
    }
}
