using System;
using System.Collections.Generic;

namespace IKVM.CoreLib.System
{

    /// <summary>
    /// Implements an <see cref="IComparer{T}"/> which lexicographically compares a two lists.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class LexicographicListComparer<T> : LexicographicListComparer<T, IComparer<T>>
    {

        /// <summary>
        /// Returns a default <see cref="LexicographicListComparer{T}"/> instance.
        /// </summary>
        public new static readonly LexicographicListComparer<T> Default = new LexicographicListComparer<T>(Comparer<T>.Default);

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="comparer"></param>
        public LexicographicListComparer(IComparer<T> comparer) :
            base(comparer)
        {

        }

    }

    /// <summary>
    /// Implements an <see cref="IComparer{T}"/> which lexicographically compares a two lists.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TComparer"></typeparam>
    internal class LexicographicListComparer<T, TComparer> : LexicographicListComparer<T, IComparer<T>, IReadOnlyList<T>>
    {

        /// <summary>
        /// Returns a default <see cref="LexicographicListComparer{T, TComparer}"/> instance.
        /// </summary>
        public static readonly LexicographicListComparer<T, TComparer> Default = new LexicographicListComparer<T, TComparer>(Comparer<T>.Default);

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="comparer"></param>
        public LexicographicListComparer(IComparer<T> comparer) :
            base(comparer)
        {

        }

    }

    /// <summary>
    /// Implements an <see cref="IComparer{T}"/> which lexicographically compares a two lists: element by element, and
    /// where one list is a prefix of the other, the shorter list first. There is no default instance, since the
    /// elements need a comparer chosen by the caller, such as an ordinal one for strings.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TComparer"></typeparam>
    /// <typeparam name="TList"></typeparam>
    class LexicographicListComparer<T, TComparer, TList> : IComparer<TList>
        where TComparer : IComparer<T>
        where TList : IReadOnlyList<T>
    {

        readonly TComparer _comparer;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="comparer"></param>
        public LexicographicListComparer(TComparer comparer)
        {
            _comparer = comparer;
        }

        /// <inheritdoc />
        public int Compare(TList? x, TList? y)
        {
            if (x == null && y is null)
                return 0;

            if (x == null)
                return +1;

            if (y == null)
                return -1;

            var length = Math.Min(x.Count, y.Count);
            for (int i = 0; i < length; i++)
            {
                var c = _comparer.Compare(x[i], y[i]);
                if (c != 0)
                    return c;
            }

            return x.Count.CompareTo(y.Count);
        }

    }

}
