namespace MyRecipeBook.Exceptions.ExceptionsBase
{
    public static class ResourceMessageHelper
    {
        public static string FieldEmpty(string fieldName)
        {
            return string.Format(ResourceMessagesException.FIELD_EMPTY, fieldName);
        }

        public static string FieldNotSupported(string fieldName)
        {
            return string.Format(ResourceMessagesException.FIELD_NOT_SUPPORTED, fieldName);
        }

        public static string FieldMustHaveMaxLength(string fieldName, int length)
        {
            return string.Format(ResourceMessagesException.FIELD_MUST_HAVE_MAX_LENGTH, fieldName, length);
        }

        public static string FieldMustHaveAtLeastOne(string fieldName)
        {
            return string.Format(ResourceMessagesException.FIELD_MUST_HAVE_AT_LEAST_ONE, fieldName);
        }

        public static string FieldNonNegative(string fieldName)
        {
            return string.Format(ResourceMessagesException.FIELD_NON_NEGATIVE, fieldName);
        }

        public static string FieldTwoOrMore(string fieldName)
        {
            return string.Format(ResourceMessagesException.FIELD_TWO_OR_MORE_SAME_ORDER, fieldName);
        }
    }
}
