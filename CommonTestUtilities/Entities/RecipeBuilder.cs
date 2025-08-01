using Bogus;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Enums;


namespace CommonTestUtilities.Entities
{
    public class RecipeBuilder
    {
        public static IList<Recipe> Collection(User user, uint count = 2)
        {
            var list = new List<Recipe>();

            if (count == 0)
                count = 1;

            var recipeId = 1;

            for (int i = 0; i < count; i++)
            {
                var fakeRecipe = Build(user);
                fakeRecipe.Id = recipeId++;

                list.Add(fakeRecipe);
            }

            return list;
        }

        public static Recipe Build(User user)
        {
            return new Faker<Recipe>()
                .RuleFor(r => r.Id, () => 1)
                .RuleFor(r => r.Title, (f) => f.Lorem.Word())
                .RuleFor(r => r.CookingTimeId, (f) => f.PickRandom<RecipeCookingTime>())
                .RuleFor(r => r.DifficultyId, (f) => f.PickRandom<RecipeDifficulty>())
                .RuleFor(r => r.Ingredients, (f) => f.Make(1, () => new Ingredient
                {
                    Id = 1,
                    Item = f.Commerce.ProductName()
                }))
                .RuleFor(r => r.Instructions, (f) => f.Make(1, () => new Instruction
                {
                    Id = 1,
                    Step = 1,
                    Description = f.Lorem.Paragraph()
                }))
                .RuleFor(u => u.RecipeDishTypes, (f) => f.Make(1, () => new MyRecipeBook.Domain.Entities.RecipeDishType
                {
                    RecipeId = 1,
                    DishTypeId = f.PickRandom<MyRecipeBook.Domain.Enums.RecipeDishType>()
                }))
                .RuleFor(r => r.UserId, _ => user.Id);

        }
    }
}
