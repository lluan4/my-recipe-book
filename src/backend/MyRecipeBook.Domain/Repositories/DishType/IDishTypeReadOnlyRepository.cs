using MyRecipeBook.Domain.Enums;

namespace MyRecipeBook.Domain.Repositories.DishType
{
    public interface IDishTypeReadOnlyRepository
    {
        Task<bool> ExistsDishType(Enums.RecipeDishType dishType);
        Task<List<Enums.RecipeDishType>> GetExistingDishTypes(IEnumerable<Enums.RecipeDishType> dishTypes);
    }
}
