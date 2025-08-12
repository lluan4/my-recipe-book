using FileTypeChecker.Extensions;
using FileTypeChecker.Types;
using Microsoft.AspNetCore.Http;
using MyRecipeBook.Domain.Extension;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.Recipe;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Domain.Services.Storage;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace MyRecipeBook.Application.UseCases.Recipe.Image
{
	public class AddUpdateImageCoverUseCase:IAddUpdateImageCoverUseCase
	{
		private readonly ILoggedUser _loggedUser;
		private readonly IRecipeUpdateOnlyRepository _recipeUpdateOnlyRepository;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IBlobStorageService _blobStorageService;

		public AddUpdateImageCoverUseCase(
			ILoggedUser loggedUser,
			IRecipeUpdateOnlyRepository recipeUpdateOnlyRepository,
			IUnitOfWork unitOfWork,
			IBlobStorageService blobStorageService
			)
		{
			_loggedUser = loggedUser;
			_recipeUpdateOnlyRepository = recipeUpdateOnlyRepository;
			_unitOfWork = unitOfWork;
			_blobStorageService = blobStorageService;
		}

		public async Task Execute(long recipeId, IFormFile file)
		{
			var loggedUser = await _loggedUser.User();

			var recipe = await _recipeUpdateOnlyRepository.GetById(user: loggedUser, recipeId);

			if(recipe is null)
				throw new NotFoundException(ResourceMessageHelper.FieldNotFound("recipeId"));

			var fileString = file.OpenReadStream();

			ValidateFile(fileString);

			if(string.IsNullOrEmpty(recipe.ImageIdentifier))
			{
				recipe.ImageIdentifier = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

				_recipeUpdateOnlyRepository.Update(recipe);

				await _unitOfWork.Commit();

			}

			await _blobStorageService.Upload(loggedUser, fileString, recipe.ImageIdentifier);

		}

		protected static void ValidateFile(Stream fileString)
		{
			var isInvalidImgType = fileString.Is<PortableNetworkGraphic>().isFalse() && fileString.Is<JointPhotographicExpertsGroup>().isFalse();

			if(isInvalidImgType)
				throw new ErrorOnValidationException([ResourceMessagesException.ONLY_IMAGES_ACCEPTED]);

			fileString.Position = 0;
		}

	}
}
