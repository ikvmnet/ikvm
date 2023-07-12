using System;

using IKVM.Compiler.Collections;

namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Stores the shape of a multi-dimensional array.
    /// </summary>
    internal readonly struct ManagedArrayShape
    {


        /// <summary>
        /// Gets the number of dimensions in the array.
        /// </summary>
        public readonly int Rank;

        /// <summary>
        /// Describes the size of each rank.
        /// </summary>
        public readonly FixedValueList2<int> Sizes;

        /// <summary>
        /// Describes the lower bounds of each rank.
        /// </summary>
        public readonly FixedValueList2<int> LowerBounds;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="rank"></param>
        public ManagedArrayShape(int rank)
        {
            Rank = rank;
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="sizes"></param>
        /// <param name="lowerBounds"></param>
        public ManagedArrayShape(int rank, in FixedValueList2<int> sizes, in FixedValueList2<int> lowerBounds)
        {
            if (sizes.Count > rank)
                throw new ArgumentOutOfRangeException(nameof(sizes), null, "sizes may be shorter than rank but not longer");
            if (lowerBounds.Count > rank)
                throw new ArgumentOutOfRangeException(nameof(lowerBounds), null, "sizes may be shorter than rank but not longer");

            Rank = rank;
            Sizes = sizes;
            LowerBounds = lowerBounds;
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="sizes"></param>
        /// <param name="lowerBounds"></param>
        public ManagedArrayShape(int rank, int[] sizes, int[] lowerBounds) :
            this(rank, new FixedValueList2<int>(new FixedValueList2<int>(sizes)), new FixedValueList2<int>(new FixedValueList2<int>(lowerBounds)))
        {

        }

        /// <summary>
        /// Gets the lower bound of the given rank.
        /// </summary>
        /// <param name="rank"></param>
        /// <returns></returns>
        public readonly int? GetSize(int rank) => rank < Sizes.Count ? Sizes[rank] : null;

        /// <summary>
        /// Gets the lower bound of the given rank.
        /// </summary>
        /// <param name="rank"></param>
        /// <returns></returns>
        public readonly int? GetLowerBound(int rank) => rank < LowerBounds.Count ? LowerBounds[rank] : null;

    }

}