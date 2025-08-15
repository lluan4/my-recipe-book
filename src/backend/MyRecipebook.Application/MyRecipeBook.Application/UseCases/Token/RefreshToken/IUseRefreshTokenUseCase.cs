using MyRecipeBook.Communication.Request;
using MyRecipeBook.Communication.Response;

namespace MyRecipeBook.Application.UseCases.Token.RefreshToken
{
	public interface IUseRefreshTokenUseCase
	{
		Task<ResponseTokensJson> Execute(RequestNewTokenJson request);

	}
}
