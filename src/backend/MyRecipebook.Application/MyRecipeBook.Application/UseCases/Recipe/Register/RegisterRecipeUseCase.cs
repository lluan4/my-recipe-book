using AutoMapper;
using MyRecipeBook.Communication.Request;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.CookingTime;
using MyRecipeBook.Domain.Repositories.Difficulty;
using MyRecipeBook.Domain.Repositories.DishType;
using MyRecipeBook.Domain.Repositories.Recipe;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace MyRecipeBook.Application.UseCases.Recipe.Register
{
    public class RegisterRecipeUseCase : IRegisterRecipeUseCase
    {
        private readonly IRecipeWriteOnlyRepository _repository;

        private readonly ICookingTimeReadOnlyRepository _repositoryCookingTime;
        private readonly IDifficultyReadOnlyRepository _repositoryDifficultyTime;
        private readonly IDishTypeReadOnlyRepository _repositoryDishType;

        private readonly ILoggedUser _loggedUser;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RegisterRecipeUseCase(
            IRecipeWriteOnlyRepository repository,
            ICookingTimeReadOnlyRepository repositoryCookingTime,
            IDifficultyReadOnlyRepository repositoryDifficultyTime,
            IDishTypeReadOnlyRepository repositoryDishType,
            ILoggedUser loggedUser,
            IUnitOfWork unitOfWork,
            IMapper mapper
            )
        {
            _repository = repository;
            _repositoryCookingTime = repositoryCookingTime;
            _repositoryDifficultyTime = repositoryDifficultyTime;
            _repositoryDishType = repositoryDishType;
            _loggedUser = loggedUser;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseRegisteredRecipeJson> Execute(RequestRecipeJson request)
        {
            await ValidateAsync(request);

            var loggedUser = await _loggedUser.User();

            var recipe = _mapper.Map<Domain.Entities.Recipe>(request);
            recipe.UserId = loggedUser.Id;

            var instructions = request.Instructions.OrderBy(i => i.Step).ToList();
            for (var i = 0; i < instructions.Count; i++)
                instructions.ElementAt(i).Step = i + 1;

            recipe.Instructions = _mapper.Map<IList<Domain.Entities.Instruction>>(instructions);

            recipe.RecipeDishTypes = _mapper.Map<IList<Domain.Entities.RecipesDishTypes>>(request.DishTypes);

            await _repository.Add(recipe);

            await _unitOfWork.Commit();

            return _mapper.Map<ResponseRegisteredRecipeJson>(recipe);
        }

        private async Task ValidateAsync(RequestRecipeJson request)
        {
            var validator = new RecipeValidator(
                _repositoryCookingTime,
                _repositoryDifficultyTime,
                _repositoryDishType);

            var result = await validator.ValidateAsync(request);

            if (!result.IsValid)
                throw new ErrorOnValidationException(
                    result.Errors.Select(e => e.ErrorMessage).Distinct().ToList());
        }
    }
}
