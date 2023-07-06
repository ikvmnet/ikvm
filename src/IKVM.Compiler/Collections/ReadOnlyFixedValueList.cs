#nullable disable

using System;
using System.Collections;
using System.Collections.Generic;

namespace IKVM.Compiler.Collections
{

    /// <summary>
    /// A fixed structural <see cref="IReadOnlyList{T}"/> implementation that optimizes for short lists.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public readonly struct ReadOnlyFixedValueList<T> : IReadOnlyList<T>
    {

        /// <summary>
        /// Returns an empty list.
        /// </summary>
        public static readonly ReadOnlyFixedValueList<T> Empty = new ReadOnlyFixedValueList<T>();

        readonly FixedValueList<T> list;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="list"></param>
        public ReadOnlyFixedValueList(in FixedValueList<T> list)
        {
            this.list = list;
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="source"></param>
        public ReadOnlyFixedValueList(ReadOnlySpan<T> source) :
            this(new FixedValueList<T>(source))
        {

        }

        /// <summary>
        /// Gets the item at the specified index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public readonly T this[int index] => list[index];

        /// <summary>
        /// Gets the number of items in the list.
        /// </summary>
        public readonly int Count => list.Count;

        /// <summary>
        /// Gets whether or not this list is empty.
        /// </summary>
        public readonly bool IsEmpty => list.Count == 0;

        /// <summary>
        /// Gets an enumerator for the list.
        /// </summary>
        /// <returns></returns>
        public FixedValueList<T>.Enumerator GetEnumerator()
        {
            return list.GetEnumerator();
        }

        /// <summary>
        /// Gets an enumerator for the list.
        /// </summary>
        /// <returns></returns>
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return ((IEnumerable<T>)list).GetEnumerator();
        }

        /// <summary>
        /// Gets an enumerator for the list.
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)list).GetEnumerator();
        }

    }

}
