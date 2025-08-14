using CommonTestUtilities.BlobStorage;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using Microsoft.AspNetCore.Http;
using MyRecipeBook.Application.UseCases.Recipe.Image;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;
using Shouldly;
using UseCases.Test.Recipe.InlineDatas;

namespace UseCases.Test.Recipe.Image
{
	public class AddUpdateImageCoverUseCaseTest
	{
		[Theory]
		[ClassData(typeof(ImageTypesInlineData))]
		public async Task Success(IFormFile file)
		{
			(var user, _) = UserBuilder.Build();

			var recipe = RecipeBuilder.Build(user);

			var useCase = CreateUseCase(user, recipe);

			Func<Task> func = async () => await useCase.Execute(recipe.Id, file);

			await func.ShouldNotThrowAsync(); 

		}

		[Theory]
		[ClassData(typeof(ImageTypesInlineData))]
		public async Task Error_Recipe_NotFound(IFormFile file)
		{
			(var user, _) = UserBuilder.Build();

			var useCase = CreateUseCase(user);

			Func<Task> func = async () => await useCase.Execute(1, file);

			var ex = await func.ShouldThrowAsync<NotFoundException>();

			ex.ShouldSatisfyAllConditions(
				 () => ex.GetErrorMessages().Count.ShouldBe(1),
				 () => ex.GetErrorMessages().ShouldContain(ResourceMessageHelper.FieldNotFound("recipeId"))
			);

		}

		[Fact]
		public async Task Error_File_Is_Txt()
		{
			(var user, _) = UserBuilder.Build();

			var recipe = RecipeBuilder.Build(user);

			var useCase = CreateUseCase(user, recipe);

			var file = FormFileBuilder.Txt();

			Func<Task> func = async () => await useCase.Execute(recipe.Id, file);

			var ex = await func.ShouldThrowAsync<ErrorOnValidationException>();

			ex.ShouldSatisfyAllConditions(
				 () => ex.GetErrorMessages().Count.ShouldBe(1),
				 () => ex.GetErrorMessages().ShouldContain(ResourceMessagesException.ONLY_IMAGES_ACCEPTED)
			);

		}

		private static AddUpdateImageCoverUseCase CreateUseCase(MyRecipeBook.Domain.Entities.User user, MyRecipeBook.Domain.Entities.Recipe? recipe = null)
		{
			var loggedUser = LoggedUserBuilder.Build(user);
			var recipeUpdateOnlyRepository = new RecipeUpdateOnlyRepositoryBuilder().GetById(user, recipe).Build();
			var unitOfWork = UnitOfWorkBuilder.Build();
			var blobStorageService = new BlobStorageServiceBuilder().Build();

			return new AddUpdateImageCoverUseCase(
				loggedUser: loggedUser,
				recipeUpdateOnlyRepository: recipeUpdateOnlyRepository,
				unitOfWork: unitOfWork,
				blobStorageService: blobStorageService
				);
		}
	}
}
