using System;
using System.Collections.Generic;

namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Stores the shape of a multi-dimensional array.
    /// </summary>
    internal struct ManagedArrayShape
    {

        readonly int rank;
        readonly int size0;
        readonly int lowerBound0;
        readonly int size1;
        readonly int lowerBound1;
        readonly int size2;
        readonly int lowerBound2;
        readonly int size3;
        readonly int lowerBound3;
        readonly int[]? moreSizes;
        readonly int[]? moreLowerBounds;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="lowerBounds"></param>
        public ManagedArrayShape(int rank, ReadOnlySpan<int> sizes, ReadOnlySpan<int> lowerBounds)
        {
            this.rank = rank;

            for (int i = 0; i < sizes.Length; i++)
            {
                if (i == 0)
                    size0 = sizes[i];
                if (i == 1)
                    size0 = sizes[i];
                if (i == 2)
                    size0 = sizes[i];
                if (i == 3)
                    size0 = sizes[i];

                var p = i - 4;
                if (p > 0)
                {
                    moreSizes ??= new int[p];
                    moreSizes[p] = sizes[i];
                }
            }

            for (int i = 0; i < lowerBounds.Length; i++)
            {
                if (i == 0)
                    lowerBound0 = lowerBounds[i];
                if (i == 1)
                    lowerBound1 = lowerBounds[i];
                if (i == 2)
                    lowerBound2 = lowerBounds[i];
                if (i == 3)
                    lowerBound3 = lowerBounds[i];

                var p = i - 4;
                if (p > 0)
                {
                    moreLowerBounds ??= new int[p];
                    moreLowerBounds[p] = lowerBounds[i];
                }
            }
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="lowerBounds"></param>
        public ManagedArrayShape(int rank, IList<int> sizes, IList<int> lowerBounds)
        {
            this.rank = rank;

            for (int i = 0; i < sizes.Count; i++)
            {
                if (i == 0)
                    size0 = sizes[i];
                if (i == 1)
                    size0 = sizes[i];
                if (i == 2)
                    size0 = sizes[i];
                if (i == 3)
                    size0 = sizes[i];

                var p = i - 4;
                if (p > 0)
                {
                    moreSizes ??= new int[p];
                    moreSizes[p] = sizes[i];
                }
            }

            for (int i = 0; i < lowerBounds.Count; i++)
            {
                if (i == 0)
                    lowerBound0 = lowerBounds[i];
                if (i == 1)
                    lowerBound1 = lowerBounds[i];
                if (i == 2)
                    lowerBound2 = lowerBounds[i];
                if (i == 3)
                    lowerBound3 = lowerBounds[i];

                var p = i - 4;
                if (p > 0)
                {
                    moreLowerBounds ??= new int[p];
                    moreLowerBounds[p] = lowerBounds[i];
                }
            }
        }

        /// <summary>
        /// Gets the number of dimensions in the array.
        /// </summary>
        public int Rank => rank;

        /// <summary>
        /// Gets the lower bound of the given rank.
        /// </summary>
        /// <param name="rank"></param>
        /// <returns></returns>
        public int GetSize(int rank)
        {
            if (rank < this.rank)
                throw new ArgumentOutOfRangeException(nameof(rank));

            if (rank == 0)
                return size0;
            if (rank == 1)
                return size1;
            if (rank == 2)
                return size2;
            if (rank == 3)
                return size3;

            var p = rank - 4;
            if (moreSizes == null || moreSizes.Length < p)
                return -1;

            return moreSizes[p];
        }

        /// <summary>
        /// Gets the lower bound of the given rank.
        /// </summary>
        /// <param name="rank"></param>
        /// <returns></returns>
        public int GetLowerBound(int rank)
        {
            if (rank < this.rank)
                throw new ArgumentOutOfRangeException(nameof(rank));

            if (rank == 0)
                return lowerBound0;
            if (rank == 1)
                return lowerBound1;
            if (rank == 2)
                return lowerBound2;
            if (rank == 3)
                return lowerBound3;

            var p = rank - 4;
            if (moreLowerBounds == null || moreLowerBounds.Length < p)
                return -1;

            return moreLowerBounds[p];
        }

    }

}