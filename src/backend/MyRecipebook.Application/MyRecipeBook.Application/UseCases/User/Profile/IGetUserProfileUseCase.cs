using MyRecipeBook.Communication.Response;

namespace MyRecipeBook.Application.UseCases.User.Profile
{
    public interface IGetUserProfileUseCase
    {
        public Task<ResponseUserProfileJson> Execute();
    }
}
