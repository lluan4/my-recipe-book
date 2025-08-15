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
				.Include(t => t.User)
				.FirstOrDefaultAsync(t => t.Value.Equals(refreshToken));
		}

		public async Task SaveNewRefreshToken(RefreshToken refreshToken)
		{
			var tokens = _dbContext
				.RefreshToken
				.Where(t => t.UserId == refreshToken.UserId);

			_dbContext.RefreshToken.RemoveRange(tokens);

			await _dbContext.RefreshToken.AddAsync(refreshToken);
		}
	}
}
