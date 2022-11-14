using System.Collections.Generic;

namespace java.util
{

    /// <summary>
    /// Provides extension methods for working against Java <see cref="Enumeration"/> instances.
    /// </summary>
    public static class EnumerationExtensions
    {

        /// <summary>
        /// Returns the appropriate wrapper type for the given <see cref="Enumeration"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumeration"></param>
        /// <returns></returns>
        public static IEnumerator<T> AsEnumerator<T>(this Enumeration enumeration) => enumeration switch
        {
            IEnumerator<T> i => i,
            Enumeration i => new EnumerationWrapper<T>(i),
        };

        /// <summary>
        /// Returns the appropriate wrapper type for the given <see cref="Enumeration"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumeration"></param>
        /// <returns></returns>
        public static IEnumerable<T> AsEnumerable<T>(this Enumeration enumeration)
        {
            var e = enumeration.AsEnumerator<T>();
            while (e.MoveNext())
                yield return e.Current;
        }

    }

}
