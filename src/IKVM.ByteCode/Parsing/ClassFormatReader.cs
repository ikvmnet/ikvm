using System;
using System.Buffers;

using IKVM.ByteCode.Buffers;

namespace IKVM.ByteCode.Parsing
{

    /// <summary>
    /// Provides common methods to read through memory of a Class file.
    /// </summary>
    public ref struct ClassFormatReader
    {

        SequenceReader<byte> reader;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="buffer"></param>
        public ClassFormatReader(ReadOnlyMemory<byte> buffer) :
            this(new ReadOnlySequence<byte>(buffer))
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="sequence"></param>
        public ClassFormatReader(ReadOnlySequence<byte> sequence)
        {
            reader = new SequenceReader<byte>(sequence);
        }

        /// <summary>
        /// Gets the count of bytes in the reader.
        /// </summary>
        public long Length => reader.Length;

        /// <summary>
        /// Attempts to read a value defined as a 'u1'.
        /// </summary>
        /// <param name="u1"></param>
        /// <returns></returns>
        public bool TryReadU1(out byte u1)
        {
            return reader.TryRead(out u1);
        }

        /// <summary>
        /// Attempts to read a value defined as a 'u2'.
        /// </summary>
        /// <param name="u2"></param>
        /// <returns></returns>
        public bool TryReadU2(out ushort u2)
        {
            return reader.TryReadBigEndian(out u2);
        }

        /// <summary>
        /// Attempts to read a value defined as a 'u4'.
        /// </summary>
        /// <param name="u4"></param>
        /// <returns></returns>
        public bool TryReadU4(out uint u4)
        {
            return reader.TryReadBigEndian(out u4);
        }

        /// <summary>
        /// Attempts to read the exact given number of bytes.
        /// </summary>
        /// <param name="count"></param>
        /// <param name="sequence"></param>
        /// <returns></returns>
        public bool TryReadManyU1(uint count, out ReadOnlySequence<byte> sequence)
        {
            return reader.TryReadExact(count, out sequence);
        }

        /// <summary>
        /// Attempts to read the exact given number of bytes.
        /// </summary>
        /// <param name="count"></param>
        /// <param name="sequence"></param>
        /// <returns></returns>
        public bool TryReadManyU1(long count, out ReadOnlySequence<byte> sequence)
        {
            return reader.TryReadExact(count, out sequence);
        }

        /// <summary>
        /// Attempts to copy available data at the current position to the destination.
        /// </summary>
        /// <param name="destination"></param>
        /// <returns></returns>
        public bool TryCopyTo(Span<byte> destination)
        {
            return reader.TryCopyTo(destination);
        }

    }

}
