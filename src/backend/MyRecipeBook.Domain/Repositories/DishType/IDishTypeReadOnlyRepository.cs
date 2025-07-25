using MyRecipeBook.Domain.Enums;

namespace MyRecipeBook.Domain.Repositories.DishType
{
    public interface IDishTypeReadOnlyRepository
    {
        Task<bool> ExistsDishType(DishTypeEnum dishType);
        Task<List<DishTypeEnum>> GetExistingDishTypes(IEnumerable<DishTypeEnum> dishTypes);
    }
}
