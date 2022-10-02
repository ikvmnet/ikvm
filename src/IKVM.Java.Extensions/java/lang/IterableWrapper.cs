using System;
using System.Collections;
using System.Collections.Generic;

using java.util;

namespace java.lang
{

    /// <summary>
    /// Wraps an <see cref="Iterable"/> as a <see cref="IEnumerable"/>.
    /// </summary>
    class IterableWrapper<T> : IEnumerable<T>
    {

        readonly Iterable iterable;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="iterable"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public IterableWrapper(Iterable iterable)
        {
            this.iterable = iterable ?? throw new ArgumentNullException(nameof(iterable));
        }

        /// <summary>
        /// Returns an enumerator that wraps the given iterable.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<T> GetEnumerator()
        {
            return new IteratorWrapper<T>(iterable.iterator());
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

    }

}