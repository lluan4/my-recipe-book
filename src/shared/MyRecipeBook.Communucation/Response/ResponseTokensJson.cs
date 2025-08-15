using System.Text.Json.Serialization;

namespace MyRecipeBook.Communication.Response
{
	public class ResponseTokensJson
	{
		public string AccessToken { get; set; } = string.Empty;
		[JsonIgnore]
		public string RefreshToken { get; set; } = string.Empty;
	}
}
