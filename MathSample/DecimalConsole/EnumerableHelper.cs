using System;
using System.Collections.Generic;
using System.Linq;

namespace DecimalConsole
{
    public static class EnumerableHelper
    {
        public static string ToSimpleString<TSource>(this IEnumerable<TSource> source) => string.Join("", source);
    }
}
