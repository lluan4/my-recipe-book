namespace MyRecipeBook.Domain.Extension
{
    public static class BooleanExtension
    {
        public static bool isFalse(this bool value) => !value;
    }
}
