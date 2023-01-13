using System;
using System.Buffers;

using IKVM.ByteCode.Buffers;

namespace IKVM.ByteCode
{

    /// <summary>
    /// Represents an error that occurred with the format of the class file.
    /// </summary>
    public class ClassParserException : Exception
    {

        readonly long consumed;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        internal ClassParserException(in SequenceReader<byte> reader) :
            this(reader, $"Unexpected end of data after {reader.Consumed}.")
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="message"></param>
        internal ClassParserException(in SequenceReader<byte> reader, string message) :
            base(message)
        {
            this.consumed = reader.Consumed;
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        internal ClassParserException(in SequenceReader<byte> reader, string message, Exception innerException) :
            base(message, innerException)
        {
            this.consumed = reader.Consumed;
        }

        /// <summary>
        /// Gets the number of bytes that had been read up until the format exception.
        /// </summary>
        public long Consumed => consumed;

    }

}
