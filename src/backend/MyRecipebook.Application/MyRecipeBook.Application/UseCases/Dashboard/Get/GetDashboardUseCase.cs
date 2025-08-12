using AutoMapper;
using MyRecipeBook.Application.Extension;
using MyRecipeBook.Communication.Response;
using MyRecipeBook.Domain.Repositories.Recipe;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Domain.Services.Storage;

namespace MyRecipeBook.Application.UseCases.Dashboard.Get
{
	public class GetDashboardUseCase:IGetDashboardUseCase
	{
		private readonly IRecipeReadOnlyRepository _recipeReadOnlyRepository;
		private readonly IMapper _mapper;
		private readonly ILoggedUser _loggedUser;
		private readonly IBlobStorageService _blobStorageService;

		public GetDashboardUseCase(IRecipeReadOnlyRepository recipeReadOnlyRepository, IMapper mapper, ILoggedUser loggedUser, IBlobStorageService blobStorageService)
		{
			_recipeReadOnlyRepository = recipeReadOnlyRepository;
			_mapper = mapper;
			_loggedUser = loggedUser;
			_blobStorageService = blobStorageService;
		}

		public async Task<ResponseRecipesJson> Execute()
		{
			var loggedUser = await _loggedUser.User();

			var recipes = await _recipeReadOnlyRepository.GetForDashboard(user: loggedUser);

			return new ResponseRecipesJson
			{
				Recipes = await recipes.MapToShortRecipeJson(loggedUser, _blobStorageService, _mapper)
			};
		}
	}
}
