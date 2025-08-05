using AutoMapper;
using MyRecipeBook.Communication.Request;
using MyRecipeBook.Domain.Extension;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.CookingTime;
using MyRecipeBook.Domain.Repositories.Difficulty;
using MyRecipeBook.Domain.Repositories.DishType;
using MyRecipeBook.Domain.Repositories.Recipe;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace MyRecipeBook.Application.UseCases.Recipe.Update
{
    public class UpdateRecipeUseCase : IUpdateRecipeUseCase
    {
        private readonly ILoggedUser _loggedUser;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        private readonly IRecipeUpdateOnlyRepository _updateRepository;
        private readonly ICookingTimeReadOnlyRepository _cookingTimeReadOnlyRepository;
        private readonly IDifficultyReadOnlyRepository _difficultyReadOnlyRepository;
        private readonly IDishTypeReadOnlyRepository _dishTypeReadOnlyRepository;

        public UpdateRecipeUseCase(ILoggedUser loggedUser, 
            IUnitOfWork unitOfWork, 
            IMapper mapper, 
            IRecipeUpdateOnlyRepository updateRepository,
            ICookingTimeReadOnlyRepository cookingTimeReadOnlyRepository, 
            IDifficultyReadOnlyRepository difficultyReadOnlyRepository,
            IDishTypeReadOnlyRepository dishTypeReadOnlyRepository)
        {
            _loggedUser = loggedUser;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _updateRepository = updateRepository;
            _cookingTimeReadOnlyRepository = cookingTimeReadOnlyRepository;
            _difficultyReadOnlyRepository = difficultyReadOnlyRepository;
            _dishTypeReadOnlyRepository = dishTypeReadOnlyRepository;
        }

        public async Task Execute(long recipeId, RequestRecipeJson request)
        {
            await Validade(request);

            var loggedUser = await _loggedUser.User();

            var recipe = await _updateRepository.GetById(loggedUser, recipeId);

            if (recipe is null)
                throw new NotFoundException(ResourceMessageHelper.FieldNotFound("Recipe"));
            
            recipe.Ingredients.Clear();
            recipe.Instructions.Clear();
            recipe.RecipeDishTypes.Clear();

            _mapper.Map(request, recipe);

            var instructions = request.Instructions.OrderBy(i => i.Step).ToList();

            for (var i = 0; i < instructions.Count; i++)
                instructions[i].Step = i + 1;

            recipe.Instructions = _mapper.Map<IList<Domain.Entities.Instruction>>(instructions);

            _updateRepository.Update(recipe);

            await _unitOfWork.Commit();

        }

        private async Task Validade(RequestRecipeJson request)
        {
            var validator = new RecipeValidator(
                cookingTimeReadOnlyRepository: _cookingTimeReadOnlyRepository, 
                difficultyReadOnlyRepository: _difficultyReadOnlyRepository, 
                dishTypeReadOnlyRepository: _dishTypeReadOnlyRepository);

            var result = await validator.ValidateAsync(request);

            if (result.IsValid.isFalse())
                throw new ErrorOnValidationException(result.Errors.Select(e => e.ErrorMessage).Distinct().ToList());
        }
    }
}
