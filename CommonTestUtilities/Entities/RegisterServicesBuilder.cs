using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Tokens;
using MyRecipeBook.Application.UseCases.User.Register;

namespace CommonTestUtilities.Entities
{
	public class RegisterServicesBuilder
	{
		public static RegisterServices Build()
		{
			var passwordEncripter = PasswordEncripterBuilder.Build();
			var userReadOnlyRepositoryBuilder = new UserReadOnlyRepositoryBuilder();
			var accessTokenGenerator = JwtTokensGeneratorBuilder.Build();
			var refreshTokenGenerator = RefreshTokenGeneratorBuilder.Build();
			var tokenRepository = new TokenRepositoryBuilder().Build();
			var unitOfWork = UnitOfWorkBuilder.Build();

			return new RegisterServices(
				passwordEncripter,
				userReadOnlyRepositoryBuilder.Build(),
				accessTokenGenerator,
				refreshTokenGenerator,
				tokenRepository,
				unitOfWork
			);
		}
	}
}
