using MyRecipeBook.Domain.Enums;

namespace MyRecipeBook.Domain.Repositories.DishType
{
    public interface IDishTypeReadOnlyRepository
    {
        Task<bool> ExistsDishType(Enums.DishType dishType);
        Task<List<Enums.DishType>> GetExistingDishTypes(IEnumerable<Enums.DishType> dishTypes);
    }
}
