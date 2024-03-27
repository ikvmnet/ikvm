using System;
using System.Buffers;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace IKVM.ByteCode.Buffers
{

    /// <summary>
    /// A <see cref="MemoryManager{byte}"/> over a raw pointer.
    /// </summary>
    sealed unsafe class UnmanagedMemoryManager : MemoryManager<byte>
    {

        readonly void* pointer;
        readonly int length;

        /// <summary>
        /// Create a new UnmanagedMemoryManager instance at the given pointer and size
        /// </summary>
        /// <remarks>It is assumed that the span provided is already unmanaged or externally pinned</remarks>
        public UnmanagedMemoryManager(Span<byte> span)
        {
            pointer = Unsafe.AsPointer(ref MemoryMarshal.GetReference(span));
            length = span.Length;
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="pointer"></param>
        /// <param name="length"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public UnmanagedMemoryManager(byte* pointer, int length)
        {
            if (length < 0)
                throw new ArgumentOutOfRangeException(nameof(length));

            this.pointer = pointer;
            this.length = length;
        }

        /// <inheritdoc />
        public override Span<byte> GetSpan() => new(pointer, length);

        /// <inheritdoc />
        public override MemoryHandle Pin(int elementIndex = 0)
        {
            if (elementIndex < 0 || elementIndex >= length)
                throw new ArgumentOutOfRangeException(nameof(elementIndex));

            return new MemoryHandle(Unsafe.Add<byte>(pointer, elementIndex));
        }

        /// <inheritdoc />
        public override void Unpin()
        {
        }

        /// <inheritdoc />
        protected override void Dispose(bool disposing)
        {

        }

    }

}