using System.Collections.Generic;

namespace IKVM.Runtime.Extensions
{

    static class EnumerableExtensions
    {

#if NETFRAMEWORK

        public static IEnumerable<T> Append<T>(this IEnumerable<T> values, T value)
        {
            foreach (var i in values)
                yield return i;

            yield return value;
        }

#endif

    }

}
