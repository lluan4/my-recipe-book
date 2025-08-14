using Microsoft.EntityFrameworkCore;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Repositories.Token;

namespace MyRecipeBook.Infrastructure.DataAccess.Repositories
{
	public class TokenRepository:ITokenRepository
	{
		private readonly MyRecipeBookDbContext _dbContext;

		public TokenRepository(MyRecipeBookDbContext dbContext) => _dbContext = dbContext;
		public async Task<RefreshToken?> Get(String refreshToken)
		{
			return await _dbContext
				.RefreshToken
				.AsNoTracking()
				.Include(token => token.User)
				.FirstOrDefaultAsync(t => t.Value.Equals(refreshToken));


		}
	}
}
