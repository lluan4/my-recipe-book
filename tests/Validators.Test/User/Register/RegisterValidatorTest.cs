using CommonTestUtilities.Requests;
using MyRecipeBook.Application.UseCases.User.Register;
using MyRecipeBook.Exceptions;
using Shouldly;

namespace Validators.Test.User.Register
{
    public class RegisterValidatorTest
    {
        [Fact]
        public void Success()
        {
            var validator = new RegisterUserValidator();

            var request = RequestRegisterUserJsonBuilder.Build();

            var result = validator.Validate(request);

            result.IsValid.ShouldBeTrue();

        }

        [Fact]
        public void Error_Name_Empty()
        {
            var validator = new RegisterUserValidator();

            var request = RequestRegisterUserJsonBuilder.Build();
            request.Name = String.Empty;

            var result = validator.Validate(request);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldSatisfyAllConditions(
             e => e.ShouldHaveSingleItem(),
             e => e.Single().ErrorMessage.ShouldBe(ResourceMessagesException.NAME_EMPTY)
             );

        }

        [Fact]
        public void Error_Email_Empty()
        {
            var validator = new RegisterUserValidator();

            var request = RequestRegisterUserJsonBuilder.Build();
            request.Email = String.Empty;

            var result = validator.Validate(request);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldSatisfyAllConditions(
             e => e.ShouldHaveSingleItem(),
             e => e.Single().ErrorMessage.ShouldBe(ResourceMessagesException.EMAIL_EMPTY)
             );

        }

        [Fact]
        public void Error_Email_Invalid()
        {
            var validator = new RegisterUserValidator();

            var request = RequestRegisterUserJsonBuilder.Build();
            request.Email = "teste.com";

            var result = validator.Validate(request);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldSatisfyAllConditions(
             e => e.ShouldHaveSingleItem(),
             e => e.Single().ErrorMessage.ShouldBe(ResourceMessagesException.EMAIL_INVALID)
             );

        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        public void Error_Password_Lenght(int passwordLenght)
        {
            var validator = new RegisterUserValidator();

            var request = RequestRegisterUserJsonBuilder.Build(passwordLenght);

            var result = validator.Validate(request);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldHaveSingleItem()
                .ErrorMessage.ShouldBe(ResourceMessagesException.PASSWORD_MUST_BE_LONGER_THAN_6_CHARACTERS);

        }

        [Fact]
        public void Error_Password_Empty()
        {
            var validator = new RegisterUserValidator();

            var request = RequestRegisterUserJsonBuilder.Build();
            request.Password = string.Empty;

            var result = validator.Validate(request);

            result.IsValid.ShouldBeFalse();

            result.Errors.ShouldHaveSingleItem()
                .ErrorMessage.ShouldBe(ResourceMessagesException.PASSWORD_EMPTY);

        }
    }
}
