using MyRecipeBook.Communication.Response;

namespace MyRecipeBook.Application.UseCases.Recipe.Update
{
    public interface IUpdateRecipeUseCase
    {
        Task<ResponseRecipeJson> Execute(long request);
    }
}
