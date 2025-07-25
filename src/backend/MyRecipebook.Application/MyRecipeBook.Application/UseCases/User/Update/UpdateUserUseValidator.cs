using FluentValidation;
using MyRecipeBook.Communication.Request;
using MyRecipeBook.Domain.Extension;
using MyRecipeBook.Exceptions;

namespace MyRecipeBook.Application.UseCases.User.Update
{
    public class UpdateUserUseValidator : AbstractValidator<RequestUpdateUserJson>
    {
        public UpdateUserUseValidator()
        {
            RuleFor(request => request.Name).NotEmpty().WithMessage(ResourceMessagesException.NAME_EMPTY);
            RuleFor(request => request.Email).NotEmpty().WithMessage(ResourceMessagesException.EMAIL_EMPTY);

            When(request => string.IsNullOrWhiteSpace(request.Email).isFalse(), () =>
            {
                RuleFor(request => request.Email).EmailAddress().WithMessage(ResourceMessagesException.EMAIL_INVALID);
            });
        }
    }
}
