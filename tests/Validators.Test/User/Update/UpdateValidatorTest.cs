using CommonTestUtilities.Requests;
using MyRecipeBook.Application.UseCases.User.Register;
using MyRecipeBook.Application.UseCases.User.Update;
using MyRecipeBook.Exceptions;
using Shouldly;

namespace Validators.Test.User.Update
{
    public class UpdateValidatorTest
    {
        [Fact]
        public void Sucess()
        {
            var validator = new UpdateUserUseValidator();

            var request = RequestUpdateUserJsonBuilder.Build();

            var result = validator.Validate(request);

            result.IsValid.ShouldBeTrue();

        }

        [Fact]
        public void Error_Name_Empty()
        {
            var validator = new UpdateUserUseValidator();

            var request = RequestUpdateUserJsonBuilder.Build();
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
            var validator = new UpdateUserUseValidator();

            var request = RequestUpdateUserJsonBuilder.Build();
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
            var validator = new UpdateUserUseValidator();

            var request = RequestUpdateUserJsonBuilder.Build();
            request.Email = "teste.com";

            var result = validator.Validate(request);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldSatisfyAllConditions(
             e => e.ShouldHaveSingleItem(),
             e => e.Single().ErrorMessage.ShouldBe(ResourceMessagesException.EMAIL_INVALID)
             );

        }
    }
}
