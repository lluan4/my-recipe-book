using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using MyRecipeBook.Application.UseCases.Recipe.Filter;
using MyRecipeBook.Exceptions.ExceptionsBase;
using Shouldly;

namespace UseCases.Test.Recipe.Filter
{
    public class FilterRecipeUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            (var user, _) = UserBuilder.Build();

            var request = RequestFilterRecipeJsonBuilder.Build();

            var recipes = RecipeBuilder.Collection(user);

            var useCase = CreateUseCase(user, recipes);

            var result = await useCase.Execute(request);

            result.ShouldNotBeNull();
            result.Recipes.ShouldNotBeNull();
            result.Recipes.ShouldNotBeEmpty();
            result.Recipes.Count.ShouldBe(recipes.Count);

        }

        [Fact]
        public async Task Error_CookingTime_Invalid()
        {
            (var user, _) = UserBuilder.Build();

            var recipes = RecipeBuilder.Collection(user);

            var request = RequestFilterRecipeJsonBuilder.Build();
            request.CookingTimes.Add((MyRecipeBook.Communication.Enums.RecipeCookingTime)1000);

            var useCase = CreateUseCase(user, recipes);

            Func<Task> act = async () => { await useCase.Execute(request); };

            var ex = await Should.ThrowAsync<ErrorOnValidationException>(act);
            ex.ShouldSatisfyAllConditions(
                () => ex.ErrorMessages.Count.ShouldBe(1),
                () => ex.ErrorMessages.ShouldContain(ResourceMessageHelper.FieldNotSupported("CookingTime"))
            );

        }


        private static FilterRecipeUseCase CreateUseCase(
            MyRecipeBook.Domain.Entities.User user,
            IList<MyRecipeBook.Domain.Entities.Recipe> recipes
            )
        {
            var mapper = MapperBuilder.Build();
            var loggedUser = LoggedUserBuilder.Build(user);
            var repository = new RecipeReadOnlyRepositoryBuilder().Filter(user, recipes).Build();

            return new FilterRecipeUseCase(mapper, loggedUser, repository);
        }
    }
}
