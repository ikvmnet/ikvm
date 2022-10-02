using System.Collections;
using System.Collections.Generic;
using System.Linq;

using java.util;

namespace java.lang
{

    /// <summary>
    /// Provides extension methods for working against Java <see cref="Iterable"/> instances.
    /// </summary>
    public static class IterableExtensions
    {

        /// <summary>
        /// Returns the appropriate wrapper type for the given <see cref="Iterable"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="iterable"></param>
        /// <returns></returns>
        public static IEnumerable<T> AsEnumerable<T>(this Iterable iterable) => iterable switch
        {
            IEnumerable<T> i => i,
            IEnumerable i => i.Cast<T>(),
            Collection i => i.AsCollection<T>(),
            Iterable i => new IterableWrapper<T>(i),
        };

    }

}
