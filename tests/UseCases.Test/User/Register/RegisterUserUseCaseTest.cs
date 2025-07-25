using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using MyRecipeBook.Application.UseCases.User.Register;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;
using Shouldly;

namespace UseCases.Test.User.Register
{
    public class UpdateUserUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            var request = RequestRegisterUserJsonBuilder.Build();

            var useCase = CreateUseCase();

            var result = await useCase.Execute(request);
             
            result.ShouldNotBeNull();
            result.Tokens.ShouldNotBeNull();
            result.Name.ShouldBe(request.Name);
            result.Tokens.AccessToken.ShouldNotBeNullOrEmpty();
        }

        [Fact]
        public async Task Error_Email_Already_Registered()
        {
            var request = RequestRegisterUserJsonBuilder.Build();

            var useCase = CreateUseCase(request.Email);

            Func<Task> act = async () => await useCase.Execute(request);

            var ex = await Should.ThrowAsync<ErrorOnValidationException>(act);
            ex.ErrorMessages.ShouldSatisfyAllConditions(
                list => list.Count.ShouldBe(1),
                list => list.ShouldContain(ResourceMessagesException.EMAIL_ALREADY_REGISTERED)
            );
        }

        [Fact]
        public async Task Error_Name_Empty()
        {
            var request = RequestRegisterUserJsonBuilder.Build();
            request.Name = string.Empty;

            var useCase = CreateUseCase();

            Func<Task> act = async () => await useCase.Execute(request);

            var ex = await Should.ThrowAsync<ErrorOnValidationException>(act);
            ex.ErrorMessages.ShouldSatisfyAllConditions(
                list => list.Count.ShouldBe(1),
                list => list.ShouldContain(ResourceMessagesException.NAME_EMPTY)
            );
        }

        private static RegisterUserUseCase CreateUseCase(string? email = null)
        {
            var mapper = MapperBuilder.Build();
            var passwordEncripter = PasswordEncripterBuilder.Build();
            var writeRepository = UserWriteOnlyRepositoryBuilder.Build();
            var unitOfWork = UnitOfWorkBuilder.Build();
            var readRepositoryBuilder = new UserReadOnlyRepositoryBuilder();
            var accessTokenGenerator = JwtTokensGeneratorBuilder.Build();

            if (string.IsNullOrEmpty(email) == false)
                readRepositoryBuilder.ExistActiveUserWithEmail(email);

            return new RegisterUserUseCase(writeRepository, readRepositoryBuilder.Build(), unitOfWork, passwordEncripter, mapper, accessTokenGenerator);
        }
    }
}
