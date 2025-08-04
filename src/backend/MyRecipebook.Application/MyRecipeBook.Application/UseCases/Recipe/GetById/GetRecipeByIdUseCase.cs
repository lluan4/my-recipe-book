using AutoMapper;
using MyRecipeBook.Communication.Response;
using MyRecipeBook.Domain.Repositories.Recipe;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace MyRecipeBook.Application.UseCases.Recipe.GetById
{
    public class GetRecipeByIdUseCase : IGetRecipeByIdUseCase
    {
        private readonly IMapper _mapper;
        private readonly ILoggedUser _loggedUSer;
        private readonly IRecipeReadOnlyRepository _repository;

        public GetRecipeByIdUseCase(IMapper mapper, ILoggedUser loggedUSer, IRecipeReadOnlyRepository repository)
        {
            _mapper = mapper;
            _loggedUSer = loggedUSer;
            _repository = repository;
        }

        public async Task<ResponseRecipeJson> Execute(long request)
        {
            var loggedUser = await _loggedUSer.User();

            var recipe = await _repository.GetById(loggedUser, request );

            if (recipe is null)
                throw new NotFoundException(ResourceMessageHelper.FieldNotFound("Recipe"));

            return _mapper.Map<ResponseRecipeJson>(recipe);
        }
    }
}
