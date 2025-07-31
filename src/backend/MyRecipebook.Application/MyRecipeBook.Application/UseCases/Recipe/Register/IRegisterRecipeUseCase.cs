using MyRecipeBook.Communication.Request;
using MyRecipeBook.Communication.Response;

namespace MyRecipeBook.Application.UseCases.Recipe.Register
{
    public interface IRegisterRecipeUseCase
    {
        public Task<ResponseRegisteredRecipeJson> Execute(RequestRecipeJson request);
    }
}
