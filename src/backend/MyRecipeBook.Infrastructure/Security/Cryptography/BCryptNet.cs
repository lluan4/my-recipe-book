using MyRecipeBook.Domain.Security.Cryptography;

namespace MyRecipeBook.Infrastructure.Security.Cryptography
{
	public class BCryptNet:IPasswordEncripter
	{
		public String Encrypt(String password)
		{
			return BCrypt.Net.BCrypt.HashPassword(password);
		}

		public Boolean Isvalid(String password, String hashedPassword)
		{
			return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
		}
	}
}