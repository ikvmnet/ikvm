using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace IKVM.Compiler.Collections
{

    /// <summary>
    /// Base implementation of a list that maps items from a source list on demand.
    /// </summary>
    /// <typeparam name="TElement"></typeparam>
    /// <typeparam name="TSource"></typeparam>
    internal class ReadOnlyListMap<TElement, TSource> : IReadOnlyList<TElement>
        where TElement : class
    {

        readonly IReadOnlyList<TSource> sources;
        readonly Func<TSource, int, TElement> map;
        readonly uint minIndex;

        TElement[] elements;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="sources"></param>
        /// <param name="map"></param>
        /// <param name="minIndex"></param>
        public ReadOnlyListMap(IReadOnlyList<TSource> sources, Func<TSource, int, TElement> map, uint minIndex = 0)
        {
            this.sources = sources ?? throw new ArgumentNullException(nameof(sources));
            this.map = map ?? throw new ArgumentNullException(nameof(map));
            this.minIndex = minIndex;
        }

        /// <summary>
        /// Gets the underlying records that make up the list.
        /// </summary>
        protected IReadOnlyList<TSource> Records => sources;

        /// <summary>
        /// Creates the appropriate reader.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        protected TElement CreateItem(TSource source, int index) => map(source, index);

        /// <summary>
        /// Maps the specified item at the given index.
        /// </summary>
        /// <returns></returns>
        TElement Map(int index)
        {
            if (index < minIndex || index >= sources.Count)
                throw new ArgumentOutOfRangeException(nameof(index));

            // initialize cache if not initialized
            if (elements == null)
                Interlocked.CompareExchange(ref elements, new TElement[sources.Count], null);

            // consult cache
            if (elements[index] is TElement element)
                return element;

            // atomic set, only one winner
            Interlocked.CompareExchange(ref elements[index], CreateItem(sources[index], index), null);
            return elements[index];
        }

        /// <summary>
        /// Gets the item at the specified index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public TElement this[int index] => Map(index);

        /// <summary>
        /// Gets the count of items.
        /// </summary>
        public int Count => sources.Count;

        /// <summary>
        /// Gets an enumerator over each item.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<TElement> GetEnumerator() => Enumerable.Range(0, sources.Count).Select(i => this[i]).GetEnumerator();

        /// <summary>
        /// Gets an enumerator over each item.
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    }

}
