using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using MyRecipeBook.Domain.Dtos.Recipes;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Extension;
using MyRecipeBook.Domain.Repositories.Recipe;

namespace MyRecipeBook.Infrastructure.DataAccess.Repositories
{
	public class RecipeRepository:IRecipeWriteOnlyRepository, IRecipeReadOnlyRepository, IRecipeUpdateOnlyRepository
	{
		private readonly MyRecipeBookDbContext _dbContext;

		public RecipeRepository(MyRecipeBookDbContext dbContext) => _dbContext = dbContext;

		public async Task Add(Recipe recipe) => await _dbContext.Recipes.AddAsync(recipe);

		public async Task Delete(long recipeId)
		{
			var recipe = await _dbContext.Recipes.FindAsync(recipeId);

			_dbContext.Recipes.Remove(recipe!);
		}

		public async Task<IList<Recipe>> Filter(User user, FilterRecipesDto filters)
		{
			var query = GetBaseRecipe(user: user);

			if(filters.Difficulties.Any())
			{
				query = query.Where(recipe => recipe.DifficultyId.HasValue && filters.Difficulties.Contains(recipe.DifficultyId.Value));
			}

			if(filters.CookingTimes.Any())
			{
				query = query.Where(recipe => recipe.CookingTimeId.HasValue && filters.CookingTimes.Contains(recipe.CookingTimeId.Value));
			}

			if(filters.DishTypes.Any())
			{
				query = query.Where(recipe => recipe.RecipeDishTypes.Any(dishtype => filters.DishTypes.Contains(dishtype.DishTypeId)));
			}

			if(filters.RecipeTitle_Ingredient.NotEmpty())
			{
				query = query.Where(recipe => recipe.Title.Contains(filters.RecipeTitle_Ingredient)
				|| recipe.Ingredients.Any(ingredient => ingredient.Item.Contains(filters.RecipeTitle_Ingredient)));
			}

			return await query.ToListAsync();
		}

		public async Task<IList<Recipe>> GetForDashboard(User user)
		{
			return await GetBaseRecipe(user: user)
				 .OrderByDescending(recipe => recipe.CreatedOn)
				 .Take(5)
				 .ToListAsync();
		}

		async Task<Recipe?> IRecipeReadOnlyRepository.GetById(User user, long recipeId)
		{
			return await GetFullRecipe()
				 .AsNoTracking()
				 .FirstOrDefaultAsync(r => r.Id == recipeId &&
													r.UserId == user.Id &&
													r.Active);
		}

		async Task<Recipe?> IRecipeUpdateOnlyRepository.GetById(User user, long recipeId)
		{
			return await GetFullRecipe()
				.FirstOrDefaultAsync(r => r.Id == recipeId &&
												 r.UserId == user.Id &&
												 r.Active);
		}

		public void Update(Recipe recipe) => _dbContext.Recipes.Update(recipe);

		private IIncludableQueryable<Recipe, IList<Instruction>> GetFullRecipe()
		{
			return _dbContext
				 .Recipes
				 .Include(r => r.CookingTime)
				 .Include(r => r.Difficulty)
				 .Include(r => r.RecipeDishTypes)
					  .ThenInclude(rd => rd.DishType)
				 .Include(r => r.Ingredients)
				 .Include(r => r.Instructions);
		}

		private IQueryable<Recipe> GetBaseRecipe(User user)
		{
			return _dbContext
				 .Recipes
				 .AsNoTracking()
				 .Include(recipe => recipe.Ingredients)
				 .Where(recipe => recipe.Active && recipe.UserId == user.Id);
		}

	}
}
