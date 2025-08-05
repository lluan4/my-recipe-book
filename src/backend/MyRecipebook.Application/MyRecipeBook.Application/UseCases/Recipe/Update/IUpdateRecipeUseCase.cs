using MyRecipeBook.Communication.Request;

namespace MyRecipeBook.Application.UseCases.Recipe.Update
{
    public interface IUpdateRecipeUseCase
    {
        public Task Execute(long recipeId, RequestRecipeJson request);
    }
}
