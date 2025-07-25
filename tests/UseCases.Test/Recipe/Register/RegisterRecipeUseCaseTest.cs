using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using MyRecipeBook.Application.UseCases.Recipe.Register;
using MyRecipeBook.Exceptions.ExceptionsBase;
using Shouldly;

namespace UseCases.Test.Recipe.Register
{
    public class RegisterRecipeUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            (var user, _) = UserBuilder.Build();

            var request = RequestRecipeJsonBuilder.Build();

            var useCase = CreateUseCase(user);

            var result = await useCase.Execute(request);

            result.ShouldNotBeNull();
            result.Id.ShouldNotBeNullOrWhiteSpace();
            result.Title.ShouldBe(request.Title);
        }

        [Fact]
        public async Task Error_Title_Empty()
        {
            (var user, _) = UserBuilder.Build();

            var request = RequestRecipeJsonBuilder.Build();
            request.Title = string.Empty;

            var useCase = CreateUseCase(user);

            async Task action() => await useCase.Execute(request);

            var ex = await Should.ThrowAsync<ErrorOnValidationException>(action);

            ex.ShouldSatisfyAllConditions(
                () => ex.ErrorMessages.Count.ShouldBe(1),
                () => ex.ErrorMessages.ShouldContain(ResourceMessageHelper.FieldEmpty("Title"))
            );
        }


        private static RegisterRecipeUseCase CreateUseCase(MyRecipeBook.Domain.Entities.User user)
        {
            var mapper = MapperBuilder.Build();
            var unitOfWork = UnitOfWorkBuilder.Build();
            var loggedUser = LoggedUserBuilder.Build(user);

            var repository = RecipeWriteOnlyRepositoryBuilder.Build();

            var repositoryCookingTime = new CookingTimeReadOnlyRepositoryBuilder();
            repositoryCookingTime.ExistsAnyCookingTime();

            var repositoryDifficulty = new DifficultyReadOnlyRepositoryBuilder();
            repositoryDifficulty.ExistsAnyDifficulty();

            var repositoryDishType = new DishTypeReadOnlyRepositoryBuilder();
            repositoryDishType.ExistsAnyDishType();

            return new RegisterRecipeUseCase(repository, repositoryCookingTime.Build(), repositoryDifficulty.Build(), repositoryDishType.Build(), loggedUser, unitOfWork, mapper);
        }

    }
}
