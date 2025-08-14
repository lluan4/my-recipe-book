using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using MyRecipeBook.Application.UseCases.User.Delete;
using Shouldly;

namespace UseCases.Test.Recipe.Delete.Delete
{
	public class DeleteUserAccountUseCaseTest
	{
		[Fact]
		public async Task Success()
		{
			(var user, _) = UserBuilder.Build();

			var useCase = CreateUseCase();

			var act = async () => await useCase.Execute(user.UserIdentifier);

			await act.ShouldNotThrowAsync();
		}

		private static DeleteUserAccountUseCase CreateUseCase()
		{
			var deleteOnlyRepository = new UserDeleteOnlyRepositoryBuilder().Build();
			var unitOfWork = UnitOfWorkBuilder.Build();
			var blobStorage = new CommonTestUtilities.BlobStorage.BlobStorageServiceBuilder().Build();

			return new DeleteUserAccountUseCase(
				repository: deleteOnlyRepository,
				unitOfWork: unitOfWork,
				blobStorageService: blobStorage
				);
		}
	}
}
