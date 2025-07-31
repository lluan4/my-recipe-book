using Bogus;
using MyRecipeBook.Communication.Enums;
using MyRecipeBook.Communication.Request;

namespace CommonTestUtilities.Requests
{
    public class RequestRecipeJsonBuilder
    {

        public static RequestRecipeJson Build()
        {
            var step = 1;

            return new Faker<RequestRecipeJson>()
                 .RuleFor(r => r.Title, f => f.Lorem.Word())
                 .RuleFor(r => r.CookingTimeId, f => f.PickRandom<RecipeCookingTime>())
                 .RuleFor(r => r.DifficultyId, f => f.PickRandom<RecipeDifficulty>())
                 .RuleFor(r => r.Ingredients, f => f.Make(3, () => f.Commerce.ProductName()))
                 .RuleFor(r => r.DishTypes, f => f.Random.EnumValues<RecipeDishType>().Take(3).ToList())
                 .RuleFor(r => r.Instructions, f => f.Make(3, () => new RequestInstructionJson
                 {
                     Description = f.Lorem.Paragraph(),
                     Step = step++,
                 }));

        }
    }
}
