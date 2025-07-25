using MyRecipeBook.Exceptions.ExceptionsBase;

namespace MyRecipeBook.Application.Mappers
{
    public static class DifficultyMapper
    {
        public static Domain.Enums.RecipeDifficulty ToDomain(
            Communication.Enums.RecipeDifficulty difficultyEnum)
        {
            return difficultyEnum switch
            {
                Communication.Enums.RecipeDifficulty.Low => Domain.Enums.RecipeDifficulty.Low,
                Communication.Enums.RecipeDifficulty.Medium => Domain.Enums.RecipeDifficulty.Medium,
                Communication.Enums.RecipeDifficulty.High => Domain.Enums.RecipeDifficulty.High,
                _ => throw new InvalidOperationException(ResourceMessageHelper.FieldNotSupported("Difficulty"))
            };
        }
    }
}