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
                 .RuleFor(r => r.CookingTime, f => f.PickRandom<CookingTimeEnum>())
                 .RuleFor(r => r.Difficulty, f => f.PickRandom<DifficultyEnum>())
                 .RuleFor(r => r.Ingredients, f => f.Make(3, () => f.Commerce.ProductName()))
                 .RuleFor(r => r.DishTypes, f => f.Make(3, () => f.PickRandom<DishTypeEnum>()))
                 .RuleFor(r => r.Instructions, f => f.Make(3, () => new RequestInstructionJson
                 {
                     Description = f.Lorem.Paragraph(),
                     Step = step++,
                 }));
                 
        }
    }
}
