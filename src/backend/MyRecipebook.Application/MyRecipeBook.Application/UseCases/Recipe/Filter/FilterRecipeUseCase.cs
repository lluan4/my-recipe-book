using AutoMapper;
using MyRecipeBook.Application.Extension;
using MyRecipeBook.Communication.Request;
using MyRecipeBook.Communication.Response;
using MyRecipeBook.Domain.Extension;
using MyRecipeBook.Domain.Repositories.Recipe;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Domain.Services.Storage;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace MyRecipeBook.Application.UseCases.Recipe.Filter
{
	public class FilterRecipeUseCase:IFilterRecipeUseCase
	{
		private readonly IMapper _mapper;
		private readonly ILoggedUser _loggedUser;
		private readonly IRecipeReadOnlyRepository _repository;
		private readonly IBlobStorageService _blobStorageService;

		public FilterRecipeUseCase(IMapper mapper, ILoggedUser loggedUser, IRecipeReadOnlyRepository repository, IBlobStorageService blobStorageService)
		{
			_mapper = mapper;
			_loggedUser = loggedUser;
			_repository = repository;
			_blobStorageService = blobStorageService;
		}

		public async Task<ResponseRecipesJson> Execute(RequestFilterRecipeJson request)
		{
			Validate(request);

			var loggedUser = await _loggedUser.User();

			var filters = new Domain.Dtos.Recipes.FilterRecipesDto
			{
				RecipeTitle_Ingredient = request.RecipeTitle_Ingredient,
				CookingTimes = request.CookingTimes.Distinct().Select(c => (Domain.Enums.RecipeCookingTime) c).ToList(),
				Difficulties = request.Difficulties.Distinct().Select(c => (Domain.Enums.RecipeDifficulty) c).ToList(),
				DishTypes = request.DishTypes.Distinct().Select(c => (Domain.Enums.RecipeDishType) c).ToList(),
			};

			var recipes = await _repository.Filter(user: loggedUser, filters: filters);

			return new ResponseRecipesJson
			{
				Recipes = await recipes.MapToShortRecipeJson(loggedUser, _blobStorageService, _mapper)
			};
		}

		private static void Validate(RequestFilterRecipeJson request)
		{
			var validator = new FilterRecipeValidator();

			var result = validator.Validate(request);

			if(result.IsValid.isFalse())
			{
				var errorMessages = result.Errors.Select(error => error.ErrorMessage).Distinct().ToList();

				throw new ErrorOnValidationException(errorMessages);
			}
		}
	}
}
