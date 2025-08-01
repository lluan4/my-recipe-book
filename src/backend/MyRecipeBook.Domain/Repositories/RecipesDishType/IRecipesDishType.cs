namespace MyRecipeBook.Domain.Repositories.RecipesDishType
{
    public interface IRecipesDishTypeWriteOnlyRepository
    {
        Task Add(Entities.RecipeDishType entity);
        Task AddRange(IList<Entities.RecipeDishType> entities);
    }

}
