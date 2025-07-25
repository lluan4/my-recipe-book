using Moq;
using MyRecipeBook.Domain.Enums;
using MyRecipeBook.Domain.Repositories.CookingTime;

namespace CommonTestUtilities.Repositories
{
    public class CookingTimeReadOnlyRepositoryBuilder
    {
        private readonly Mock<ICookingTimeReadOnlyRepository> _repository;

        public CookingTimeReadOnlyRepositoryBuilder() => _repository = new Mock<ICookingTimeReadOnlyRepository>();

        public ICookingTimeReadOnlyRepository Build() => _repository.Object;

        public void ExistsCookingTime(CookingTimeEnum cookingTime)
        {
            _repository.Setup(repository => repository.ExistsCookingTime(cookingTime)).ReturnsAsync(true);
        }

        public void DoesNotExistCookingTime(CookingTimeEnum cookingTime)
        {
            _repository.Setup(repository => repository.ExistsCookingTime(cookingTime)).ReturnsAsync(false);
        }

        public void ExistsAnyCookingTime()
        {
            _repository.Setup(repository => repository.ExistsCookingTime(It.IsAny<CookingTimeEnum>())).ReturnsAsync(true);
        }

        public void DoesNotExistAnyCookingTime()
        {
            _repository.Setup(repository => repository.ExistsCookingTime(It.IsAny<CookingTimeEnum>())).ReturnsAsync(false);
        }
    }
}