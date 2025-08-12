using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using MyRecipeBook.Application.UseCases.Dashboard.Get;
using Shouldly;

namespace UseCases.Test.Dashboard
{
	public class GetDashboardUseCaseTest
	{

		[Fact]
		public async Task Success()
		{
			(var user, _) = UserBuilder.Build();

			var recipes = RecipeBuilder.Collection(user);

			var useCase = CreateUseCase(user, recipes);

			var result = await useCase.Execute();

			result.Recipes.ShouldSatisfyAllConditions(
				 () => recipes.ShouldNotBeEmpty(),
				 () => recipes.Select(r => r.Id).ShouldBeUnique()
			);

			foreach(var recipe in result.Recipes)
			{
				recipe.ShouldSatisfyAllConditions(
					 () => recipe.Id.ShouldNotBeNullOrWhiteSpace(),
					 () => recipe.Title.ShouldNotBeNullOrWhiteSpace(),
					 () => recipe.AmountIngredients.ShouldBeGreaterThan(0),
					 () => recipe.ImageUrl.ShouldNotBeNullOrWhiteSpace()
				);
			}

		}

		private static GetDashboardUseCase CreateUseCase(
			 MyRecipeBook.Domain.Entities.User user,
			 IList<MyRecipeBook.Domain.Entities.Recipe> recipes
			 )
		{
			var mapper = MapperBuilder.Build();
			var loggedUser = LoggedUserBuilder.Build(user);
			var repository = new RecipeReadOnlyRepositoryBuilder().GetForDashboard(user, recipes).Build();
			var blobStorage = new CommonTestUtilities.BlobStorage.BlobStorageServiceBuilder().GetFileUrl(user, recipes).Build();

			return new GetDashboardUseCase(
				recipeReadOnlyRepository: repository,
				mapper: mapper,
				loggedUser: loggedUser,
				blobStorageService: blobStorage
				);
		}
	}
}
