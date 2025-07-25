using MyRecipeBook.Exceptions.ExceptionsBase;

namespace MyRecipeBook.Application.Mappers
{
    public static class CookingTimeMapper
    {
        public static Domain.Enums.CookingTime ToDomain(
            Communication.Enums.CookingTime communicationEnum)
        {

            return communicationEnum switch
            {
                Communication.Enums.CookingTime.Less_10_Minutes => Domain.Enums.CookingTime.Less_10_Minutes,
                Communication.Enums.CookingTime.Between_10_30_Minutes => Domain.Enums.CookingTime.Between_10_30_Minutes,
                Communication.Enums.CookingTime.Between_30_60_Minutes => Domain.Enums.CookingTime.Between_30_60_Minutes,
                Communication.Enums.CookingTime.Greather_60_Minutes => Domain.Enums.CookingTime.Greather_60_Minutes,
                _ => throw new InvalidOperationException(ResourceMessageHelper.FieldNotSupported("CookingTime"))
            };
        }
    }
}