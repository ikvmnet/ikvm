using System.Collections.Generic;

namespace java.util
{

    /// <summary>
    /// Provides extension methods for working against Java <see cref="Iterator"/> instances.
    /// </summary>
    public static class IteratorExtensions
    {

        /// <summary>
        /// Returns the appropriate wrapper type for the given <see cref="Iterator"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="iterator"></param>
        /// <returns></returns>
        public static IEnumerator<T> AsEnumerator<T>(this Iterator iterator) => iterator switch
        {
            IEnumerator<T> i => i,
            Iterator i => new IteratorWrapper<T>(i),
        };

        /// <summary>
        /// Iterators over the items in an iterator and produces an array.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="iterator"></param>
        /// <returns></returns>
        public static List<T> RemainingToList<T>(this Iterator iterator)
        {
            var l = new List<T>();
            while (iterator.hasNext())
                l.Add((T)iterator.next());

            return l;
        }

    }

}
