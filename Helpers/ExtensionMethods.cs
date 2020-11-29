using Microsoft;
using System;

namespace Helpers
{
    public static class ExtensionMethods
    {
        public static T ThrowIfNull<T>([ValidatedNotNullAttribute] this T value, string parameterName) where T : class
        {
            if (value == null)
            {
                throw new ArgumentNullException(parameterName);
            }

            return value;
        }

        public static void ThrowIfNull(this string stringTargetObj, string parameterName)
        {
            if (string.IsNullOrWhiteSpace(stringTargetObj))
            {
                throw new ArgumentNullException(parameterName);
            }
        }
    }
}

