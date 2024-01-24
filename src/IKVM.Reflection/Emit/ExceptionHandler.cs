using System;

namespace IKVM.Reflection.Emit
{

    public readonly struct ExceptionHandler : IEquatable<ExceptionHandler>
    {

        public static bool operator ==(ExceptionHandler left, ExceptionHandler right) => left.Equals(right);

        public static bool operator !=(ExceptionHandler left, ExceptionHandler right) => !left.Equals(right);

        readonly int tryStartOffset;
        readonly int tryEndOffset;
        readonly int filterOffset;
        readonly int handlerStartOffset;
        readonly int handlerEndOffset;
        readonly ExceptionHandlingClauseOptions kind;
        readonly int exceptionTypeToken;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="tryOffset"></param>
        /// <param name="tryLength"></param>
        /// <param name="filterOffset"></param>
        /// <param name="handlerOffset"></param>
        /// <param name="handlerLength"></param>
        /// <param name="kind"></param>
        /// <param name="exceptionTypeToken"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="ArgumentException"></exception>
        internal ExceptionHandler(int tryOffset, int tryLength, int filterOffset, int handlerOffset, int handlerLength, ExceptionHandlingClauseOptions kind, int exceptionTypeToken)
        {
            if (tryOffset < 0)
                throw new ArgumentOutOfRangeException(nameof(tryOffset), "Non-negative number required.");
            if (tryLength < 0)
                throw new ArgumentOutOfRangeException(nameof(tryLength), "Non-negative number required.");
            if (filterOffset < 0)
                throw new ArgumentOutOfRangeException(nameof(filterOffset), "Non-negative number required.");
            if (handlerOffset < 0)
                throw new ArgumentOutOfRangeException(nameof(handlerOffset), "Non-negative number required.");
            if (handlerLength < 0)
                throw new ArgumentOutOfRangeException(nameof(handlerLength), "Non-negative number required.");
            if ((long)tryOffset + tryLength > int.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(tryLength), string.Format("Valid values are between {0} and {1}, inclusive.", 0, int.MaxValue - tryOffset));
            if ((long)handlerOffset + handlerLength > int.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(handlerLength), string.Format("Valid values are between {0} and {1}, inclusive.", 0, int.MaxValue - handlerOffset));
            if (kind == ExceptionHandlingClauseOptions.Clause && (exceptionTypeToken & 0x00FFFFFF) == 0)
                throw new ArgumentException("Invalid type token.", nameof(exceptionTypeToken));

            this.tryStartOffset = tryOffset;
            this.tryEndOffset = tryOffset + tryLength;
            this.filterOffset = filterOffset;
            this.handlerStartOffset = handlerOffset;
            this.handlerEndOffset = handlerOffset + handlerLength;
            this.kind = kind;
            this.exceptionTypeToken = exceptionTypeToken;
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="tryStartOffset"></param>
        /// <param name="tryEndOffset"></param>
        /// <param name="filterOffset"></param>
        /// <param name="handlerStartOffset"></param>
        /// <param name="handlerEndOffset"></param>
        /// <param name="kind"></param>
        /// <param name="exceptionTypeToken"></param>
        internal ExceptionHandler(int tryStartOffset, int tryEndOffset, int filterOffset, int handlerStartOffset, int handlerEndOffset, int kind, int exceptionTypeToken)
        {
            this.tryStartOffset = tryStartOffset;
            this.tryEndOffset = tryEndOffset;
            this.filterOffset = filterOffset;
            this.handlerStartOffset = handlerStartOffset;
            this.handlerEndOffset = handlerEndOffset;
            this.kind = (ExceptionHandlingClauseOptions)kind;
            this.exceptionTypeToken = exceptionTypeToken;
        }

        public int TryOffset => tryStartOffset;

        public int TryLength => tryEndOffset - tryStartOffset;

        public int FilterOffset => filterOffset;

        public int HandlerOffset => handlerStartOffset;

        public int HandlerLength => handlerEndOffset - handlerStartOffset;

        public ExceptionHandlingClauseOptions Kind => kind;

        public int ExceptionTypeToken => exceptionTypeToken;

        public bool Equals(ExceptionHandler other)
        {
            return tryStartOffset == other.tryStartOffset
                && tryEndOffset == other.tryEndOffset
                && filterOffset == other.filterOffset
                && handlerStartOffset == other.handlerStartOffset
                && handlerEndOffset == other.handlerEndOffset
                && kind == other.kind
                && exceptionTypeToken == other.exceptionTypeToken;
        }

        public override bool Equals(object obj) => obj is ExceptionHandler other && Equals(other);

        public override int GetHashCode() => tryStartOffset ^ tryEndOffset * 33 ^ filterOffset * 333 ^ handlerStartOffset * 3333 ^ handlerEndOffset * 33333;

    }

}
