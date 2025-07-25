using Moq;
using MyRecipeBook.Domain.Enums;
using MyRecipeBook.Domain.Repositories.DishType;


namespace CommonTestUtilities.Repositories
{
    public class DishTypeReadOnlyRepositoryBuilder
    {
        private readonly Mock<IDishTypeReadOnlyRepository> _repository;

        public DishTypeReadOnlyRepositoryBuilder() => _repository = new Mock<IDishTypeReadOnlyRepository>();

        public IDishTypeReadOnlyRepository Build() => _repository.Object;

        public void ExistsDishType(RecipeDishType dishType)
        {
            _repository.Setup(repository => repository.ExistsDishType(dishType)).ReturnsAsync(true);
        }

        public void DoesNotExistDishType(RecipeDishType dishType)
        {
            _repository.Setup(repository => repository.ExistsDishType(dishType)).ReturnsAsync(false);
        }

        public void ExistsAnyDishType()
        {
            _repository.Setup(repository => repository.ExistsDishType(It.IsAny<RecipeDishType>())).ReturnsAsync(true);
        }

        public void DoesNotExistAnyDishType()
        {
            _repository.Setup(repository => repository.ExistsDishType(It.IsAny<RecipeDishType>())).ReturnsAsync(false);
        }
    }
}