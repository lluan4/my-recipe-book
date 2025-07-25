using Microsoft.EntityFrameworkCore;
using MyRecipeBook.Domain.Enums;
using MyRecipeBook.Domain.Repositories.CookingTime;

namespace MyRecipeBook.Infrastructure.DataAccess.Repositories
{
    public class CookingTimeRepository(MyRecipeBookDbContext dbContext) : ICookingTimeReadOnlyRepository
    {
        public async Task<bool> ExistsCookingTime(CookingTimeEnum cookingTime)
        {
            return await dbContext.CookingTime.AnyAsync(ct => ct.Id == cookingTime);
        }
    }
}
