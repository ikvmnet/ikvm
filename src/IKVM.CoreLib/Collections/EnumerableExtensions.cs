using System;
using System.Collections.Generic;

namespace IKVM.CoreLib.Collections
{

    static class EnumerableExtensions
    {

        public static TSource? SingleOrDefaultOrThrow<TSource>(this IEnumerable<TSource> source, Func<Exception> exception)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (exception == null)
                throw new ArgumentNullException(nameof(exception));

            if (source is IReadOnlyList<TSource> list)
            {
                switch (list.Count)
                {
                    case 0:
                        return default;
                    case 1:
                        return list[0];
                }
            }
            else
            {
                using IEnumerator<TSource> enumerator = source.GetEnumerator();
                if (enumerator.MoveNext() == false)
                    return default;

                var current = enumerator.Current;
                if (enumerator.MoveNext() == false)
                    return current;
            }

            throw exception();
        }

        public static TSource? SingleOrDefaultOrThrow<TSource>(this IEnumerable<TSource> source, Predicate<TSource> predicate, Func<Exception> exception)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));
            if (exception == null)
                throw new ArgumentNullException(nameof(exception));

            var val = default(TSource);
            var num = 0;

            foreach (var item in source)
            {
                if (predicate(item))
                {
                    val = item;
                    if ((++num) >= 2)
                        throw exception();
                }
            }

            return num switch
            {
                0 => default,
                1 => val,
                _ => throw new InvalidOperationException(),
            };
        }

    }

}
