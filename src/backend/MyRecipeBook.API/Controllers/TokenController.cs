using Microsoft.AspNetCore.Mvc;
using MyRecipeBook.Application.UseCases.Token.RefreshToken;
using MyRecipeBook.Communication.Request;
using MyRecipeBook.Communication.Response;
using MyRecipeBook.Domain.ValueObjects;
using Swashbuckle.AspNetCore.Annotations;

namespace MyRecipeBook.API.Controllers
{
	public class TokenController:MyRecipeBookBaseController
	{
		[HttpPost("refresh-token")]
		[SwaggerOperation(
		  Summary = "Gera Refresh Token",
		  Description = "Cria uma nova receita para o usuário autenticado",
		  OperationId = "CreateRecipe"
		)]
		[ProducesResponseType(typeof(ResponseTokensJson), StatusCodes.Status200OK)]

		public async Task<IActionResult> RefreshToken(
		  [FromServices] IUseRefreshTokenUseCase useCase
		)
		{
			var refreshToken = Request.Cookies["refreshToken"];

			if(string.IsNullOrEmpty(refreshToken))
				return Unauthorized("Refresh token not found");

			var request = new RequestNewTokenJson { RefreshToken = refreshToken };

			var response = await useCase.Execute(request);

			var cookieOptions = new CookieOptions
			{
				HttpOnly = true,
				Secure = true,
				SameSite = SameSiteMode.Strict,
				Expires = DateTime.UtcNow.AddDays(MyRecipeBookRuleConstants.REFRESH_TOKEN_EXPIRATION)
			};

			Response.Cookies.Append("refreshToken", response.RefreshToken, cookieOptions);

			response = new ResponseTokensJson
			{
				AccessToken = response.AccessToken,
			};

			return Ok(response);
		}
	}
}
