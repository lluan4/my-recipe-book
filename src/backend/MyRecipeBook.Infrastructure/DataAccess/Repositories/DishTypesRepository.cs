using Microsoft.EntityFrameworkCore;
using MyRecipeBook.Domain.Enums;
using MyRecipeBook.Domain.Repositories.DishType;


namespace MyRecipeBook.Infrastructure.DataAccess.Repositories
{
    public class DishTypesRepository(MyRecipeBookDbContext _dbContext) : IDishTypeReadOnlyRepository
    {
        public async Task<bool> ExistsDishType(RecipeDishType dishType)
        {
            return await _dbContext.DishTypes
                 .AnyAsync(d => d.Id == dishType);
        }

        public async Task<List<RecipeDishType>> GetExistingDishTypes(IEnumerable<RecipeDishType> dishTypes)
        {
            return await _dbContext.DishTypes
                .Where(dt => dishTypes.Contains(dt.Id))
                .Select(dt => dt.Id)
                .ToListAsync();
        }
    }
}
