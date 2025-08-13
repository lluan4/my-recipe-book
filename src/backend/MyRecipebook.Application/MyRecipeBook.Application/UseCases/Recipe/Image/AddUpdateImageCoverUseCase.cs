using Microsoft.AspNetCore.Http;
using MyRecipeBook.Application.Extension.MyRecipeBook.Domain.Extension;
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

			var fileStream = file.OpenReadStream();

			(var isValidImage, var imageIdentifier) = fileStream.ValidadeAndGetImageIdentifier();

			if(isValidImage.isFalse())
				throw new ErrorOnValidationException([ResourceMessagesException.ONLY_IMAGES_ACCEPTED]);


			if(string.IsNullOrEmpty(recipe.ImageIdentifier))
			{
				recipe.ImageIdentifier = imageIdentifier;

				_recipeUpdateOnlyRepository.Update(recipe);

				await _unitOfWork.Commit();

			}

			await _blobStorageService.Upload(loggedUser, fileStream, recipe.ImageIdentifier);

		}

	}
}
