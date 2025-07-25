using MyRecipeBook.Domain.Enums;

namespace MyRecipeBook.Domain.Repositories.Difficulty
{
    public interface IDifficultyReadOnlyRepository
    {
        Task<bool> ExistsDifficulty(Enums.RecipeDifficulty difficulty);
    }
}
