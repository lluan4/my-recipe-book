using MyRecipeBook.Exceptions.ExceptionsBase;

namespace MyRecipeBook.Application.Mappers
{
    public static class DishTypesMapper
    {
        public static Domain.Enums.DishType ToDomain(
            Communication.Enums.DishType dishType)
        {
            return dishType switch
            {
                Communication.Enums.DishType.Snack => Domain.Enums.DishType.Snack,
                Communication.Enums.DishType.Lunch => Domain.Enums.DishType.Lunch,
                Communication.Enums.DishType.Appertizers => Domain.Enums.DishType.Appertizers,
                Communication.Enums.DishType.Breakfast => Domain.Enums.DishType.Breakfast,
                Communication.Enums.DishType.Drinks => Domain.Enums.DishType.Drinks,
                Communication.Enums.DishType.Dessert => Domain.Enums.DishType.Dessert,
                Communication.Enums.DishType.Dinner => Domain.Enums.DishType.Dinner,
                _ => throw new InvalidOperationException(ResourceMessageHelper.FieldNotSupported("DishType"))
            };
        }
    }
}