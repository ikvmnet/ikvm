using System;

namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Stores the shape of a multi-dimensional array.
    /// </summary>
    internal struct ManagedArrayShape
    {

        readonly int rank;
        readonly int lowerBound0;
        readonly int lowerBound1;
        readonly int lowerBound2;
        readonly int lowerBound3;
        readonly object? next;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="lowerBound0"></param>
        /// <param name="lowerBound1"></param>
        /// <param name="lowerBound2"></param>
        /// <param name="lowerBound3"></param>
        /// <param name="next"></param>
        public ManagedArrayShape(int rank, int lowerBound0, int lowerBound1, int lowerBound2, int lowerBound3, ManagedArrayShape? next)
        {
            if (rank < 1)
                throw new ArgumentOutOfRangeException(nameof(rank));
            if (rank > 4 && next is null)
                throw new ArgumentOutOfRangeException(nameof(rank));

            this.rank = rank;
            this.lowerBound0 = lowerBound0;
            this.lowerBound1 = lowerBound1;
            this.lowerBound2 = lowerBound2;
            this.lowerBound3 = lowerBound3;
            this.next = next;
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="lowerBounds"></param>
        public ManagedArrayShape(ReadOnlySpan<int> lowerBounds)
        {
            // TODO loop lowerbounds, setting bounds and next each time
            throw new NotImplementedException();
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="size"></param>
        /// <param name="lowerBound0"></param>
        public ManagedArrayShape(int size, int lowerBound0, int lowerBound1, int lowerBound2, int lowerBound3) :
            this(size, lowerBound0, lowerBound1, lowerBound2, lowerBound3, null)
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="size"></param>
        /// <param name="lowerBound0"></param>
        public ManagedArrayShape(int size, int lowerBound0, int lowerBound1, int lowerBound2) :
            this(size, lowerBound0, lowerBound1, lowerBound2, 0, null)
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="size"></param>
        /// <param name="lowerBound0"></param>
        public ManagedArrayShape(int size, int lowerBound0, int lowerBound1) :
            this(size, lowerBound0, lowerBound1, 0, 0, null)
        {

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
        public int GetLowerBound(int rank)
        {
            if (rank < this.rank)
                throw new ArgumentOutOfRangeException(nameof(rank));

            // advance to the shape that describes the specified rank
            var p = (rank - 1) / 4;
            var s = (ManagedArrayShape?)this;
            for (int i = 0; i < p; i++)
                s = (ManagedArrayShape?)s.Value.next ?? throw new InvalidOperationException();

            var o = (rank - 1) % 4;
            if (o == 0)
                return s.Value.lowerBound0;
            if (o == 1)
                return s.Value.lowerBound1;
            if (o == 2)
                return s.Value.lowerBound2;
            if (o == 3)
                return s.Value.lowerBound3;

            throw new InvalidOperationException();
        }

    }

}