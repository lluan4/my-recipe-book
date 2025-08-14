using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using MyRecipeBook.Application.UseCases.Login.DoLogin;
using MyRecipeBook.Application.UseCases.Login.External;
using MyRecipeBook.Communication.Request;
using MyRecipeBook.Communication.Response;
using MyRecipeBook.Communication.Responses;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;

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

		[HttpGet]
		[Route("google")]
		public async Task<IActionResult> LoginGoogle(
			string returnUrl,
			[FromServices] IExternalLoginUseCase useCase
			)
		{
			var authenticate = await Request.HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);

			if(IsNotAuthenticated(authenticate))
			{
				return Challenge(GoogleDefaults.AuthenticationScheme);
			}
			else
			{
				var claims = authenticate.Principal!.Identities.First().Claims;

				var name = claims.First(c => c.Type == ClaimTypes.Name).Value;

				var email = claims.First(c => c.Type == ClaimTypes.Email).Value;

				var token = await useCase.Execute(name, email);

				return Redirect($"{returnUrl}{token}");
			}
		}

	}
}