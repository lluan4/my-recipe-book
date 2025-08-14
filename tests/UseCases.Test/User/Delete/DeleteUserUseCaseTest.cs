using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.ServiceBus;
using MyRecipeBook.Application.UseCases.User.Delete.Request;
using Shouldly;

namespace UseCases.Test.User.Delete
{
	public class DeleteUserUseCaseTest
	{
		[Fact]
		public async Task Success()
		{
			(var user, _) = UserBuilder.Build();

			var useCase = CreateUseCase(user);

			var act = async () => await useCase.Execute();

			await act.ShouldNotThrowAsync();

			user.Active.ShouldBeFalse();
		}

		private static DeleteUserUseCase CreateUseCase(MyRecipeBook.Domain.Entities.User user)
		{
			var loggedUser = LoggedUserBuilder.Build(user);
			var unitOfWork = UnitOfWorkBuilder.Build();
			var userUpdateOnlyRepository = new UserUpdateOnlyRepositoryBuilder().GetById(user).Build();
			var queue = DeleteUserQueueBuilder.Build();

			return new DeleteUserUseCase(
				queue: queue,
				userUpdateOnlyRepository: userUpdateOnlyRepository,
				loggedUser: loggedUser,
				unitOfWork: unitOfWork
				);
		}
	}
}
