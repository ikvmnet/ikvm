using System;
using System.Buffers.Binary;

namespace IKVM.ByteCode.Writing
{

    /// <summary>
    /// Provides a forward-only writer of big-endian values.
    /// </summary>
    public ref struct ClassFormatWriter
    {

        Span<byte> span;
        Span<byte> next;
        uint size = 0;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="span"></param>
        public ClassFormatWriter(Span<byte> span)
        {
            this.span = this.next = span;
        }

        /// <summary>
        /// Gets the span being written to.
        /// </summary>
        public Span<byte> Span => span;

        /// <summary>
        /// Gets the total number of written bytes.
        /// </summary>
        public uint Size => size;

        /// <summary>
        /// Writes a <see cref="byte" /> to the writer and advances the position.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryWrite(byte value)
        {
            BinaryPrimitives.WriteUInt16BigEndian(next, value);
            next = next.Slice(sizeof(byte));
            size += sizeof(byte);
            return true;
        }

        /// <summary>
        /// Writes a <see cref="ushort" /> to the writer and advances the position.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryWrite(ushort value)
        {
            BinaryPrimitives.WriteUInt16BigEndian(next, value);
            next = next.Slice(sizeof(ushort));
            size += sizeof(ushort);
            return true;
        }

        /// <summary>
        /// Writes a <see cref="uint" /> to the writer and advances the position.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryWrite(uint value)
        {
            BinaryPrimitives.WriteUInt32BigEndian(next, value);
            next = next.Slice(sizeof(uint));
            size += sizeof(uint);
            return true;
        }

    }

}
