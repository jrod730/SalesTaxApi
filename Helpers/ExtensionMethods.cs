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
    }
}

