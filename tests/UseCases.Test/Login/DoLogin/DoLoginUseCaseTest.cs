using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using MyRecipeBook.Application.UseCases.Login.DoLogin;
using MyRecipeBook.Communication.Request;

using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;
using Shouldly;

namespace UseCases.Test.Login.DoLogin
{
    public class DoLoginUseCaseTest
    {

        [Fact]
        public async Task Sucess()
        {
           (var user, var password) = UserBuilder.Build();

            var request = new RequestLoginJson { Email = user.Email, Password = password};

            var useCase = CreateUseCase(user);

            var result = await useCase.Execute(request);

            result.ShouldNotBeNull();
            result.Tokens.ShouldNotBeNull();
            result.Name.ShouldBe(user.Name);
            result.Tokens.AccessToken.ShouldNotBeNullOrEmpty();
        }

        [Fact]
        public async Task Error_Invalid_User()
        {
            var request = RequestLoginJsonBuilder.Build();

            var useCase = CreateUseCase();

            Func<Task> act = async () => { await useCase.Execute(request); };

            var ex = await act.ShouldThrowAsync<InvalidLoginException>();

            ex.Message.ShouldBe(ResourceMessagesException.EMAIL_OR_PASSWORD_INVALID);
        }



        private static DoLoginUseCase CreateUseCase(MyRecipeBook.Domain.Entities.User? user = null)
        {
            var passwordEncripter = PasswordEncripterBuilder.Build();
            var userReadOnlyRepositoryBuilder = new UserReadOnlyRepositoryBuilder();
            var accessTokenGenerator = JwtTokensGeneratorBuilder.Build();

            if (user is not null)
                userReadOnlyRepositoryBuilder.GetByEmailAndPassword(user);



            return new DoLoginUseCase(userReadOnlyRepositoryBuilder.Build(), passwordEncripter, accessTokenGenerator);
        }
    }
}
