using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using MyRecipeBook.Application.UseCases.Recipe.Delete;
using MyRecipeBook.Exceptions.ExceptionsBase;
using Shouldly;

namespace UseCases.Test.Recipe.Delete
{
    public class DeleteRecipeUseCaseTest
    {

        [Fact]
        public async Task Success()
        {
            (var user, _) = UserBuilder.Build();

            var recipe = RecipeBuilder.Build(user);

            var useCase = CreateUseCase(user, recipe);

            async Task act() { await useCase.Execute(recipe.Id); }

            await Should.NotThrowAsync(act);
        }

        [Fact]
        public async Task Error_Recipe_NotFound()
        {
            (var user, _) = UserBuilder.Build();

            var useCase = CreateUseCase(user);

            async Task act() { await useCase.Execute(recipeId: 1000); }

            var ex = await Should.ThrowAsync<NotFoundException>(act);

            ex.ShouldSatisfyAllConditions(
                () => ex.GetErrorMessages().Count.ShouldBe(1),
                () => ex.GetErrorMessages().ShouldContain(ResourceMessageHelper.FieldNotFound("Recipe"))
            );

        }

        private static DeleteRecipeUseCase CreateUseCase(
            MyRecipeBook.Domain.Entities.User user,
            MyRecipeBook.Domain.Entities.Recipe? recipe = null
            )
        {
            var loggedUser = LoggedUserBuilder.Build(user);
            var repositoryRead = new RecipeReadOnlyRepositoryBuilder().GetById(user, recipe).Build();
            var repositoryWrite = RecipeWriteOnlyRepositoryBuilder.Build();
            var unitOfWork = UnitOfWorkBuilder.Build();

            return new DeleteRecipeUseCase(loggedUser:loggedUser, repositoryRead:repositoryRead, repositoryWrite:repositoryWrite, unitOfWork:unitOfWork);
        }
    }
}
