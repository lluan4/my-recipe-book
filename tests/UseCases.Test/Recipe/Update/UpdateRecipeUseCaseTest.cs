using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using MyRecipeBook.Application.UseCases.Recipe.Update;
using MyRecipeBook.Exceptions.ExceptionsBase;
using Shouldly;

namespace UseCases.Test.Recipe.Update
{
    public class UpdateRecipeUseCaseTest
    {

        [Fact]
        public async Task Success()
        {
            (var user, _) = UserBuilder.Build();
            var recipe = RecipeBuilder.Build(user);
            var request = RequestRecipeJsonBuilder.Build();

            var useCase = CreateUseCase(user, recipe);

            Func<Task> act = async () => await useCase.Execute(recipe.Id, request); 

            await act.ShouldNotThrowAsync();
        }

        [Fact]
        public async Task Error_Recipe_NotFound()
        {
            (var user, _) = UserBuilder.Build();
            var recipe = RecipeBuilder.Build(user);
            var request = RequestRecipeJsonBuilder.Build();

            var useCase = CreateUseCase(user);

            async Task act() { await useCase.Execute(recipeId: user.Id, request); }

            var ex = await Should.ThrowAsync<NotFoundException>(act);

            ex.ShouldSatisfyAllConditions(
                () => ex.GetErrorMessages().Count.ShouldBe(1),
                () => ex.GetErrorMessages().ShouldContain(ResourceMessageHelper.FieldNotFound("Recipe"))
            );

        }

        [Fact]
        public async Task Error_Title_Empty()
        {
            (var user, _) = UserBuilder.Build();
            var recipe = RecipeBuilder.Build(user);
            var request = RequestRecipeJsonBuilder.Build();
            request.Title = string.Empty;

            var useCase = CreateUseCase(user, recipe);

            async Task act() { await useCase.Execute(recipeId: user.Id, request); }

            var ex = await Should.ThrowAsync<ErrorOnValidationException>(act);

            ex.ShouldSatisfyAllConditions(
                () => ex.GetErrorMessages().Count.ShouldBe(1),
                () => ex.GetErrorMessages().ShouldContain(ResourceMessageHelper.FieldEmpty("Title"))
            );

        }

        private static UpdateRecipeUseCase CreateUseCase(
            MyRecipeBook.Domain.Entities.User user,
            MyRecipeBook.Domain.Entities.Recipe? recipe = null
            )
        {
            var repositoryCookingTime = new CookingTimeReadOnlyRepositoryBuilder();
            repositoryCookingTime.ExistsAnyCookingTime();

            var repositoryDifficulty = new DifficultyReadOnlyRepositoryBuilder();
            repositoryDifficulty.ExistsAnyDifficulty();

            var repositoryDishType = new DishTypeReadOnlyRepositoryBuilder();
            repositoryDishType.ExistsAnyDishType();

            return new UpdateRecipeUseCase(
                loggedUser: LoggedUserBuilder.Build(user),
                unitOfWork: UnitOfWorkBuilder.Build(),
                mapper: MapperBuilder.Build(),
                updateRepository: new RecipeUpdateOnlyRepositoryBuilder().GetById(user, recipe).Build(),
                cookingTimeReadOnlyRepository: repositoryCookingTime.Build(),
                difficultyReadOnlyRepository: repositoryDifficulty.Build(),
                dishTypeReadOnlyRepository: repositoryDishType.Build()
                );
        }
    }
}
