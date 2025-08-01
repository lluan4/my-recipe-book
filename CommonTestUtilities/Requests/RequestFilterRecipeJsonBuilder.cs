using Bogus;
using MyRecipeBook.Communication.Enums;
using MyRecipeBook.Communication.Request;


namespace CommonTestUtilities.Requests
{
    public class RequestFilterRecipeJsonBuilder
    {

        public static RequestFilterRecipeJson Build()
        {
            return new Faker<RequestFilterRecipeJson>()
                 .RuleFor(r => r.RecipeTitle_Ingredient, f => f.Lorem.Word())
                 .RuleFor(r => r.CookingTimes, f => f.Random.EnumValues<RecipeCookingTime>().Take(TakeNumber<RecipeCookingTime>()).ToList())
                 .RuleFor(r => r.Difficulties, f => f.Random.EnumValues<RecipeDifficulty>().Take(TakeNumber<RecipeDifficulty>()).ToList())
                 .RuleFor(r => r.DishTypes, f => f.Random.EnumValues<RecipeDishType>().Take(TakeNumber<RecipeDishType>()).ToList());
        }

        protected static int TakeNumber<TEnum>() where TEnum : Enum
        {
            var total = Enum.GetValues(typeof(TEnum)).Length;
            var random = new Random();
            return random.Next(1, total + 1);
        }

    }
}
