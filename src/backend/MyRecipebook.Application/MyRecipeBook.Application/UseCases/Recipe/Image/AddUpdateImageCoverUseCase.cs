using FileTypeChecker.Extensions;
using FileTypeChecker.Types;
using Microsoft.AspNetCore.Http;
using MyRecipeBook.Domain.Extension;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.Recipe;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace MyRecipeBook.Application.UseCases.Recipe.Image
{
	public class AddUpdateImageCoverUseCase:IAddUpdateImageCoverUseCase
	{
		private readonly ILoggedUser _loggedUser;
		private readonly IRecipeUpdateOnlyRepository _recipeUpdateOnlyRepository;
		private readonly IUnitOfWork _unitOfWork;

		public AddUpdateImageCoverUseCase(ILoggedUser loggedUser, IRecipeUpdateOnlyRepository recipeUpdateOnlyRepository, IUnitOfWork unitOfWork)
		{
			_loggedUser = loggedUser;
			_recipeUpdateOnlyRepository = recipeUpdateOnlyRepository;
			_unitOfWork = unitOfWork;
		}

		public async Task Execute(long recipeId, IFormFile file)
		{
			var loggedUser = await _loggedUser.User();

			var recipe = await _recipeUpdateOnlyRepository.GetById(user: loggedUser, recipeId);

			if(recipe is null)
				throw new NotFoundException(ResourceMessageHelper.FieldNotFound("recipeId"));

			ValidateFile(file);

		}

		protected void ValidateFile(IFormFile file)
		{
			var fileString = file.OpenReadStream();

			var isInvalidImgType = fileString.Is<PortableNetworkGraphic>().isFalse() && fileString.Is<JointPhotographicExpertsGroup>().isFalse();

			if(isInvalidImgType)
				throw new ErrorOnValidationException([ResourceMessagesException.ONLY_IMAGES_ACCEPTED]);
		}

	}
}
