using AutoMapper;
using MyRecipeBook.Application.Extension.MyRecipeBook.Domain.Extension;
using MyRecipeBook.Communication.Request;
using MyRecipeBook.Communication.Response;
using MyRecipeBook.Domain.Extension;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.CookingTime;
using MyRecipeBook.Domain.Repositories.Difficulty;
using MyRecipeBook.Domain.Repositories.DishType;
using MyRecipeBook.Domain.Repositories.Recipe;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Domain.Services.Storage;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;
using System.IO;

namespace MyRecipeBook.Application.UseCases.Recipe.Register
{
	public class RegisterRecipeUseCase:IRegisterRecipeUseCase
	{
		private readonly IRecipeWriteOnlyRepository _repository;

		private readonly ICookingTimeReadOnlyRepository _repositoryCookingTime;
		private readonly IDifficultyReadOnlyRepository _repositoryDifficultyTime;
		private readonly IDishTypeReadOnlyRepository _repositoryDishType;

		private readonly RecipeRegisterServices _recipeRegisterServices;



		public RegisterRecipeUseCase(
			 IRecipeWriteOnlyRepository repository,
			 ICookingTimeReadOnlyRepository repositoryCookingTime,
			 IDifficultyReadOnlyRepository repositoryDifficultyTime,
			 IDishTypeReadOnlyRepository repositoryDishType,
			 RecipeRegisterServices recipeRegisterServices
			 )
		{
			_repository = repository;
			_repositoryCookingTime = repositoryCookingTime;
			_repositoryDifficultyTime = repositoryDifficultyTime;
			_repositoryDishType = repositoryDishType;
			_recipeRegisterServices = recipeRegisterServices;
		}

		public async Task<ResponseRegisteredRecipeJson> Execute(RequestRegisterRecipeFormData request)
		{
			await ValidateAsync(request);

			var loggedUser = await _recipeRegisterServices._loggedUser.User();

			var recipe = _recipeRegisterServices._mapper.Map<Domain.Entities.Recipe>(request);
			recipe.UserId = loggedUser.Id;

			var instructions = request.Instructions.OrderBy(i => i.Step).ToList();
			for(var i = 0;i < instructions.Count;i++)
				instructions[i].Step = i + 1;

			recipe.Instructions = _recipeRegisterServices._mapper.Map<IList<Domain.Entities.Instruction>>(instructions);

			recipe.RecipeDishTypes = _recipeRegisterServices._mapper.Map<IList<Domain.Entities.RecipeDishType>>(request.DishTypes);

			if(request.Image is not null)
			{
				var fileStream = request.Image.OpenReadStream();

				(var isValidImage, var imageIdentifier) = fileStream.ValidadeAndGetImageIdentifier();

				if(isValidImage.isFalse())
					throw new ErrorOnValidationException([ResourceMessagesException.ONLY_IMAGES_ACCEPTED]);

				recipe.ImageIdentifier = imageIdentifier;

				await _recipeRegisterServices._blobStorageService.Upload(loggedUser, fileStream, recipe.ImageIdentifier);
			}

			await _repository.Add(recipe);

			await _recipeRegisterServices._unitOfWork.Commit();

			return _recipeRegisterServices._mapper.Map<ResponseRegisteredRecipeJson>(recipe);
		}

		private async Task ValidateAsync(RequestRecipeJson request)
		{
			var validator = new RecipeValidator(
				 _repositoryCookingTime,
				 _repositoryDifficultyTime,
				 _repositoryDishType);

			var result = await validator.ValidateAsync(request);

			if(!result.IsValid)
				throw new ErrorOnValidationException(
					 result.Errors.Select(e => e.ErrorMessage).Distinct().ToList());
		}
	}
}
