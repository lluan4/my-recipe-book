


using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using MyRecipeBook.Application.UseCases.User.ChangePassword;
using MyRecipeBook.Communication.Request;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;
using Shouldly;


namespace UseCases.Test.User.ChangePassword;


public class ChangePasswordUseCaseTest
{
    [Fact]
    public async Task Success()
    {

        (var user, var password) = UserBuilder.Build();

        var request = RequestChangePasswordJsonBuilder.Build();
        request.Password = password;

        var useCase = CreateUseCase(user);

        Func<Task> act = async () => await useCase.Execute(request);

        await act.ShouldNotThrowAsync();

        var passwordEncripter = PasswordEncripterBuilder.Build();

        user.Password.ShouldBe(passwordEncripter.Encrypt(request.NewPassword));
    }

    [Fact]
    public async Task Error_NewPassword_Empty()
    {

        (var user, var password) = UserBuilder.Build();

        var request = new RequestChangePasswordJson
        {
            Password = password,
            NewPassword = String.Empty
        };


        var useCase = CreateUseCase(user);

        Func<Task> act = async () =>{ await useCase.Execute(request); };

        var exception = await act.ShouldThrowAsync<ErrorOnValidationException>();

        exception.ErrorMessages.ShouldHaveSingleItem();
        exception.ErrorMessages.ShouldContain(ResourceMessagesException.PASSWORD_EMPTY);

        var passwordEncripter = PasswordEncripterBuilder.Build();

        user.Password.ShouldBe(passwordEncripter.Encrypt(password));

    }
    [Fact]
    public async Task Error_CurrentPassword_Different()
    {

        (var user, var password) = UserBuilder.Build();

        var request = RequestChangePasswordJsonBuilder.Build();

        var useCase = CreateUseCase(user);

        Func<Task> act = async () =>{ await useCase.Execute(request); };

        var exception = await act.ShouldThrowAsync<ErrorOnValidationException>();

        exception.ErrorMessages.ShouldHaveSingleItem();
        exception.ErrorMessages.ShouldContain(ResourceMessagesException.PASSWORD_DIFFERENT_CURRENT_PASSWORD);

        var passwordEncripter = PasswordEncripterBuilder.Build();

        user.Password.ShouldBe(passwordEncripter.Encrypt(password));

    }

    private static ChangePasswordUseCase CreateUseCase(MyRecipeBook.Domain.Entities.User user)
    {
        var loggedUser = LoggedUserBuilder.Build(user);
        var userUpdateRepository = new UserUpdateOnlyRepositoryBuilder().GetById(user).Build();
        var unitOfWork = UnitOfWorkBuilder.Build();
        var passwordEncripter = PasswordEncripterBuilder.Build();

        return new ChangePasswordUseCase(loggedUser, userUpdateRepository, unitOfWork, passwordEncripter);
    }


}
