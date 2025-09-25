using System;
using System.Buffers;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

namespace IKVM.CoreLib.Buffers
{

    /// <summary>
    /// Implements the <see cref="IBufferWriter{T}"/> interface around a <see cref="Memory{T}"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class MemoryBufferWriter<T> : IBufferWriter<T>
    {

        readonly Memory<T> _memory;
        int _index;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="memory"></param>
        public MemoryBufferWriter(Memory<T> memory)
        {
            _memory = memory;
        }


        /// <inheritdoc/>
        public ReadOnlyMemory<T> WrittenMemory
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _memory.Slice(0, _index);
        }

        /// <inheritdoc/>
        public ReadOnlySpan<T> WrittenSpan
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _memory.Slice(0, _index).Span;
        }

        /// <inheritdoc/>
        public int WrittenCount
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _index;
        }

        /// <inheritdoc/>
        public int Capacity
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _memory.Length;
        }

        /// <inheritdoc/>
        public int FreeCapacity
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _memory.Length - _index;
        }

        /// <inheritdoc/>
        public void Clear()
        {
            _memory.Slice(0, _index).Span.Clear();
            _index = 0;
        }

        /// <inheritdoc/>
        public void Advance(int count)
        {
            if (count < 0)
            {
                ThrowArgumentOutOfRangeExceptionForNegativeCount();
            }

            if (_index > _memory.Length - count)
            {
                ThrowArgumentExceptionForAdvancedTooFar();
            }

            _index += count;
        }

        /// <inheritdoc/>
        public Memory<T> GetMemory(int sizeHint = 0)
        {
            ValidateSizeHint(sizeHint);

            return _memory.Slice(_index);
        }

        /// <inheritdoc/>
        public Span<T> GetSpan(int sizeHint = 0)
        {
            ValidateSizeHint(sizeHint);

            return _memory.Slice(_index).Span;
        }

        /// <summary>
        /// Validates the requested size for either <see cref="GetMemory"/> or <see cref="GetSpan"/>.
        /// </summary>
        /// <param name="sizeHint">The minimum number of items to ensure space for in <see cref="_memory"/>.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void ValidateSizeHint(int sizeHint)
        {
            if (sizeHint < 0)
            {
                ThrowArgumentOutOfRangeExceptionForNegativeSizeHint();
            }

            if (sizeHint == 0)
                sizeHint = 1;

            if (sizeHint > FreeCapacity)
            {
                ThrowArgumentExceptionForCapacityExceeded();
            }
        }

        /// <inheritdoc/>
        [Pure]
        public override string ToString()
        {
            // See comments in MemoryOwner<T> about this
            if (typeof(T) == typeof(char))
            {
                return _memory.Slice(0, _index).ToString();
            }

            // Same representation used in Span<T>
            return $"IKVM.CoreLib.Buffers.MemoryBufferWriter<{typeof(T)}>[{_index}]";
        }

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> when the requested count is negative.
        /// </summary>
        static void ThrowArgumentOutOfRangeExceptionForNegativeCount()
        {
            throw new ArgumentOutOfRangeException("count", "The count can't be a negative value");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> when the size hint is negative.
        /// </summary>
        static void ThrowArgumentOutOfRangeExceptionForNegativeSizeHint()
        {
            throw new ArgumentOutOfRangeException("sizeHint", "The size hint can't be a negative value");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> when the requested count is negative.
        /// </summary>
        static void ThrowArgumentExceptionForAdvancedTooFar()
        {
            throw new ArgumentException("The buffer writer has advanced too far");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when the requested size exceeds the capacity.
        /// </summary>
        static void ThrowArgumentExceptionForCapacityExceeded()
        {
            throw new ArgumentException("The buffer writer doesn't have enough capacity left");
        }

    }

}
