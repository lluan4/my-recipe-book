using MyRecipeBook.Domain.Security.Tokens;

namespace MyRecipeBook.Infrastructure.Security.Tokens.Refresh
{
	public class RefreshTokenGenerator:IRefreshTokenGenerator
	{
		public String Generate() => Convert.ToBase64String(Guid.NewGuid().ToByteArray());
	}
}
