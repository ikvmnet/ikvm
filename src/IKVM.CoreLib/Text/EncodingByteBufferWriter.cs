using System;
using System.Buffers;
using System.Text;

namespace IKVM.CoreLib.Text
{

    /// <summary>
    /// Provides an interface to write text to a <see cref="IBufferWriter{byte}"/>.
    /// </summary>
    struct EncodingByteBufferWriter
    {

        readonly Encoding _encoding;
        IBufferWriter<byte> _buffer;
        int _bytesWritten;
        int _charsWritten;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="encoding"></param>
        /// <param name="buffer"></param>
        public EncodingByteBufferWriter(Encoding encoding, IBufferWriter<byte> buffer)
        {
            _encoding = encoding ?? throw new ArgumentNullException(nameof(encoding));
            _buffer = buffer ?? throw new ArgumentNullException(nameof(buffer));
        }

        /// <summary>
        /// Gets the number of bytes written.
        /// </summary>
        public readonly int BytesWritten => _bytesWritten;

        /// <summary>
        /// Gets the number of characters written.
        /// </summary>
        public readonly int CharsWritten => _charsWritten;

        /// <summary>
        /// Writes the text to the span, advancing the current position.
        /// </summary>
        /// <param name="text"></param>
        public void Write(string text)
        {
            var l = _encoding.GetByteCount(text);
            var b = _buffer.GetSpan(l);
            new EncodingSpanWriter(_encoding, b).Write(text);
            _buffer.Advance(l);
            _bytesWritten += l;
            _charsWritten += text.Length;
        }

        /// <summary>
        /// Writes the text to the span, advancing the current position.
        /// </summary>
        /// <param name="text"></param>
        public void Write(ReadOnlySpan<char> text)
        {
            var l = _encoding.GetByteCount(text);
            var b = _buffer.GetSpan(l);
            new EncodingSpanWriter(_encoding, b).Write(text);
            _buffer.Advance(l);
            _bytesWritten += l;
            _charsWritten += text.Length;
        }

        /// <summary>
        /// Writes a line feed to the span.
        /// </summary>
        public void WriteLine()
        {
            Write(Environment.NewLine);
        }

    }

}
