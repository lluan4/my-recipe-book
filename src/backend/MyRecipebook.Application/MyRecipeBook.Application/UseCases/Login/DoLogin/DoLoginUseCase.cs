using MyRecipeBook.Communication.Request;
using MyRecipeBook.Communication.Response;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Extension;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Domain.Security.Cryptography;
using MyRecipeBook.Domain.Security.Tokens;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace MyRecipeBook.Application.UseCases.Login.DoLogin
{
	public class DoLoginUseCase:IDoLoginUseCase
	{
		private readonly IUserReadOnlyRepository _repository;
		private readonly IPasswordEncripter _passwordEncripter;
		private readonly IAccessTokenGenerator _accessTokenGenerator;

		public DoLoginUseCase(
			 IUserReadOnlyRepository repository,
			 IPasswordEncripter passwordEncripter,
			 IAccessTokenGenerator accessTokenGenerator
			 )
		{
			_repository = repository;
			_passwordEncripter = passwordEncripter;
			_accessTokenGenerator = accessTokenGenerator;
		}

		public async Task<ResponseRegisteredUserJson> Execute(RequestLoginJson request)
		{
			var user = await _repository.GetByEmail(request.Email);

			if (user is null || _passwordEncripter.Isvalid(request.Password, user.Password).isFalse()) 
				throw new InvalidLoginException();


			return new ResponseRegisteredUserJson
			{
				Name = user.Name,
				Tokens = new ResponseTokensJson
				{
					AccessToken = _accessTokenGenerator.Generate(user.UserIdentifier),
				}
			};
		}
	}
}
