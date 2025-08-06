using AutoMapper;
using MyRecipeBook.Communication.Response;
using MyRecipeBook.Domain.Repositories.Recipe;
using MyRecipeBook.Domain.Services.LoggedUser;

namespace MyRecipeBook.Application.UseCases.Dashboard.Get
{
    public class GetDashboardUseCase : IGetDashboardUseCase
    {
        private readonly IRecipeReadOnlyRepository _recipeReadOnlyRepository;
        private readonly IMapper _mapper;
        private readonly ILoggedUser _loggedUser;

        public GetDashboardUseCase(IRecipeReadOnlyRepository recipeReadOnlyRepository, IMapper mapper, ILoggedUser loggedUser)
        {
            _recipeReadOnlyRepository = recipeReadOnlyRepository;
            _mapper = mapper;
            _loggedUser = loggedUser;
        }

        public async Task<ResponseRecipesJson> Execute()
        {
            var loggedUser = await _loggedUser.User();

            var recipes = await _recipeReadOnlyRepository.GetForDashboard(user: loggedUser);

            return new ResponseRecipesJson
            {
                Recipes = _mapper.Map<IList<ResponseShortRecipeJson>>(recipes)
            };
        }
    }
}
