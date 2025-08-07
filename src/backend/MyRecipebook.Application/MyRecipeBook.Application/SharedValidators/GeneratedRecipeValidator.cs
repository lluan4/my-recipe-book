using FluentValidation;
using MyRecipeBook.Communication.Enums;
using MyRecipeBook.Domain.Dtos.Recipes;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace MyRecipeBook.Application.SharedValidators
{
    public class GeneratedRecipeDataValidator : AbstractValidator<GeneratedRecipeDataModel>
    {
        public GeneratedRecipeDataValidator()
        {
            RuleFor(data => data.Title)
                .NotEmpty()
                .WithMessage(ResourceMessageHelper.FieldEmpty("Title"))
                .MaximumLength(100)
                .WithMessage(ResourceMessageHelper.FieldMustHaveMaxLength("Title", 100));

            RuleFor(data => data.CookingTimeValue)
                .Must(value => Enum.TryParse<RecipeCookingTime>(value, out _))
                .WithMessage(ResourceMessageHelper.FieldNotSupported("CookingTime"));

            RuleFor(data => data.DifficultyValue)
                .Must(value => Enum.TryParse<RecipeDifficulty>(value, out _))
                .WithMessage(ResourceMessageHelper.FieldNotSupported("Difficulty"));

            RuleFor(data => data.DishTypeValue)
                .Must(value => Enum.TryParse<RecipeDishType>(value, out _))
                .WithMessage(ResourceMessageHelper.FieldNotSupported("DishType"));

            RuleFor(data => data.Ingredients.Count)
                .GreaterThan(0)
                .WithMessage(ResourceMessageHelper.FieldMustHaveAtLeastOne("Ingredients"));

            RuleForEach(data => data.Ingredients)
                .NotEmpty()
                .WithMessage(ResourceMessageHelper.FieldEmpty("Ingredients"))
                .MaximumLength(100)
                .WithMessage(ResourceMessageHelper.FieldMustHaveMaxLength("Ingredients", 100));

            RuleFor(data => data.Instructions.Count)
                .GreaterThan(0)
                .WithMessage(ResourceMessageHelper.FieldMustHaveAtLeastOne("Instructions"));

            RuleForEach(data => data.Instructions)
                .ChildRules(instructionRule =>
                {
                    instructionRule.RuleFor(instruction => instruction.Step)
                        .GreaterThan(0)
                        .WithMessage(ResourceMessageHelper.FieldNonNegative("Instructions - Step"));

                    instructionRule.RuleFor(instruction => instruction.Description)
                        .NotEmpty()
                        .WithMessage(ResourceMessageHelper.FieldEmpty("Instructions - Description"))
                        .MaximumLength(2000)
                        .WithMessage(ResourceMessageHelper.FieldMustHaveMaxLength("Instructions - Description", 2000));
                });
        }
    }

    public class GeneratedRecipeDataModel
    {
        public string Title { get; set; } = string.Empty;
        public string CookingTimeValue { get; set; } = string.Empty;
        public string DifficultyValue { get; set; } = string.Empty;
        public string DishTypeValue { get; set; } = string.Empty;
        public List<string> Ingredients { get; set; } = new();
        public List<GeneratedInstructionDto> Instructions { get; set; } = new();
    }

    public class InstructionDataModel
    {
        public int Step { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}