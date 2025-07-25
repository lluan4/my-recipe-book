using Bogus;
using MyRecipeBook.Communication.Request;

namespace CommonTestUtilities.Requests
{
    public class RequestChangePasswordJsonBuilder
    {

        public static RequestChangePasswordJson Build(int passwordLenght = 10) 
        {
            return new Faker<RequestChangePasswordJson>()
               .RuleFor(user => user.Password, (f) => f.Internet.Password())
               .RuleFor(user => user.NewPassword, (f) => f.Internet.Password(passwordLenght));
        }
    }
}
