using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using MyRecipeBook.Application.UseCases.Recipe.GetById;
using MyRecipeBook.Exceptions.ExceptionsBase;
using Shouldly;

namespace UseCases.Test.Recipe.GetById
{
    public class GetRecipeByIdUseCaseTest
    {

        [Fact]
        public async Task Success()
        {
            (var user, _) = UserBuilder.Build();

            var recipe = RecipeBuilder.Build(user);

            var useCase = CreateUseCase(user, recipe);

            var result = await useCase.Execute(recipe.Id);

            result.ShouldNotBeNull();
            result.Id.ShouldNotBeNullOrWhiteSpace();
            result.Title.ShouldBe(recipe.Title);

        }

        [Fact]
        public async Task Error_Recipe_NotFound()
        {
            (var user, _) = UserBuilder.Build();

            var useCase = CreateUseCase(user);

            async Task act() { await useCase.Execute(request: 1000); }

            var ex = await Should.ThrowAsync<NotFoundException>(act);

            ex.ShouldSatisfyAllConditions(
                () => ex.GetErrorMessages().Count.ShouldBe(1),
                () => ex.GetErrorMessages().ShouldContain(ResourceMessageHelper.FieldNotFound("Recipe"))
            );

        }



        private static GetRecipeByIdUseCase CreateUseCase(
            MyRecipeBook.Domain.Entities.User user,
            MyRecipeBook.Domain.Entities.Recipe? recipe = null
            )
        {
            var mapper = MapperBuilder.Build();
            var loggedUser = LoggedUserBuilder.Build(user);
            var repository = new RecipeReadOnlyRepositoryBuilder().GetById(user, recipe).Build();

            return new GetRecipeByIdUseCase(mapper, loggedUser, repository);
        }
    }
}
