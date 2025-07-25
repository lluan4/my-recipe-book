using Microsoft.EntityFrameworkCore;
using MyRecipeBook.Domain.Enums;
using MyRecipeBook.Domain.Repositories.Difficulty;

namespace MyRecipeBook.Infrastructure.DataAccess.Repositories
{
    public class DifficultyRepository(MyRecipeBookDbContext dbContext) : IDifficultyReadOnlyRepository
    {
        public async Task<bool> ExistsDifficulty(DifficultyEnum difficulty)
        {
            return await dbContext.Difficulty.AnyAsync(d => d.Id == difficulty);

        }
    }
}
