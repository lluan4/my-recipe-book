using MyRecipeBook.Domain.Enums;

namespace MyRecipeBook.Domain.Repositories.CookingTime
{
    public interface ICookingTimeReadOnlyRepository
    {
        Task<bool> ExistsCookingTime(Enums.CookingTime cookingTime);
    }
}
