using MyRecipeBook.Communication.Request;
using MyRecipeBook.Communication.Response;

namespace MyRecipeBook.Application.UseCases.Recipe.Generate
{
    public interface IGeneratedRecipeUseCase
    {
        Task<ResponseGeneratedRecipeJson> Execute(RequestGenerateRecipeJson request);
    }
}
