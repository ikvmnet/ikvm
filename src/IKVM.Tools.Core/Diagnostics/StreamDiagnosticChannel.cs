using System;
using System.IO;
using System.IO.Pipelines;
using System.Text;

namespace IKVM.Tools.Core.Diagnostics
{

    /// <summary>
    /// Implementation of <see cref="IDiagnosticChannel"/> that writes encoded text to a <see cref="PipeWriter"/>.
    /// </summary>
    class StreamDiagnosticChannel : IDiagnosticChannel, IDisposable
    {

        readonly Stream _stream;
        readonly PipeWriter _pipe;
        readonly AsyncDiagnosticChannelWriter _writer;
        readonly Encoding _encoding;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="encoding"></param>
        /// <param name="leaveOpen"></param>
        public StreamDiagnosticChannel(Stream stream, Encoding encoding, bool leaveOpen = true)
        {
            _stream = stream ?? throw new ArgumentNullException(nameof(stream));
            _pipe = PipeWriter.Create(_stream, new StreamPipeWriterOptions(leaveOpen: leaveOpen));
            _writer = new AsyncDiagnosticChannelWriter(_pipe);
            _encoding = encoding ?? throw new ArgumentNullException(nameof(encoding));
        }

        /// <inheritdoc />
        public IDiagnosticChannelWriter Writer => _writer;

        /// <inheritdoc />
        public Encoding Encoding => _encoding;

        /// <summary>
        /// Disposes of the instance.
        /// </summary>
        public void Dispose()
        {
            // dispose the writer, which causes the background writer to terminate and releases any buffers, but does not close the pipe
            _writer.Dispose();

            // flush and close the pipe, which should close the stream if requested
            _pipe.FlushAsync().AsTask().GetAwaiter().GetResult();
            _pipe.Complete();
        }

    }

}
