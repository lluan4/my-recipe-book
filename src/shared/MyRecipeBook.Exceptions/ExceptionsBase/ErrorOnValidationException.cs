using System.Net;

namespace MyRecipeBook.Exceptions.ExceptionsBase
{
    public class ErrorOnValidationException : MyRecipeBookException
    {
        private readonly IList<string> ErrorMessages;

        public ErrorOnValidationException(IList<string> errors) : base(string.Empty)
        {
            ErrorMessages = errors;
        }

        public override IList<string> GetErrorMessages() => ErrorMessages;

        public override HttpStatusCode GetStatusCode() => HttpStatusCode.BadRequest;
    }
}
