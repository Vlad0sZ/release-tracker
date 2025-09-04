using System;
using JetBrains.Annotations;

namespace Runtime.Extensions
{
    public static class ValueExtensions
    {
        public static void Let<T>([CanBeNull] this T val, Action<T> ifNotNullAction)
        {
            if (val != null)
                ifNotNullAction(val);
        }
    }
}