using System;
using System.Collections;
using System.Collections.Generic;

namespace IKVM.Compiler.Collections
{

    /// <summary>
    /// Provides an implementation of <see cref="IReadOnlyList{T}"/> over a <see cref="IReadOnlyCollection{T}"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class ReadOnlyListList<T> : IReadOnlyList<T>
    {

        readonly IList<T> source;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="source"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ReadOnlyListList(IList<T> source)
        {
            this.source = source ?? throw new ArgumentNullException(nameof(source));
        }

        /// <summary>
        /// Gets the item at the specified position.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public T this[int index] => source[index];

        public int Count => source.Count;

        public IEnumerator<T> GetEnumerator() => source.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => source.GetEnumerator();

    }

}
