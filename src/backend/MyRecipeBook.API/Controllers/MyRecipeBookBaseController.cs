using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using MyRecipeBook.Domain.Extension;

namespace MyRecipeBook.API.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class MyRecipeBookBaseController:ControllerBase
	{

		protected static bool IsNotAuthenticated(AuthenticateResult authenticate)
		{
			return authenticate.Succeeded.isFalse()
				|| authenticate.Principal is null
				|| authenticate.Principal.Identities.Any(id => id.IsAuthenticated).isFalse();
		}

	}
}
