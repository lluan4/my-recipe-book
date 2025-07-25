using MyRecipeBook.Exceptions.ExceptionsBase;

namespace MyRecipeBook.Application.Mappers
{
    public static class DifficultyMapper
    {
        public static Domain.Enums.DifficultyEnum ToDomain(
            Communication.Enums.DifficultyEnum difficultyEnum)
        {
            return difficultyEnum switch
            {
                Communication.Enums.DifficultyEnum.Low => Domain.Enums.DifficultyEnum.Low,
                Communication.Enums.DifficultyEnum.Medium => Domain.Enums.DifficultyEnum.Medium,
                Communication.Enums.DifficultyEnum.High => Domain.Enums.DifficultyEnum.High,
                _ => throw new InvalidOperationException(ResourceMessageHelper.FieldNotSupported("Difficulty"))
            };
        }
    }
}