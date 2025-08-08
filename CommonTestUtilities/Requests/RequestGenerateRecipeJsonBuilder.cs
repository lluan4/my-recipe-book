using Bogus;
using MyRecipeBook.Communication.Request;
using MyRecipeBook.Domain.ValueObjects;

namespace CommonTestUtilities.Requests
{
    public class RequestGenerateRecipeJsonBuilder
    {
        public static RequestGenerateRecipeJson Build(int count = MyRecipeBookRuleConstants.MAXIMUM_INGREDIENTS_GENERATE_RECIPE)
        {
            return new Faker<RequestGenerateRecipeJson>()
                .RuleFor(r => r.Ingredients, f => f.Make(count, () => f.Commerce.ProductName()));
        }
    }
}
