using System.Diagnostics.CodeAnalysis;

namespace MyRecipeBook.Domain.Extension
{
    public static class StringExtension
    {
        public static bool NotEmpty([NotNullWhen(true)]this string? value) => string.IsNullOrEmpty(value).isFalse();
    }
}
