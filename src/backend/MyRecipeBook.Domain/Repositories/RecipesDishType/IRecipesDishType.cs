namespace MyRecipeBook.Domain.Repositories.RecipesDishType
{
    public interface IRecipesDishTypeWriteOnlyRepository
    {
        Task Add(Entities.RecipesDishTypes entity);
        Task AddRange(IList<Entities.RecipesDishTypes> entities);
    }

}
