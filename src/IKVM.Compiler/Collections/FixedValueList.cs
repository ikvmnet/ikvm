﻿#nullable disable

using System;
using System.Collections;
using System.Collections.Generic;

namespace IKVM.Compiler.Collections
{

    /// <summary>
    /// A fixed structural <see cref="IReadOnlyList{T}"/> implementation that optimizes for short lists.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public struct FixedValueList<T> : IReadOnlyList<T>
    {

        public struct Enumerator : IEnumerator<T>
        {

            readonly FixedValueList<T> list;
            int position = -1;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="list"></param>
            public Enumerator(in FixedValueList<T> list)
            {
                this.list = list;
            }

            /// <summary>
            /// Gets the current item in the list.
            /// </summary>
            public readonly T Current => list[position];

            /// <summary>
            /// Advances to the next entry.
            /// </summary>
            /// <returns></returns>
            public bool MoveNext()
            {
                return list.Count > ++position;
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
            public void Dispose()
            {

            }

            /// <summary>
            /// Gets the current item in the list.
            /// </summary>
            readonly object IEnumerator.Current => Current;

        }

        readonly int count;
        T item0 = default;
        T item1 = default;
        T item2 = default;
        T item3 = default;
        T item4 = default;
        T item5 = default;
        T item6 = default;
        T item7 = default;
        readonly T[] more;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="count"></param>
        public FixedValueList(int count)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            this.count = count;

            if (count > 8)
                more = new T[count - 8];
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="source"></param>
        public FixedValueList(ReadOnlySpan<T> source) :
            this(source.Length)
        {
            if (count > 0)
            {
                item0 = source[0];
                if (count > 1)
                {
                    item1 = source[1];
                    if (count > 2)
                    {
                        item2 = source[2];
                        if (count > 3)
                        {
                            item3 = source[3];
                            if (count > 4)
                            {
                                item4 = source[4];
                                if (count > 5)
                                {
                                    item5 = source[5];
                                    if (count > 6)
                                    {
                                        item6 = source[6];
                                        if (count > 7)
                                        {
                                            item7 = source[7];
                                            if (count > 8)
                                            {
                                                var s = count - 8;
                                                for (int i = 0; i < s; i++)
                                                    more[i] = source[i + 8];
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the item at the specified index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public T this[int index]
        {
            readonly get => GetItem(index);
            set => SetItem(index, value);
        }

        /// <summary>
        /// Gets the item at the specified index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        readonly T GetItem(int index)
        {
            if (index < 0 || index >= count)
                throw new IndexOutOfRangeException(nameof(index));

            if (index >= 8)
                return more![index - 8];
            else if (index == 7)
                return item7;
            else if (index == 6)
                return item6;
            else if (index == 5)
                return item5;
            else if (index == 4)
                return item4;
            else if (index == 3)
                return item3;
            else if (index == 2)
                return item2;
            else if (index == 1)
                return item1;
            else if (index == 0)
                return item0;
            else
                throw new IndexOutOfRangeException();
        }

        /// <summary>
        /// Sets the item at the specified index.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        /// <exception cref="IndexOutOfRangeException"></exception>
        void SetItem(int index, T value)
        {
            if (index < 0 || index >= count)
                throw new IndexOutOfRangeException(nameof(index));

            if (index == 0)
                item0 = value;
            else if (index == 1)
                item1 = value;
            else if (index == 2)
                item2 = value;
            else if (index == 3)
                item3 = value;
            else if (index == 4)
                item4 = value;
            else if (index == 5)
                item5 = value;
            else if (index == 6)
                item6 = value;
            else if (index == 7)
                item7 = value;
            else
                more![index - 8] = value;
        }

        /// <summary>
        /// Gets the number of items in the list.
        /// </summary>
        public readonly int Count => count;

        /// <summary>
        /// Gets an enumerator for the list.
        /// </summary>
        /// <returns></returns>
        public Enumerator GetEnumerator()
        {
            return new Enumerator(this);
        }

        /// <summary>
        /// Gets an enumerator for the list.
        /// </summary>
        /// <returns></returns>
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Gets an enumerator for the list.
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <returns></returns>
        public ReadOnlyFixedValueList<T> AsReadOnly()
        {
            return new ReadOnlyFixedValueList<T>(this);
        }

    }

}