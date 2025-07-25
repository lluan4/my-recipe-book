using FluentValidation;
using MyRecipeBook.Application.Mappers;
using MyRecipeBook.Application.SharedValidators;
using MyRecipeBook.Communication.Request;
using MyRecipeBook.Domain.Repositories.CookingTime;
using MyRecipeBook.Domain.Repositories.Difficulty;
using MyRecipeBook.Domain.Repositories.DishType;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace MyRecipeBook.Application.UseCases.Recipe
{
    public class RecipeValidator : AbstractValidator<RequestRecipeJson>
    {
        private readonly ICookingTimeReadOnlyRepository _cookingTimeReadOnlyRepository;
        private readonly IDifficultyReadOnlyRepository _difficultyReadOnlyRepository;
        private readonly IDishTypeReadOnlyRepository _dishTypeReadOnlyRepository;

        public RecipeValidator(
            ICookingTimeReadOnlyRepository cookingTimeReadOnlyRepository,
            IDifficultyReadOnlyRepository difficultyReadOnlyRepository,
            IDishTypeReadOnlyRepository dishTypeReadOnlyRepository)
        {
            _cookingTimeReadOnlyRepository = cookingTimeReadOnlyRepository;
            _difficultyReadOnlyRepository = difficultyReadOnlyRepository;
            _dishTypeReadOnlyRepository = dishTypeReadOnlyRepository;



            RuleFor(recipe => recipe.Title)
                .SetValidator(new StringValidator<RequestRecipeJson>("Title", 100));

            RuleFor(recipe => recipe.CookingTimeId)
                .Cascade(CascadeMode.Stop)
                .IsInEnum()
                .WithMessage(ResourceMessageHelper.FieldNotSupported("CookingTime"))
                .MustAsync(async (cookingTime, cancellation) =>
                {
                    Console.WriteLine(cookingTime);
                    if (cookingTime == null) return true;

                    try
                    {
                        var domainCookingTime = CookingTimeMapper.ToDomain(cookingTime.Value);
                        return await _cookingTimeReadOnlyRepository.ExistsCookingTime(domainCookingTime);
                    }
                    catch (InvalidOperationException)
                    {
                        return false;
                    }


                })
                .WithMessage(ResourceMessageHelper.FieldEmpty("CookingTime"));

            RuleFor(recipe => recipe.DifficultyId)
                .Cascade(CascadeMode.Stop)
                .IsInEnum()
                .WithMessage(ResourceMessageHelper.FieldNotSupported("Difficulty"))
                .MustAsync(async (difficulty, cancellation) =>
                {
                    if (difficulty == null) return true;

                    try
                    {
                        var domainDifficulty = DifficultyMapper.ToDomain(difficulty.Value);
                        return await _difficultyReadOnlyRepository.ExistsDifficulty(domainDifficulty);
                    }
                    catch (InvalidOperationException)
                    {
                        return false;
                    }

                })
                .WithMessage(ResourceMessageHelper.FieldNotSupported("Difficulty"));

            RuleFor(recipe => recipe.Ingredients.Count)
                .GreaterThan(0)
                .WithMessage(ResourceMessageHelper.FieldMustHaveAtLeastOne("Ingredients"));

            RuleFor(recipe => recipe.Instructions.Count)
                .GreaterThan(0)
                .WithMessage(ResourceMessageHelper.FieldMustHaveAtLeastOne("Instructions"));

            RuleForEach(recipe => recipe.DishTypes)
                .Cascade(CascadeMode.Stop)
                .IsInEnum()
                .WithMessage(ResourceMessageHelper.FieldNotSupported("DishTypes"))
                .MustAsync(async (dishType, cancellation) =>
                {
                    try
                    {
                        var domainDishType = DishTypesMapper.ToDomain(dishType);
                        return await _dishTypeReadOnlyRepository.ExistsDishType(domainDishType);
                    }
                    catch (InvalidOperationException)
                    {
                        return false;
                    }
                })
                .WithMessage(ResourceMessageHelper.FieldNotSupported("DishTypes"));

            RuleForEach(recipe => recipe.Ingredients)
                .SetValidator(new StringValidator<RequestRecipeJson>("Ingredients", 100));

            RuleForEach(recipe => recipe.Instructions)
                .ChildRules(instructionRule =>
                {
                    instructionRule.RuleFor(instruction => instruction.Step)
                        .GreaterThan(0)
                        .WithMessage(ResourceMessageHelper.FieldNonNegative("Instructions - Step"));

                    instructionRule.RuleFor(instruction => instruction.Description)
                        .SetValidator(new StringValidator<RequestInstructionJson>("Instructions - Description", 2000));
                });

            RuleFor(recipe => recipe.Instructions)
                .Must(instructions => instructions.Select(i => i.Step).Distinct().Count() == instructions.Count)
                .WithMessage(ResourceMessageHelper.FieldTwoOrMore("Instructions - Step"));
        }
    }
}