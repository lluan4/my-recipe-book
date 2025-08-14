using MyRecipeBook.Communication.Request;

namespace MyRecipeBook.Application.UseCases.Token.RefreshToken
{
	public interface IUseRefreshTokenUseCase
	{
		Task Execute(RequestNewTokenJson request);

	}
}
