using System;

using IKVM.Compiler.Collections;

namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Stores the shape of a multi-dimensional array.
    /// </summary>
    internal readonly struct ManagedArrayShape
    {

        readonly int rank;
        readonly ReadOnlyFixedValueList2<int> sizes;
        readonly ReadOnlyFixedValueList2<int> lowerBounds;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="sizes"></param>
        /// <param name="lowerBounds"></param>
        public ManagedArrayShape(int rank, in ReadOnlyFixedValueList2<int> sizes, in ReadOnlyFixedValueList2<int> lowerBounds)
        {
            if (sizes.Count > rank)
                throw new ArgumentOutOfRangeException(nameof(sizes), null, "sizes may be shorter than rank but not longer");
            if (lowerBounds.Count > rank)
                throw new ArgumentOutOfRangeException(nameof(lowerBounds), null, "sizes may be shorter than rank but not longer");

            this.rank = rank;
            this.sizes = sizes;
            this.lowerBounds = lowerBounds;
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="sizes"></param>
        /// <param name="lowerBounds"></param>
        public ManagedArrayShape(int rank, int[] sizes, int[] lowerBounds) :
            this(rank, new ReadOnlyFixedValueList2<int>(new FixedValueList2<int>(sizes)), new ReadOnlyFixedValueList2<int>(new FixedValueList2<int>(lowerBounds)))
        {

        }

        /// <summary>
        /// Gets the number of dimensions in the array.
        /// </summary>
        public readonly int Rank => rank;

        /// <summary>
        /// Gets the lower bound of the given rank.
        /// </summary>
        /// <param name="rank"></param>
        /// <returns></returns>
        public readonly int? GetSize(int rank) => rank < sizes.Count ? sizes[rank] : null;

        /// <summary>
        /// Gets the lower bound of the given rank.
        /// </summary>
        /// <param name="rank"></param>
        /// <returns></returns>
        public readonly int? GetLowerBound(int rank) => rank < lowerBounds.Count ? lowerBounds[rank] : null;

    }

}