namespace MyRecipeBook.Infrastructure.Persistence
{

    public static class DbObjects
    {
      
        public static class Naming
        {
          
            public static string FK(string fromTable, string toTable) => $"FK_{fromTable}_{toTable}";

        
            public static string PK(string table) => $"PK_{table}";

            
            public static string IX(string table, params string[] columns) => $"IX_{table}_{string.Join("_", columns)}";

            
            public static string UC(string table, params string[] columns) => $"UC_{table}_{string.Join("_", columns)}";
        }

        public static class Users
        {
            public const string TableName = "Users";
            public const string Id = "Id";
        }

        public static class CookingTime
        {
            public const string TableName = "CookingTime";
            public const string Id = "Id";
        }

        public static class Difficulty
        {
            public const string TableName = "Difficulty";
            public const string Id = "Id";
        }

        public static class DishTypes
        {
            public const string TableName = "DishTypes";
            public const string Id = "Id";
        }

        public static class Recipes
        {
            public const string TableName = "Recipes";
            public const string Id = "Id";
            public const string Title = "Title";
            public const string UserId = "UserId";
            public const string CookingTimeId = "CookingTimeId";
            public const string DifficultyId = "DifficultyId";
        }

        public static class Ingredients
        {
            public const string TableName = "Ingredients";
            public const string Id = "Id";
            public const string Item = "Item";
            public const string RecipeId = "RecipeId";
        }

        public static class Instructions
        {
            public const string TableName = "Instructions";
            public const string Id = "Id";
            public const string Description = "Description";
            public const string RecipeId = "RecipeId";
            public const string Step = "Step";
        }

        public static class RecipesDishTypes
        {
            public const string TableName = "RecipesDishTypes";
            public const string RecipeId = "RecipeId";
            public const string DishTypeId = "DishTypeId";
        }
    }
}
