using Moq;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Repositories.User;

namespace CommonTestUtilities.Repositories
{
	public class UserDeleteOnlyRepositoryBuilder
	{
		private readonly Mock<IUserDeleteOnlyRepository> _repository;

		public UserDeleteOnlyRepositoryBuilder() => _repository = new Mock<IUserDeleteOnlyRepository>();

		public IUserDeleteOnlyRepository Build() => _repository.Object;

	}
}
