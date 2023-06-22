using System;
using System.Collections.Generic;

namespace IKVM.Compiler.Collections
{

    static class EnumerableExtensions
    {

        /// <summary>
        /// Returns a read-only list version of the given list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static IReadOnlyList<T> AsReadOnly<T>(this T[] list)
        {
            return list;
        }

        /// <summary>
        /// Returns a read-only list version of the given list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static IReadOnlyList<T> AsReadOnly<T>(this IReadOnlyList<T> list)
        {
            return list;
        }

        /// <summary>
        /// Returns a read-only list version of the given list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static IReadOnlyList<T> AsReadOnly<T>(this IEnumerable<T> list)
        {
            if (list is IReadOnlyList<T> r)
                return r;
            else if (list is IList<T> l)
                return new ReadOnlyListList<T>(l);
            else if (list is IReadOnlyCollection<T> c)
                return new ReadOnlyCollectionList<T>(c);
            else
                return new ReadOnlyEnumerableList<T>(list);
        }

        /// <summary>
        /// Returns a new collection that maps the first collection.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TElement"></typeparam>
        /// <param name="source"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static ReadOnlyListMap<TSource, TElement> Map<TSource, TElement>(this TSource[] source, Func<TSource, int, TElement> func)
            where TElement : class
        {
            return new ReadOnlyListMap<TSource, TElement>(source, func);
        }

        /// <summary>
        /// Returns a new collection that maps the first collection.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TElement"></typeparam>
        /// <param name="source"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static ReadOnlyListMap<TSource, TElement> Map<TSource, TElement>(this IReadOnlyList<TSource> source, Func<TSource, int, TElement> func)
            where TElement : class
        {
            return new ReadOnlyListMap<TSource, TElement>(source, func);
        }

        /// <summary>
        /// Returns a new collection that maps the first collection.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TElement"></typeparam>
        /// <param name="source"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static ReadOnlyListMap<TSource, TElement> Map<TSource, TElement>(this IEnumerable<TSource> source, Func<TSource, int, TElement> func)
            where TElement : class
        {
            return new ReadOnlyListMap<TSource, TElement>(source.AsReadOnly(), func);
        }

    }

}
