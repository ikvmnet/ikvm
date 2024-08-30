using System;
using System.Text;

namespace IKVM.CoreLib.Text
{

    /// <summary>
    /// Provides an interface to write text to a <see cref="Span{byte}"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    ref struct EncodingSpanWriter
    {

        readonly Encoding _encoding;
        Span<byte> _span;
        int _bytesWritten;
        int _charsWritten;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="encoding"></param>
        /// <param name="span"></param>
        public EncodingSpanWriter(Encoding encoding, Span<byte> span)
        {
            _encoding = encoding ?? throw new ArgumentNullException(nameof(encoding));
            _span = span;
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
            var l = _encoding.GetBytes(text, _span);
            _span = _span.Slice(l);
            _bytesWritten += l;
            _charsWritten += text.Length;
        }

        /// <summary>
        /// Writes the text to the span, advancing the current position.
        /// </summary>
        /// <param name="text"></param>
        public void Write(scoped ReadOnlySpan<char> text)
        {
            var l = _encoding.GetBytes(text, _span);
            _span = _span.Slice(l);
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
