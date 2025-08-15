using MyRecipeBook.Domain.Repositories.Token;
using MyRecipeBook.Domain.Security.Cryptography;
using MyRecipeBook.Domain.Security.Tokens;

namespace MyRecipeBook.Application.UseCases.User.Register
{
	public class RegisterServices
	{
		public IPasswordEncripter _passwordEncripter { get; init; }
		public IAccessTokenGenerator _accessTokenGenerator { get; init; }
		public IRefreshTokenGenerator _refreshTokenGenerator { get; init; }
		public ITokenRepository _tokenRepository { get; init; }

		public RegisterServices(
			IPasswordEncripter passwordEncripter,
			Domain.Repositories.User.IUserReadOnlyRepository userReadOnlyRepository,
			IAccessTokenGenerator accessTokenGenerator,
			IRefreshTokenGenerator refreshTokenGenerator,
			ITokenRepository tokenRepository,
			Domain.Repositories.IUnitOfWork unitOfWork)
		{
			_passwordEncripter = passwordEncripter;
			_accessTokenGenerator = accessTokenGenerator;
			_refreshTokenGenerator = refreshTokenGenerator;
			_tokenRepository = tokenRepository;
		}
	}
}
