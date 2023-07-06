using System;
using System.Collections.Generic;

using IKVM.Compiler.Collections;

namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Stores the shape of a multi-dimensional array.
    /// </summary>
    internal readonly struct ManagedArrayShape
    {

        readonly int rank;
        readonly ReadOnlyFixedValueList<int> sizes;
        readonly ReadOnlyFixedValueList<int> lowerBounds;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="sizes"></param>
        /// <param name="lowerBounds"></param>
        public ManagedArrayShape(int rank, in ReadOnlyFixedValueList<int> sizes, in ReadOnlyFixedValueList<int> lowerBounds)
        {
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
        public ManagedArrayShape(int rank, ReadOnlySpan<int> sizes, ReadOnlySpan<int> lowerBounds)
        {
            this.rank = rank;

            var s = new FixedValueList<int>(sizes.Length);
            var b = new FixedValueList<int>(lowerBounds.Length);

            for (int i = 0; i < sizes.Length; i++)
                s[i] = sizes[i];

            for (int i = 0; i < lowerBounds.Length; i++)
                b[i] = lowerBounds[i];

            this.sizes = s.AsReadOnly();
            this.lowerBounds = b.AsReadOnly();
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="sizes"></param>
        /// <param name="lowerBounds"></param>
        public ManagedArrayShape(int rank, IList<int> sizes, IList<int> lowerBounds)
        {
            this.rank = rank;

            var s = new FixedValueList<int>(sizes.Count);
            var b = new FixedValueList<int>(lowerBounds.Count);

            for (int i = 0; i < sizes.Count; i++)
                s[i] = sizes[i];

            for (int i = 0; i < lowerBounds.Count; i++)
                b[i] = lowerBounds[i];

            this.sizes = s.AsReadOnly();
            this.lowerBounds = b.AsReadOnly();
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
        public readonly int GetSize(int rank) => sizes[rank];

        /// <summary>
        /// Gets the lower bound of the given rank.
        /// </summary>
        /// <param name="rank"></param>
        /// <returns></returns>
        public readonly int GetLowerBound(int rank) => lowerBounds[rank];

    }

}