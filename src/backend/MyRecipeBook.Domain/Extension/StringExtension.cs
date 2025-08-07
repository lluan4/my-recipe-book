using System.Diagnostics.CodeAnalysis;
using System.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace MyRecipeBook.Domain.Extension
{
    public static class StringExtension
    {
        public static bool NotEmpty([NotNullWhen(true)]this string? value) => string.IsNullOrEmpty(value).isFalse();
        public static string CapitalizeFirstLetterExtension(this string? value) =>
            string.IsNullOrWhiteSpace(value) ? value ?? string.Empty : char.ToUpper(value[0]) + value.Substring(1);
    }
}
