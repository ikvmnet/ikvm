using System;
using System.Buffers;

using IKVM.Runtime;
using IKVM.Runtime.Accessors.Java.Lang;

namespace IKVM.Runtime.Util.Java.Nio
{

#if FIRST_PASS == false

    /// <summary>
    /// Provides a <see cref="MemoryManager{T}"/> implementation backed by a Java DirectBuffer. This is a bit backward
    /// because ultimatly Java owns the memory, and we just use this manager to obtain a <see cref="Memory{byte}"/>
    /// reference to it. Ideally, we would override DirectBuffer in the JDK to allocate from the runtime MemoryPool.
    /// </summary>
    class DirectBufferMemoryManager : MemoryManager<byte>
    {

        static BufferAccessor bufferAccessor;
        static BufferAccessor BufferAccessor => JVM.BaseAccessors.Get(ref bufferAccessor);

        readonly object buffer;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public DirectBufferMemoryManager(object buffer)
        {
            this.buffer = buffer ?? throw new ArgumentNullException(nameof(buffer));
        }

        /// <inheritdoc />
        public override unsafe Span<byte> GetSpan() => new Span<byte>((byte*)(IntPtr)BufferAccessor.GetAddress(buffer), BufferAccessor.GetCapacity(buffer));

        /// <inheritdoc />
        public override unsafe MemoryHandle Pin(int elementIndex = 0)
        {
            if (elementIndex < 0 || elementIndex >= bufferAccessor.GetCapacity(buffer))
                throw new ArgumentOutOfRangeException(nameof(elementIndex));

            return new MemoryHandle((byte*)(IntPtr)(bufferAccessor.GetAddress(buffer)) + elementIndex);
        }

        /// <inheritdoc />
        public override void Unpin()
        {

        }

        /// <inheritdoc />
        protected override void Dispose(bool disposing)
        {
            // Java deals with this through the use of a Cleaner
            // TODO I think we should own this
        }

    }

#endif

}
