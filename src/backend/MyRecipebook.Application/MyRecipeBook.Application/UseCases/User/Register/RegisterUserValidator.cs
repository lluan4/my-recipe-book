﻿using FluentValidation;
using MyRecipeBook.Application.SharedValidators;
using MyRecipeBook.Communication.Request;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Domain.Extension;
using MyRecipeBook.Exceptions;

namespace MyRecipeBook.Application.UseCases.User.Register
{
    public class RegisterUserValidator : AbstractValidator<RequestRegisterUserJson>
    {
        public RegisterUserValidator()
        {
            RuleFor(user => user.Name).NotEmpty().WithMessage(ResourceMessagesException.NAME_EMPTY);
            RuleFor(user => user.Email).NotEmpty().WithMessage(ResourceMessagesException.EMAIL_EMPTY);
            RuleFor(user => user.Password).SetValidator(new PasswordValidator<RequestRegisterUserJson>());
            When(user => user.Email.NotEmpty(), () =>
            {
               RuleFor(user => user.Email)
                .EmailAddress()
                .WithMessage(ResourceMessagesException.EMAIL_INVALID);
            });
        }
    }
}
