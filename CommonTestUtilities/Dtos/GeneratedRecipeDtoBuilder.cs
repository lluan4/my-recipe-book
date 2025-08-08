using Bogus;
using MyRecipeBook.Domain.Dtos.Recipes;
using MyRecipeBook.Domain.Enums;

namespace CommonTestUtilities.Dtos
{
    public class GeneratedRecipeDtoBuilder
    {
        public static GeneratedRecipeDto Build()
        {
            return new Faker<GeneratedRecipeDto>()
                .RuleFor(r => r.Title, f => f.Lorem.Word())
                .RuleFor(r => r.CookingTime, f => f.PickRandom<RecipeCookingTime>())
                .RuleFor(r => r.Difficulty, f => f.PickRandom<RecipeDifficulty>())
                .RuleFor(r => r.DishType, f => f.PickRandom<RecipeDishType>())
                .RuleFor(r => r.Ingredients, f => f.Make(1, () => f.Commerce.ProductName()))
                .RuleFor(r => r.Instructions, f => f.Make(1, () => new GeneratedInstructionDto
                {
                    Step = 1,
                    Description = f.Lorem.Paragraph()
                }));

        }
    }
}
