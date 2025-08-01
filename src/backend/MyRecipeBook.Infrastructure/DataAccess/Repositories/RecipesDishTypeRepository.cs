using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Repositories.RecipesDishType;

namespace MyRecipeBook.Infrastructure.DataAccess.Repositories
{
    public class RecipesDishTypeRepository(MyRecipeBookDbContext dbContext) : IRecipesDishTypeWriteOnlyRepository
    {
        private readonly MyRecipeBookDbContext _dbContext = dbContext;

        public async Task Add(RecipeDishType entity) => await _dbContext.RecipesDishTypes.AddAsync(entity);

        public async Task AddRange(IList<RecipeDishType> entities) => await _dbContext.RecipesDishTypes.AddRangeAsync(entities);

    }
}
