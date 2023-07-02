using System;
using System.Collections;
using System.Collections.Generic;

namespace IKVM.Compiler.Collections
{

    internal readonly struct ReadOnlyValueList<T> : IReadOnlyList<T>
    {

        struct ReadOnlyValueListEnumerator : IEnumerator<T>
        {

            readonly ReadOnlyValueList<T> list;
            int position = -1;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="list"></param>
            public ReadOnlyValueListEnumerator(in ReadOnlyValueList<T> list)
            {
                this.list = list;
            }

            /// <summary>
            /// Gets the current item in the list.
            /// </summary>
            public T Current => list[position];

            /// <summary>
            /// Advances to the next entry.
            /// </summary>
            /// <returns></returns>
            public bool MoveNext()
            {
                position++;
                return list.Count >= position;
            }

            /// <summary>
            /// Resets the enumerator.
            /// </summary>
            public void Reset()
            {
                position = -1;
            }

            /// <summary>
            /// Disposes of the instance.
            /// </summary>
            /// <exception cref="NotImplementedException"></exception>
            public void Dispose()
            {
                throw new NotImplementedException();
            }

            object? IEnumerator.Current => Current;

        }

        readonly int count;
        readonly ReadOnlyValueSegment<T> beg;
        readonly ReadOnlyValueSegment<T>[]? rem;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="source"></param>
        public ReadOnlyValueList(ReadOnlySpan<T> source)
        {
            this.count = source.Length;

            // allocate segments
            var num = source.Length / 8;
            rem = num > 1 ? (new ReadOnlyValueSegment<T>[num - 1]) : null;
        }

        /// <summary>
        /// Gets the item at the specified index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public T this[int index] => GetItem(index);

        /// <summary>
        /// Gets the item at the specified index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        T GetItem(int index)
        {
            if (index < 0 || index >= count)
                throw new IndexOutOfRangeException(nameof(index));

            var num = index / 8;
            return GetItemFromSegment(num == 0 ? beg : rem![num - 1], index % 8);
        }

        /// <summary>
        /// Gets the index of the specified segment.
        /// </summary>
        /// <param name="segment"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        T GetItemFromSegment(in ReadOnlyValueSegment<T> segment, int index)
        {
            if (index == 0)
                return segment.item0;
            if (index == 1)
                return segment.item1;
            if (index == 2)
                return segment.item2;
            if (index == 3)
                return segment.item3;
            if (index == 4)
                return segment.item4;
            if (index == 5)
                return segment.item5;
            if (index == 6)
                return segment.item6;
            if (index == 7)
                return segment.item7;

            throw new InvalidOperationException();
        }

        /// <summary>
        /// Gets the number of items in the list.
        /// </summary>
        public int Count => count;

        /// <summary>
        /// Gets an enumerator for the list.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets an enumerator for the list.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

    }

}
