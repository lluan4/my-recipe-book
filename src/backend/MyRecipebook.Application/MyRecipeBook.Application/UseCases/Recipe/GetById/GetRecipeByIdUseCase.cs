using AutoMapper;
using MyRecipeBook.Communication.Response;
using MyRecipeBook.Domain.Extension;
using MyRecipeBook.Domain.Repositories.Recipe;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Domain.Services.Storage;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace MyRecipeBook.Application.UseCases.Recipe.GetById
{
	public class GetRecipeByIdUseCase : IGetRecipeByIdUseCase
	{
		private readonly IMapper _mapper;
		private readonly ILoggedUser _loggedUSer;
		private readonly IRecipeReadOnlyRepository _repository;
		private readonly IBlobStorageService _blobStorageService;

		public GetRecipeByIdUseCase(
			IMapper mapper,
			ILoggedUser loggedUser, 
			IRecipeReadOnlyRepository repository,
			IBlobStorageService blobStorageService
			)
			{
				_mapper = mapper;
				_loggedUSer = loggedUser;
				_repository = repository;
				_blobStorageService = blobStorageService;
			}

		public async Task<ResponseRecipeJson> Execute(long recipeId)
		{
			var loggedUser = await _loggedUSer.User();

			var recipe = await _repository.GetById(loggedUser, recipeId);

			if (recipe is null)
					throw new NotFoundException(ResourceMessageHelper.FieldNotFound("Recipe"));

			var response = _mapper.Map<ResponseRecipeJson>(recipe);

			if(recipe.ImageIdentifier.NotEmpty())
			{
				var imageUrl = await _blobStorageService.GetFileUrl(loggedUser, recipe.ImageIdentifier);
				response.ImageUrl = imageUrl;
			}

			return response;
		}
	}
}
