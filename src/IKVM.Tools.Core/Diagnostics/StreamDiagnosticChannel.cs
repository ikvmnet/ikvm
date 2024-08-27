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
        readonly PipeWriter _writer;
        readonly Encoding? _encoding;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="encoding"></param>
        public StreamDiagnosticChannel(Stream stream, Encoding? encoding = null, bool leaveOpen = true)
        {
            _stream = stream;
            _writer = PipeWriter.Create(_stream, new StreamPipeWriterOptions(leaveOpen: leaveOpen));
            _encoding = encoding;
        }

        /// <inheritdoc />
        public PipeWriter Writer => _writer;

        /// <inheritdoc />
        public Encoding? Encoding => _encoding;

        /// <summary>
        /// Disposes of the instance.
        /// </summary>
        public void Dispose()
        {
            _writer.Complete();
        }

    }

}
