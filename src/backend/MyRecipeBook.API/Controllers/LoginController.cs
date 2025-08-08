using Microsoft.AspNetCore.Mvc;
using MyRecipeBook.Application.UseCases.Login.DoLogin;
using MyRecipeBook.Communication.Request;
using MyRecipeBook.Communication.Response;
using MyRecipeBook.Communication.Responses;
using Swashbuckle.AspNetCore.Annotations;

namespace MyRecipeBook.API.Controllers
{
	[Tags("Authentication")]
	public class LoginController:MyRecipeBookBaseController
	{
		[HttpPost]
		[SwaggerOperation(
			 Description = "Autentica um usuário no sistema usando email e senha",
			 OperationId = "LoginUser"
		)]
		[ProducesResponseType(typeof(ResponseRegisteredUserJson), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> Login(
			 [FromServices] IDoLoginUseCase useCase,
			 [FromBody] RequestLoginJson request
		)
		{
			var response = await useCase.Execute(request);
			return Ok(response);
		}
	}
}