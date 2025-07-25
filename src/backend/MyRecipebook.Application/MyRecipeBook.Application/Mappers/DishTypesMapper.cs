using MyRecipeBook.Exceptions.ExceptionsBase;

namespace MyRecipeBook.Application.Mappers
{
    public static class DishTypesMapper
    {
        public static Domain.Enums.RecipeDishType ToDomain(
            Communication.Enums.RecipeDishType dishType)
        {
            return dishType switch
            {
                Communication.Enums.RecipeDishType.Snack => Domain.Enums.RecipeDishType.Snack,
                Communication.Enums.RecipeDishType.Lunch => Domain.Enums.RecipeDishType.Lunch,
                Communication.Enums.RecipeDishType.Appertizers => Domain.Enums.RecipeDishType.Appertizers,
                Communication.Enums.RecipeDishType.Breakfast => Domain.Enums.RecipeDishType.Breakfast,
                Communication.Enums.RecipeDishType.Drinks => Domain.Enums.RecipeDishType.Drinks,
                Communication.Enums.RecipeDishType.Dessert => Domain.Enums.RecipeDishType.Dessert,
                Communication.Enums.RecipeDishType.Dinner => Domain.Enums.RecipeDishType.Dinner,
                _ => throw new InvalidOperationException(ResourceMessageHelper.FieldNotSupported("DishType"))
            };
        }
    }
}