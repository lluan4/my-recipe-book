using Microsoft.EntityFrameworkCore;
using MyRecipeBook.Domain.Enums;
using MyRecipeBook.Domain.Repositories.Difficulty;

namespace MyRecipeBook.Infrastructure.DataAccess.Repositories
{
    public class DifficultyRepository(MyRecipeBookDbContext dbContext) : IDifficultyReadOnlyRepository
    {
        public async Task<bool> ExistsDifficulty(RecipeDifficulty difficulty)
        {
            return await dbContext.Difficulty.AnyAsync((System.Linq.Expressions.Expression<Func<Domain.Entities.Difficulty, bool>>)(d => d.Id == difficulty));

        }
    }
}
