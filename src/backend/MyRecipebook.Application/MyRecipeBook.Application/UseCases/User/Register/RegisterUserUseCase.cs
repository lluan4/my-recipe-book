using AutoMapper;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Response;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace MyRecipeBook.Application.UseCases.User.Register
{
	public class RegisterUserUseCase:IRegisterUserUseCase
	{
		private readonly IUserWriteOnlyRepository _writeOnlyRepository;
		private readonly IUserReadOnlyRepository _readOnlyRepository;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		private readonly RegisterServices _registerServices;

		public RegisterUserUseCase(
			 IUserWriteOnlyRepository writeOnlyRepository,
			 IUserReadOnlyRepository readOnlyRepository,
			 IUnitOfWork unitOfWork,
			 IMapper mapper,
			 RegisterServices registerServices
		 )
		{
			_writeOnlyRepository = writeOnlyRepository;
			_readOnlyRepository = readOnlyRepository;
			_mapper = mapper;
			_unitOfWork = unitOfWork;
			_registerServices = registerServices;
		}

		public async Task<ResponseRegisteredUserJson> Execute(RequestRegisterUserJson request)
		{

			// Validar Request
			await Validate(request);

			// Mapear a request em uma entidade
			var user = _mapper.Map<Domain.Entities.User>(request);

			// Criptografia da senha
			user.Password = _registerServices._passwordEncripter.Encrypt(request.Password);
			user.UserIdentifier = Guid.NewGuid();

			// Salvar no banco de dados
			await _writeOnlyRepository.Add(user);

			await _unitOfWork.Commit();

			var refreshToken = await CreateAndSaveRefreshToken(user);

			return new ResponseRegisteredUserJson
			{
				Name = user.Name,
				Tokens = new ResponseTokensJson
				{
					AccessToken = _registerServices._accessTokenGenerator.Generate(user.UserIdentifier, refreshToken),
				}
			};
		}

		private async Task<String> CreateAndSaveRefreshToken(Domain.Entities.User user)
		{
			var refreshToken = new RefreshToken
			{
				Value = _registerServices._refreshTokenGenerator.Generate(),
				UserId = user.Id,
			};

			await _registerServices._tokenRepository.SaveNewRefreshToken(refreshToken);

			await _unitOfWork.Commit();

			return refreshToken.Value;
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

			if(!result.IsValid)
			{
				var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();
				throw new ErrorOnValidationException(errorMessages);
			}
		}
	}


}
