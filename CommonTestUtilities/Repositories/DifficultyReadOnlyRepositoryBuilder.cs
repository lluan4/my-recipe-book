using Moq;
using MyRecipeBook.Domain.Enums;
using MyRecipeBook.Domain.Repositories.Difficulty;

namespace CommonTestUtilities.Repositories
{
    public class DifficultyReadOnlyRepositoryBuilder
    {
        private readonly Mock<IDifficultyReadOnlyRepository> _repository;

        public DifficultyReadOnlyRepositoryBuilder() => _repository = new Mock<IDifficultyReadOnlyRepository>();

        public IDifficultyReadOnlyRepository Build() => _repository.Object;

        public void ExistsDifficulty(DifficultyEnum difficulty)
        {
            _repository.Setup(repository => repository.ExistsDifficulty(difficulty)).ReturnsAsync(true);
        }

        public void DoesNotExistDifficulty(DifficultyEnum difficulty)
        {
            _repository.Setup(repository => repository.ExistsDifficulty(difficulty)).ReturnsAsync(false);
        }

        public void ExistsAnyDifficulty()
        {
            _repository.Setup(repository => repository.ExistsDifficulty(It.IsAny<DifficultyEnum>())).ReturnsAsync(true);
        }

        public void DoesNotExistAnyDifficulty()
        {
            _repository.Setup(repository => repository.ExistsDifficulty(It.IsAny<DifficultyEnum>())).ReturnsAsync(false);
        }
    }
}