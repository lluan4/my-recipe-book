using Microsoft.AspNetCore.Http;
using MyRecipeBook.Communication.Request;
using MyRecipeBook.Communication.Response;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Extension;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.Token;
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
		private readonly IRefreshTokenGenerator _refreshTokenGenerator;
		private readonly ITokenRepository _tokenRepository;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public DoLoginUseCase(
			IUserReadOnlyRepository repository,
			IPasswordEncripter passwordEncripter,
			IAccessTokenGenerator accessTokenGenerator,
			IRefreshTokenGenerator refreshTokenGenerator,
			ITokenRepository tokenRepository,
			IUnitOfWork unitOfWork,
			IHttpContextAccessor httpContextAccessor
			 )
		{
			_repository = repository;
			_passwordEncripter = passwordEncripter;
			_accessTokenGenerator = accessTokenGenerator;
			_refreshTokenGenerator = refreshTokenGenerator;
			_tokenRepository = tokenRepository;
			_unitOfWork = unitOfWork;
			_httpContextAccessor = httpContextAccessor;
		}

		public async Task<ResponseRegisteredUserJson> Execute(RequestLoginJson request)
		{
			var user = await _repository.GetByEmail(request.Email);

			if(user is null || _passwordEncripter.Isvalid(request.Password, user.Password).isFalse())
				throw new InvalidLoginException();

			var refreshToken = await CreateAndSaveRefreshToken(user);

			SetRefreshTokenCookie(refreshToken);

			return new ResponseRegisteredUserJson
			{
				Name = user.Name,
				Tokens = new ResponseTokensJson
				{
					AccessToken = _accessTokenGenerator.Generate(user.UserIdentifier, refreshToken)
				}
			};
		}

		private async Task<String> CreateAndSaveRefreshToken(Domain.Entities.User user)
		{
			var refreshToken = new RefreshToken
			{
				Value = _refreshTokenGenerator.Generate(),
				UserId = user.Id,
			};

			await _tokenRepository.SaveNewRefreshToken(refreshToken);

			await _unitOfWork.Commit();

			return refreshToken.Value;
		}

		private void SetRefreshTokenCookie(string refreshToken)
		{
			var cookieOptions = new CookieOptions
			{
				HttpOnly = true,
				Secure = true,
				SameSite = SameSiteMode.Strict,
				Expires = DateTime.UtcNow.AddDays(30)
			};

			_httpContextAccessor.HttpContext?.Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
		}
	}
}
