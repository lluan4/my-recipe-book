﻿using AutoMapper;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Response;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Domain.Security.Cryptography;
using MyRecipeBook.Domain.Security.Tokens;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace MyRecipeBook.Application.UseCases.User.Register
{
    public class RegisterUserUseCase : IRegisterUserUseCase
    {
        private readonly IUserWriteOnlyRepository _writeOnlyRepository;
        private readonly IUserReadOnlyRepository _readOnlyRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPasswordEncripter _passwordEncripter;
        private readonly IAccessTokenGenerator _accessTokenGenerator;


        public RegisterUserUseCase(
            IUserWriteOnlyRepository writeOnlyRepository, 
            IUserReadOnlyRepository readOnlyRepository,
            IUnitOfWork unitOfWork,
            IPasswordEncripter passwordEncripter,
            IMapper mapper,
            IAccessTokenGenerator accessTokenGenerator
         )
        {
            _writeOnlyRepository = writeOnlyRepository;
            _readOnlyRepository = readOnlyRepository;
            _mapper = mapper;
            _passwordEncripter = passwordEncripter;
            _unitOfWork = unitOfWork;
            _accessTokenGenerator = accessTokenGenerator;
        }

        public async Task<ResponseRegisteredUserJson> Execute(RequestRegisterUserJson request)
        {

            // Validar Request
            await Validate(request);

            // Mapear a request em uma entidade
            var user = _mapper.Map<Domain.Entities.User>(request);

            // Criptografia da senha
            user.Password = _passwordEncripter.Encrypt(request.Password);
            user.UserIdentifier = Guid.NewGuid();

            // Salvar no banco de dados
            await _writeOnlyRepository.Add(user);

            await _unitOfWork.Commit();

            return new ResponseRegisteredUserJson 
            { 
                Name = user.Name,
                Tokens = new ResponseTokensJson 
                {
                    AccessToken = _accessTokenGenerator.Generate(user.UserIdentifier),
                }
            };
        }
        private async Task Validate(RequestRegisterUserJson request)
        {
            var validator = new RegisterUserValidator();

            var result = validator.Validate(request);

            var emailExists = await _readOnlyRepository.ExistActiveUserWithEmail(request.Email);

            if(emailExists)
            {
                result.Errors.Add(new FluentValidation.Results.ValidationFailure(string.Empty, ResourceMessagesException.EMAIL_ALREADY_REGISTERED));
            }

            if (!result.IsValid)
            {
                var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();
                throw new ErrorOnValidationException(errorMessages);
            }
        }
    }

   
}
