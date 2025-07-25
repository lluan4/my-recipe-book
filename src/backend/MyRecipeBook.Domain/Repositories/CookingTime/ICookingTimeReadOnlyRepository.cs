using MyRecipeBook.Domain.Enums;

namespace MyRecipeBook.Domain.Repositories.CookingTime
{
    public interface ICookingTimeReadOnlyRepository
    {
        Task<bool> ExistsCookingTime(CookingTimeEnum cookingTime);
    }
}
