using System;
using System.Collections.Generic;
using System.Linq;

namespace DecimalConsole
{
    public static class EnumerableHelper
    {
        public static string ToSimpleString<TSource>(this IEnumerable<TSource> source) => string.Join("", source);

        public static T[] Subarray<T>(this T[] source, int startIndex, int length)
        {
            var a = new T[length];
            Array.Copy(source, startIndex, a, 0, length);
            return a;
        }

        public static T[][] Split<T>(this T[] source, int index)
        {
            var a1 = source.Subarray(0, index);
            var a2 = source.Subarray(index, source.Length - index);
            return new[] { a1, a2 };
        }

        public static T[][] Split<T>(this T[] source, params int[] indexes)
        {
            throw new NotImplementedException();
        }

        public static bool ArrayEquals<T>(this T[] source, T[] second) where T : IEquatable<T> =>
            source.Length == second.Length && Enumerable.Range(0, source.Length).All(i => source[i].Equals(second[i]));
    }
}
