using System;
using System.Buffers;
using System.Text;

using IKVM.CoreLib.Buffers;
using IKVM.CoreLib.Diagnostics;

namespace IKVM.Tools.Core.Diagnostics
{

    /// <summary>
    /// Formats diagnostic events into a stream of JSON objects.
    /// </summary>
    class JsonDiagnosticFormatter : DiagnosticChannelFormatter<JsonDiagnosticFormatterOptions>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="options"></param>
        public JsonDiagnosticFormatter(JsonDiagnosticFormatterOptions options) :
            base(options)
        {

        }

        /// <inheritdoc />
        protected override void WriteImpl(in DiagnosticEvent @event, IDiagnosticChannel channel)
        {
            var encoding = channel.Encoding ?? Encoding.UTF8;
            using var buffer = MemoryPool<byte>.Shared.Rent(8192);
            var writer = new MemoryBufferWriter<byte>(buffer.Memory);
            JsonDiagnosticFormat.Write(@event.Diagnostic.Id, @event.Diagnostic.Level, @event.Diagnostic.Message, @event.Args, @event.Exception, @event.Location, ref writer, encoding);
            WriteLine(writer, encoding);
            channel.Writer.Write(buffer, writer.WrittenCount);
            return;
        }

        /// <summary>
        /// Writes a new line to the writer.
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="encoding"></param>
        void WriteLine(MemoryBufferWriter<byte> writer, Encoding encoding)
        {
            var buf = (Span<byte>)stackalloc byte[8];
            var len = encoding.GetBytes(Environment.NewLine, buf);
            writer.Write(buf[..len]);
        }

    }

}
