using System.Buffers;

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
            // stage string in buffer so we can write it to the channel in one call
            using var buffer = MemoryPool<byte>.Shared.Rent(8192);
            var writer = new MemoryBufferWriter<byte>(buffer.Memory);
            JsonDiagnosticFormat.Write(@event.Diagnostic.Id, @event.Diagnostic.Level, @event.Diagnostic.Message, @event.Args, @event.Exception, @event.Location, ref writer, channel.Encoding);
            channel.Writer.Write(buffer, writer.WrittenCount);
            return;
        }

    }

}
