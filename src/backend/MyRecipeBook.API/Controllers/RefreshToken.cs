using Microsoft.AspNetCore.Mvc;
using MyRecipeBook.Application.UseCases.Token.RefreshToken;
using MyRecipeBook.Communication.Request;
using MyRecipeBook.Communication.Response;
using Swashbuckle.AspNetCore.Annotations;

namespace MyRecipeBook.API.Controllers
{
	public class RefreshToken
	{
		[HttpPost("refresh-token")]
		[SwaggerOperation(
		  Summary = "Gera Refresh Token",
		  Description = "Cria uma nova receita para o usuário autenticado",
		  OperationId = "CreateRecipe"
		)]
		[ProducesResponseType(typeof(ResponseTokensJson), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status401Unauthorized)]
		public async Task<IActionResult> Get(
		  [FromServices] IUseRefreshTokenUseCase useCase,
		  [FromBody] RequestNewTokenJson request
		)
		{
			await useCase.Execute(request);

			return Ok(response);
		}
	}
}
