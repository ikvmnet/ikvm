using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace IKVM.Compiler.Collections
{

    /// <summary>
    /// Provides an implementation of <see cref="IReadOnlyList{T}"/> over a <see cref="IEnumerable{T}"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class ReadOnlyEnumerableList<T> : IReadOnlyList<T>
    {

        readonly IEnumerable<T> source;

        IList<T> values;
        IEnumerator<T> cursor;
        int position = -1;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="source"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ReadOnlyEnumerableList(IEnumerable<T> source)
        {
            this.source = source ?? throw new ArgumentNullException(nameof(source));

            if (source is IReadOnlyCollection<T> c)
                values = new T[c.Count];
        }

        /// <summary>
        /// Gets the item at the specified position.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public T this[int index] => Resolve(index);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        T Resolve(int index)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index));

            // initialize cache if not initialized
            if (values == null)
                Interlocked.CompareExchange(ref values, new List<T>(), null);

            // initialize enumerator if not initalized
            if (cursor == null)
                Interlocked.CompareExchange(ref cursor, values.GetEnumerator(), null);

            // lock the cache as we read in records to the requested index
            lock (values)
            {
                while (position < index)
                {
                    position++;
                    if (cursor.MoveNext() == false)
                        break;

                    values[position] = cursor.Current;
                }
            }

            // return value, can throw out of range exception if we don't have enough
            return values[index];
        }

        public int Count => values?.Count ?? throw new NotSupportedException("Cannot retrieve the length of an uninitialized read-only list.");

        public IEnumerator<T> GetEnumerator() => source.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => source.GetEnumerator();

    }

}
