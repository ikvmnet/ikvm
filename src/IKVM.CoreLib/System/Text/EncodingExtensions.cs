using System.Buffers;
using System.Text;

namespace System.Text
{

    /// <summary>
    /// Provides extension methods for <see cref="Encoding"/>, <see cref="Encoder"/> and <see cref="Decoder"/>.
    /// </summary>
    static class EncodingExtensions
    {

#if NETFRAMEWORK

        /// <summary>
        /// The maximum number of input elements after which we'll begin to chunk the input.
        /// </summary>
        /// <remarks>
        /// The reason for this chunking is that the existing Encoding / Encoder / Decoder APIs
        /// like GetByteCount / GetCharCount will throw if an integer overflow occurs. Since
        /// we may be working with large inputs in these extension methods, we don't want to
        /// risk running into this issue. While it's technically possible even for 1 million
        /// input elements to result in an overflow condition, such a scenario is unrealistic,
        /// so we won't worry about it.
        /// </remarks>
        const int MaxInputElementsPerIteration = 1 * 1024 * 1024;

        /// <summary>
        /// Encodes the specified <see cref="ReadOnlySpan{Char}"/> to <see langword="byte"/>s using the specified <see cref="Encoding"/>
        /// and writes the result to <paramref name="writer"/>.
        /// </summary>
        /// <param name="encoding">The <see cref="Encoding"/> which represents how the data in <paramref name="chars"/> should be encoded.</param>
        /// <param name="chars">The <see cref="ReadOnlySpan{Char}"/> to encode to <see langword="byte"/>s.</param>
        /// <param name="writer">The buffer to which the encoded bytes will be written.</param>
        /// <exception cref="EncoderFallbackException">Thrown if <paramref name="chars"/> contains data that cannot be encoded and <paramref name="encoding"/> is configured
        /// to throw an exception when such data is seen.</exception>
        public static unsafe long GetBytes(this Encoding encoding, ReadOnlySpan<char> chars, IBufferWriter<byte> writer)
        {
            if (encoding is null)
                throw new ArgumentNullException(nameof(encoding));
            if (writer is null)
                throw new ArgumentNullException(nameof(writer));

            if (chars.Length <= MaxInputElementsPerIteration)
            {
                // The input span is small enough where we can one-shot this.

                int byteCount = encoding.GetByteCount(chars);
                Span<byte> scratchBuffer = writer.GetSpan(byteCount);

                int actualBytesWritten = encoding.GetBytes(chars, scratchBuffer);

                writer.Advance(actualBytesWritten);
                return actualBytesWritten;
            }
            else
            {
                // Allocate a stateful Encoder instance and chunk this.

                encoding.GetEncoder().Convert(chars, writer, flush: true, out long totalBytesWritten, out _);
                return totalBytesWritten;
            }
        }

        public static unsafe int GetBytes(this Encoding encoding, string chars, Span<byte> bytes)
        {
            return encoding.GetBytes(chars.AsSpan(), bytes);
        }

        public static unsafe int GetByteCount(this Encoder encoder, ReadOnlySpan<char> chars, bool flush)
        {
            fixed (char* charsPtr = chars)
            {
                return encoder.GetByteCount(charsPtr, chars.Length, flush);
            }
        }

        /// <summary>
        /// Converts a <see cref="ReadOnlySpan{Char}"/> to bytes using <paramref name="encoder"/> and writes the result to <paramref name="writer"/>.
        /// </summary>
        /// <param name="encoder">The <see cref="Encoder"/> instance which can convert <see langword="char"/>s to <see langword="byte"/>s.</param>
        /// <param name="chars">A sequence of characters to encode.</param>
        /// <param name="writer">The buffer to which the encoded bytes will be written.</param>
        /// <param name="flush"><see langword="true"/> to indicate no further data is to be converted; otherwise <see langword="false"/>.</param>
        /// <param name="bytesUsed">When this method returns, contains the count of <see langword="byte"/>s which were written to <paramref name="writer"/>.</param>
        /// <param name="completed">
        /// When this method returns, contains <see langword="true"/> if <paramref name="encoder"/> contains no partial internal state; otherwise, <see langword="false"/>.
        /// If <paramref name="flush"/> is <see langword="true"/>, this will always be set to <see langword="true"/> when the method returns.
        /// </param>
        /// <exception cref="EncoderFallbackException">Thrown if <paramref name="chars"/> contains data that cannot be encoded and <paramref name="encoder"/> is configured
        /// to throw an exception when such data is seen.</exception>
        public static void Convert(this Encoder encoder, ReadOnlySpan<char> chars, IBufferWriter<byte> writer, bool flush, out long bytesUsed, out bool completed)
        {
            if (encoder is null)
                throw new ArgumentNullException(nameof(encoder));
            if (writer is null)
                throw new ArgumentNullException(nameof(writer));

            // We need to perform at least one iteration of the loop since the encoder could have internal state.

            long totalBytesWritten = 0;

            do
            {
                // If our remaining input is very large, instead truncate it and tell the encoder
                // that there'll be more data after this call. This truncation is only for the
                // purposes of getting the required byte count. Since the writer may give us a span
                // larger than what we asked for, we'll pass the entirety of the remaining data
                // to the transcoding routine, since it may be able to make progress beyond what
                // was initially computed for the truncated input data.

                int byteCountForThisSlice = chars.Length <= MaxInputElementsPerIteration
                  ? encoder.GetByteCount(chars, flush)
                  : encoder.GetByteCount(chars.Slice(0, MaxInputElementsPerIteration), flush: false /* this isn't the end of the data */);

                Span<byte> scratchBuffer = writer.GetSpan(byteCountForThisSlice);

                encoder.Convert(chars, scratchBuffer, flush, out int charsUsedJustNow, out int bytesWrittenJustNow, out completed);

                chars = chars.Slice(charsUsedJustNow);
                writer.Advance(bytesWrittenJustNow);
                totalBytesWritten += bytesWrittenJustNow;
            } while (!chars.IsEmpty);

            bytesUsed = totalBytesWritten;
        }

        public static unsafe void Convert(this Encoder encoder, ReadOnlySpan<char> chars, Span<byte> bytes, bool flush, out int charsUsed, out int bytesUsed, out bool completed)
        {
            fixed (char* charsPtr = chars)
            fixed (byte* bytesPtr = bytes)
            {
                encoder.Convert(charsPtr, chars.Length, bytesPtr, bytes.Length, flush, out charsUsed, out bytesUsed, out completed);
            }
        }

#endif

    }

}
