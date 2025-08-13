using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using Microsoft.AspNetCore.Http;
using MyRecipeBook.Application.UseCases.Recipe.Register;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;
using Shouldly;
using UseCases.Test.Recipe.InlineDatas;

namespace UseCases.Test.Recipe.Register
{
	public class RegisterRecipeUseCaseTest
	{
		[Theory]
		[ClassData(typeof(ImageTypesInlineData))]
		public async Task Success(IFormFile file)
		{
			(var user, _) = UserBuilder.Build();

			var request = RequestRecipeFormDataBuilder.Build(file);

			var useCase = CreateUseCase(user);

			var result = await useCase.Execute(request);

			result.ShouldNotBeNull();
			result.Id.ShouldNotBeNullOrWhiteSpace();
			result.Title.ShouldBe(request.Title);
		}

		[Fact]
		public async Task Success_Withour_Image()
		{
			(var user, _) = UserBuilder.Build();

			var request = RequestRecipeFormDataBuilder.Build();

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

			var request = RequestRecipeFormDataBuilder.Build();
			request.Title = string.Empty;

			var useCase = CreateUseCase(user);

			async Task action() => await useCase.Execute(request);

			var ex = await Should.ThrowAsync<ErrorOnValidationException>(action);

			ex.ShouldSatisfyAllConditions(
				 () => ex.GetErrorMessages().Count.ShouldBe(1),
				 () => ex.GetErrorMessages().ShouldContain(ResourceMessageHelper.FieldEmpty("Title"))
			);
		}

		[Fact]
		public async Task Error_Invalid_File()
		{
			(var user, _) = UserBuilder.Build();

			var textFile = FormFileBuilder.Txt();

			var request = RequestRecipeFormDataBuilder.Build(textFile);

			var useCase = CreateUseCase(user);

			async Task action() => await useCase.Execute(request);

			var ex = await Should.ThrowAsync<ErrorOnValidationException>(action);

			ex.ShouldSatisfyAllConditions(
				 () => ex.GetErrorMessages().Count.ShouldBe(1),
				 () => ex.GetErrorMessages().ShouldContain(ResourceMessagesException.ONLY_IMAGES_ACCEPTED)
			);
		}


		private static RegisterRecipeUseCase CreateUseCase(MyRecipeBook.Domain.Entities.User user)
		{
			var mapper = MapperBuilder.Build();
			var unitOfWork = UnitOfWorkBuilder.Build();
			var loggedUser = LoggedUserBuilder.Build(user);
			var blobStorage = new CommonTestUtilities.BlobStorage.BlobStorageServiceBuilder().GetFileUrl(user, string.Empty).Build();

			var repository = RecipeWriteOnlyRepositoryBuilder.Build();

			var repositoryCookingTime = new CookingTimeReadOnlyRepositoryBuilder();
			repositoryCookingTime.ExistsAnyCookingTime();

			var repositoryDifficulty = new DifficultyReadOnlyRepositoryBuilder();
			repositoryDifficulty.ExistsAnyDifficulty();

			var repositoryDishType = new DishTypeReadOnlyRepositoryBuilder();
			repositoryDishType.ExistsAnyDishType();

			return new RegisterRecipeUseCase(repository, repositoryCookingTime.Build(), repositoryDifficulty.Build(), repositoryDishType.Build(), loggedUser, unitOfWork, mapper, blobStorage);
		}

	}
}
