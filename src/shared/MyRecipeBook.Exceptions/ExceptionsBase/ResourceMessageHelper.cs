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
        public static string FieldNotFound(string fieldName)
        {
            return string.Format(ResourceMessagesException.FIELD_NOT_FOUND, fieldName);
        }
        public static string FieldOutOfRange(string fieldName, string firstValue, string secondValue)
        {
            return string.Format(ResourceMessagesException.FIELD_VALUE_OUT_OF_RANGE, fieldName, firstValue, secondValue);
        }
        public static string FieldDuplicateValue(string fieldName)
        {
            return string.Format(ResourceMessagesException.FIELD_DUPLICATE_VALUE, fieldName);
        }
        public static string FieldPatternMismatch(string fieldName, string pattern)
        {
            return string.Format(ResourceMessagesException.FIELD_PATTERN_MISMATCH, fieldName, pattern);
        }
    }
}
