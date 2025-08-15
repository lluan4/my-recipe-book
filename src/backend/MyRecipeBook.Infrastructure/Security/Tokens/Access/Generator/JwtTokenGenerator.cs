using Microsoft.IdentityModel.Tokens;
using MyRecipeBook.Domain.Security.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MyRecipeBook.Infrastructure.Security.Tokens.Access.Generator
{
	public class JwtTokenGenerator(uint expirationTimeMinutes, string signingKey):JwtTokenHandler, IAccessTokenGenerator
	{
		private readonly uint _expirationTimeMinutes = expirationTimeMinutes;
		private readonly string _signingKey = signingKey;

		public string Generate(Guid userIdentifier, string? refreshTokenId)
		{

			var claims = new List<Claim>()
				{
					new(ClaimTypes.Sid, userIdentifier.ToString()),
					new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
					new(JwtRegisteredClaimNames.Iat,
						DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(),
						ClaimValueTypes.Integer64)
				};

			if(!string.IsNullOrEmpty(refreshTokenId))
			{
				claims.Add(new Claim("refresh_token_id", refreshTokenId));
			}

			claims.Add(new Claim(JwtRegisteredClaimNames.Iat,
				DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(),
				ClaimValueTypes.Integer64));

			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(claims),
				Expires = DateTime.UtcNow.AddMinutes(_expirationTimeMinutes),
				IssuedAt = DateTime.UtcNow,
				SigningCredentials = new SigningCredentials(SecurityKey(_signingKey), SecurityAlgorithms.HmacSha256Signature),
			};

			var tokenHandler = new JwtSecurityTokenHandler();

			var securityToken = tokenHandler.CreateToken(tokenDescriptor);

			return tokenHandler.WriteToken(securityToken);
		}


	}
}
