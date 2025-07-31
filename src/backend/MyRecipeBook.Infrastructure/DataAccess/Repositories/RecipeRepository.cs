using Microsoft.EntityFrameworkCore;
using MyRecipeBook.Domain.Dtos;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Extension;
using MyRecipeBook.Domain.Repositories.Recipe;

namespace MyRecipeBook.Infrastructure.DataAccess.Repositories
{
    public class RecipeRepository : IRecipeWriteOnlyRepository, IRecipeReadOnlyRepository
    {
        private readonly MyRecipeBookDbContext _dbContext;

        public RecipeRepository(MyRecipeBookDbContext dbContext) => _dbContext = dbContext;

        public async Task Add(Recipe recipe) => await _dbContext.Recipes.AddAsync(recipe);

        public async Task<IList<Recipe>> Filter(User user, FilterRecipesDto filters)
        {
           var query = _dbContext
                .Recipes
                .AsNoTracking()
                .Include(recipe => recipe.Ingredients)
                .Where(recipe => recipe.Active && recipe.UserId == user.Id);

            if (filters.Difficulties.Any())
            {
                query = query.Where(recipe => recipe.DifficultyId.HasValue && filters.Difficulties.Contains(recipe.DifficultyId.Value));
            }

            if (filters.CookingTimes.Any())
            {
                query = query.Where(recipe => recipe.CookingTimeId.HasValue && filters.CookingTimes.Contains(recipe.CookingTimeId.Value));
            }

            if (filters.DishTypes.Any())
            {
                query = query.Where(recipe => recipe.RecipeDishTypes.Any(dishtype => filters.DishTypes.Contains(dishtype.DishTypeId)));
            }

            if (filters.RecipeTitle_Ingredient.NotEmpty())
            {
                query = query.Where(recipe => recipe.Title.Contains(filters.RecipeTitle_Ingredient) 
                || recipe.Ingredients.Any(ingredient => ingredient.Item.Contains(filters.RecipeTitle_Ingredient)));
            }

            return await query.ToListAsync();
        }
    }
}
