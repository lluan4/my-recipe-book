using MyRecipeBook.Exceptions.ExceptionsBase;

namespace MyRecipeBook.Application.Mappers
{
    public static class CookingTimeMapper
    {
        public static Domain.Enums.RecipeCookingTime ToDomain(
            Communication.Enums.RecipeCookingTime communicationEnum)
        {

            return communicationEnum switch
            {
                Communication.Enums.RecipeCookingTime.Less_10_Minutes => Domain.Enums.RecipeCookingTime.Less_10_Minutes,
                Communication.Enums.RecipeCookingTime.Between_10_30_Minutes => Domain.Enums.RecipeCookingTime.Between_10_30_Minutes,
                Communication.Enums.RecipeCookingTime.Between_30_60_Minutes => Domain.Enums.RecipeCookingTime.Between_30_60_Minutes,
                Communication.Enums.RecipeCookingTime.Greater_60_Minutes => Domain.Enums.RecipeCookingTime.Greater_60_Minutes,
                _ => throw new InvalidOperationException(ResourceMessageHelper.FieldNotSupported("CookingTime"))
            };
        }
    }
}