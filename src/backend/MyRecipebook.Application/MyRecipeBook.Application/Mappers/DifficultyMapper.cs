using MyRecipeBook.Exceptions.ExceptionsBase;

namespace MyRecipeBook.Application.Mappers
{
    public static class DifficultyMapper
    {
        public static Domain.Enums.Difficulty ToDomain(
            Communication.Enums.Difficulty difficultyEnum)
        {
            return difficultyEnum switch
            {
                Communication.Enums.Difficulty.Low => Domain.Enums.Difficulty.Low,
                Communication.Enums.Difficulty.Medium => Domain.Enums.Difficulty.Medium,
                Communication.Enums.Difficulty.High => Domain.Enums.Difficulty.High,
                _ => throw new InvalidOperationException(ResourceMessageHelper.FieldNotSupported("Difficulty"))
            };
        }
    }
}