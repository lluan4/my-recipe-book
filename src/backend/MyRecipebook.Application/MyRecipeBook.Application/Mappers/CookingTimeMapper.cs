using MyRecipeBook.Exceptions.ExceptionsBase;

namespace MyRecipeBook.Application.Mappers
{
    public static class CookingTimeMapper
    {
        public static Domain.Enums.CookingTimeEnum ToDomain(
            Communication.Enums.CookingTimeEnum communicationEnum)
        {

            return communicationEnum switch
            {
                Communication.Enums.CookingTimeEnum.Less_10_Minutes => Domain.Enums.CookingTimeEnum.Less_10_Minutes,
                Communication.Enums.CookingTimeEnum.Between_10_30_Minutes => Domain.Enums.CookingTimeEnum.Between_10_30_Minutes,
                Communication.Enums.CookingTimeEnum.Between_30_60_Minutes => Domain.Enums.CookingTimeEnum.Between_30_60_Minutes,
                Communication.Enums.CookingTimeEnum.Greather_60_Minutes => Domain.Enums.CookingTimeEnum.Greather_60_Minutes,
                _ => throw new InvalidOperationException(ResourceMessageHelper.FieldNotSupported("CookingTime"))
            };
        }
    }
}