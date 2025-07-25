using MyRecipeBook.Exceptions.ExceptionsBase;

namespace MyRecipeBook.Application.Mappers
{
    public static class DishTypesMapper
    {
        public static Domain.Enums.DishTypeEnum ToDomain(
            Communication.Enums.DishTypeEnum dishType)
        {
            return dishType switch
            {
                Communication.Enums.DishTypeEnum.Snack => Domain.Enums.DishTypeEnum.Snack,
                Communication.Enums.DishTypeEnum.Lunch => Domain.Enums.DishTypeEnum.Lunch,
                Communication.Enums.DishTypeEnum.Appertizers => Domain.Enums.DishTypeEnum.Appertizers,
                Communication.Enums.DishTypeEnum.Breakfast => Domain.Enums.DishTypeEnum.Breakfast,
                Communication.Enums.DishTypeEnum.Drinks => Domain.Enums.DishTypeEnum.Drinks,
                Communication.Enums.DishTypeEnum.Dessert => Domain.Enums.DishTypeEnum.Dessert,
                Communication.Enums.DishTypeEnum.Dinner => Domain.Enums.DishTypeEnum.Dinner,
                _ => throw new InvalidOperationException(ResourceMessageHelper.FieldNotSupported("DishType"))
            };
        }
    }
}