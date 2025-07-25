namespace MyRecipeBook.Domain.Security.Tokens
{
    public interface IAccessTokenValidator
    {
        public Guid ValidadeAndGetUserIdentifier(string token);
    }
}
