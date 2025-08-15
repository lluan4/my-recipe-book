using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MyRecipeBook.API.Controllers
{
	public class LogoutController:MyRecipeBookBaseController
	{
		[HttpPost("logout")]
		[SwaggerOperation(Summary = "Logout", Description = "Remove o refresh token cookie")]
		[ProducesResponseType<int>(StatusCodes.Status200OK)]
		public IActionResult Logout()
		{
			Response.Cookies.Delete("refreshToken");
			return Ok(new { message = "Logged out successfully" });
		}
	}
}
