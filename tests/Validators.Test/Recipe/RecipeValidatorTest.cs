using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using MyRecipeBook.Application.UseCases.Recipe;
using MyRecipeBook.Communication.Enums;
using MyRecipeBook.Exceptions.ExceptionsBase;
using Shouldly;

namespace Validators.Test.Recipe;

public class RecipeValidatorTest
{
    private static RecipeValidator CreateValidator(
        Action<CookingTimeReadOnlyRepositoryBuilder>? cookingTimeSetup = null,
        Action<DifficultyReadOnlyRepositoryBuilder>? difficultySetup = null,
        Action<DishTypeReadOnlyRepositoryBuilder>? dishTypeSetup = null)
    {
        var cookingTimeBuilder = new CookingTimeReadOnlyRepositoryBuilder();
        var difficultyBuilder = new DifficultyReadOnlyRepositoryBuilder();
        var dishTypeBuilder = new DishTypeReadOnlyRepositoryBuilder();

        if (cookingTimeSetup != null)
            cookingTimeSetup(cookingTimeBuilder);
        else
            cookingTimeBuilder.ExistsAnyCookingTime();

        if (difficultySetup != null)
            difficultySetup(difficultyBuilder);
        else
            difficultyBuilder.ExistsAnyDifficulty();

        if (dishTypeSetup != null)
            dishTypeSetup(dishTypeBuilder);
        else
            dishTypeBuilder.ExistsAnyDishType();

        return new RecipeValidator(
            cookingTimeBuilder.Build(),
            difficultyBuilder.Build(),
            dishTypeBuilder.Build()
        );
    }

    [Fact]
    public async Task Success()
    {
        var validator = CreateValidator();

        var request = RequestRecipeJsonBuilder.Build();

        var result = await validator.ValidateAsync(request);

        result.IsValid.ShouldBeTrue();
    }

    [Fact]
    public async Task Error_Invalid_Cooking_Time()
    {
        var validator = CreateValidator();

        var request = RequestRecipeJsonBuilder.Build();
        request.CookingTimeId = (MyRecipeBook.Communication.Enums.RecipeCookingTime?)1000;

        var result = await validator.ValidateAsync(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldHaveSingleItem()
            .ErrorMessage.ShouldBe(ResourceMessageHelper.FieldNotSupported("CookingTime"));
    }

    [Fact]
    public async Task Error_Cooking_Time_Not_In_Database()
    {
        var validator = CreateValidator(
            cookingTimeSetup: repo => repo.DoesNotExistAnyCookingTime()
        );

        var request = RequestRecipeJsonBuilder.Build();
        request.CookingTimeId = (MyRecipeBook.Communication.Enums.RecipeCookingTime?)1000;

        var result = await validator.ValidateAsync(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.Any(e => e.ErrorMessage == ResourceMessageHelper.FieldNotSupported("CookingTime"))
            .ShouldBeTrue();
    }

    [Fact]
    public async Task Success_Cooking_Time_Null()
    {
        var validator = CreateValidator();

        var request = RequestRecipeJsonBuilder.Build();
        request.CookingTimeId = null;

        var result = await validator.ValidateAsync(request);

        result.IsValid.ShouldBeTrue();
    }

    [Fact]
    public async Task Error_Invalid_Difficulty()
    {
        var validator = CreateValidator();

        var request = RequestRecipeJsonBuilder.Build();
        request.DifficultyId = (MyRecipeBook.Communication.Enums.RecipeDifficulty?)1000;

        var result = await validator.ValidateAsync(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldHaveSingleItem()
            .ErrorMessage.ShouldBe(ResourceMessageHelper.FieldNotSupported("Difficulty"));
    }

    [Fact]
    public async Task Error_Difficulty_Not_In_Database()
    {
        var validator = CreateValidator(
            difficultySetup: repo => repo.DoesNotExistAnyDifficulty()
        );

        var request = RequestRecipeJsonBuilder.Build();
        request.DifficultyId = (MyRecipeBook.Communication.Enums.RecipeDifficulty?)1000;

        var result = await validator.ValidateAsync(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.Any(e => e.ErrorMessage == ResourceMessageHelper.FieldNotSupported("Difficulty"))
            .ShouldBeTrue();
    }

    [Fact]
    public async Task Success_Difficulty_Null()
    {
        var validator = CreateValidator();

        var request = RequestRecipeJsonBuilder.Build();
        request.DifficultyId = null;

        var result = await validator.ValidateAsync(request);

        result.IsValid.ShouldBeTrue();
    }

    [Theory]
    [InlineData(1)]
    [InlineData(50)]
    [InlineData(99)]
    [InlineData(100)]
    public async Task Success_Title_Size(int size)
    {
        var validator = CreateValidator();

        var request = RequestRecipeJsonBuilder.Build();
        request.Title = new string('A', size);

        var result = await validator.ValidateAsync(request);

        result.IsValid.ShouldBeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("          ")]
    [InlineData("")]
    public async Task Error_Title_Empty(string? title)
    {
        var validator = CreateValidator();

        var request = RequestRecipeJsonBuilder.Build();
        request.Title = title!;

        var result = await validator.ValidateAsync(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldHaveSingleItem()
            .ErrorMessage.ShouldBe(ResourceMessageHelper.FieldEmpty("Title"));
    }

    [Theory]
    [InlineData(101)]
    [InlineData(150)]
    public async Task Error_Title_Exceeds_Max_Length(int size)
    {
        var validator = CreateValidator();

        var request = RequestRecipeJsonBuilder.Build();
        request.Title = new string('A', size);

        var result = await validator.ValidateAsync(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldHaveSingleItem()
            .ErrorMessage.ShouldBe(ResourceMessageHelper.FieldMustHaveMaxLength("Title", 100));
    }

    [Fact]
    public async Task Success_DishTypes_Empty()
    {
        var validator = CreateValidator();

        var request = RequestRecipeJsonBuilder.Build();
        request.DishTypes.Clear();

        var result = await validator.ValidateAsync(request);

        result.IsValid.ShouldBeTrue();
    }

    [Fact]
    public async Task Error_DishTypes_Invalid()
    {
        var validator = CreateValidator();

        var request = RequestRecipeJsonBuilder.Build();
        request.DishTypes.Add((RecipeDishType)1000);

        var result = await validator.ValidateAsync(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldHaveSingleItem()
            .ErrorMessage.ShouldBe(ResourceMessageHelper.FieldNotSupported("DishTypes"));
    }

    [Fact]
    public async Task Error_DishTypes_Not_In_Database()
    {
        var validator = CreateValidator(
            dishTypeSetup: repo => repo.DoesNotExistAnyDishType()
        );

        var request = RequestRecipeJsonBuilder.Build();

        var result = await validator.ValidateAsync(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.Any(e => e.PropertyName.StartsWith("DishTypes"))
            .ShouldBeTrue();
    }

    [Fact]
    public async Task Error_Ingredients_Empty()
    {
        var validator = CreateValidator();

        var request = RequestRecipeJsonBuilder.Build();
        request.Ingredients.Clear();

        var result = await validator.ValidateAsync(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldHaveSingleItem()
            .ErrorMessage.ShouldBe(ResourceMessageHelper.FieldMustHaveAtLeastOne("Ingredients"));
    }

    [Fact]
    public async Task Error_Instructions_Empty()
    {
        var validator = CreateValidator();

        var request = RequestRecipeJsonBuilder.Build();
        request.Instructions.Clear();

        var result = await validator.ValidateAsync(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldHaveSingleItem()
            .ErrorMessage.ShouldBe(ResourceMessageHelper.FieldMustHaveAtLeastOne("Instructions"));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("          ")]
    [InlineData("")]
    public async Task Error_Ingredient_Empty(string? ingredient)
    {
        var validator = CreateValidator();

        var request = RequestRecipeJsonBuilder.Build();
        request.Ingredients.Add(ingredient!);

        var result = await validator.ValidateAsync(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldHaveSingleItem()
            .ErrorMessage.ShouldBe(ResourceMessageHelper.FieldEmpty("Ingredients"));
    }

    [Fact]
    public async Task Error_Instructions_Same_Step()
    {
        var validator = CreateValidator();

        var request = RequestRecipeJsonBuilder.Build();
        request.Instructions.First().Step = request.Instructions.Last().Step;

        var result = await validator.ValidateAsync(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldHaveSingleItem()
            .ErrorMessage.ShouldBe(ResourceMessageHelper.FieldTwoOrMore("Instructions - Step"));
    }

    [Fact]
    public async Task Error_Instructions_Negative_Step()
    {
        var validator = CreateValidator();

        var request = RequestRecipeJsonBuilder.Build();
        request.Instructions.First().Step = -1;

        var result = await validator.ValidateAsync(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldHaveSingleItem()
            .ErrorMessage.ShouldBe(ResourceMessageHelper.FieldNonNegative("Instructions - Step"));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("          ")]
    [InlineData("")]
    public async Task Error_Instructions_Description_Empty(string? instruction)
    {
        var validator = CreateValidator();

        var request = RequestRecipeJsonBuilder.Build();
        request.Instructions.First().Description = instruction!;

        var result = await validator.ValidateAsync(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldHaveSingleItem()
            .ErrorMessage.ShouldBe(ResourceMessageHelper.FieldEmpty("Instructions - Description"));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(1000)]
    [InlineData(1999)]
    [InlineData(2000)]
    public async Task Success_Instructions_Description_Size(int size)
    {
        var validator = CreateValidator();

        var request = RequestRecipeJsonBuilder.Build();
        request.Instructions.First().Description = new string('A', size);

        var result = await validator.ValidateAsync(request);

        result.IsValid.ShouldBeTrue();
    }

    [Theory]
    [InlineData(2001)]
    [InlineData(10000)]
    public async Task Error_Instructions_Description_Size(int size)
    {
        var validator = CreateValidator();

        var request = RequestRecipeJsonBuilder.Build();
        request.Instructions.First().Description = new string('A', size);

        var result = await validator.ValidateAsync(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldHaveSingleItem()
            .ErrorMessage.ShouldBe(ResourceMessageHelper.FieldMustHaveMaxLength("Instructions - Description", 2000));
    }
}