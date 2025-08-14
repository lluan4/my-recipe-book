using MyRecipeBook.Communication.Request;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Security.Tokens;

namespace MyRecipeBook.Application.UseCases.Token.RefreshToken
{
	public class UseRefreshTokenUseCase :IUseRefreshTokenUseCase
	{
		private readonly ITokenRepository _tokenRepository;
		private readonly IRefreshTokenGenerator _refreshTokenGenerator;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IAccessTokenGenerator _accessTokenGenerator;

		public UseRefreshTokenUseCase(ITokenRepository tokenRepository, IUnitOfWork unitOfWork, IAccessTokenGenerator accessTokenGenerator, IRefreshTokenGenerator refreshTokenGenerator)
		{
			_tokenRepository = tokenRepository;
			_unitOfWork = unitOfWork;
			_accessTokenGenerator = accessTokenGenerator;
			_refreshTokenGenerator = refreshTokenGenerator;
		}

		public async Task Execute(RequestNewTokenJson request)
		{
			var refreshToken = await _tokenRepository.Get(request.RefreshToken);
		}
	}
}
