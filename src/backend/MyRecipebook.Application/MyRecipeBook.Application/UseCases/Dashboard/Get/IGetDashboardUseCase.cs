using MyRecipeBook.Communication.Response;

namespace MyRecipeBook.Application.UseCases.Dashboard.Get
{
    public interface IGetDashboardUseCase
    {
        Task<ResponseRecipesJson> Execute();
    }
}
