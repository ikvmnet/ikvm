using System.Buffers;

using IKVM.CoreLib.Buffers;
using IKVM.CoreLib.Diagnostics;

namespace IKVM.Tools.Core.Diagnostics
{

    /// <summary>
    /// Handles writing diagnostic events to text output.
    /// </summary>
    class TextDiagnosticFormatter : DiagnosticChannelFormatter<TextDiagnosticFormatterOptions>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="options"></param>
        public TextDiagnosticFormatter(TextDiagnosticFormatterOptions options) :
            base(options)
        {

        }

        /// <inheritdoc />
        protected override void WriteImpl(in DiagnosticEvent @event, IDiagnosticChannel channel)
        {
            // stage string in buffer so we can write it to the channel in one call
            using var buffer = MemoryPool<byte>.Shared.Rent(8192);
            var writer = new MemoryBufferWriter<byte>(buffer.Memory);
            TextDiagnosticFormat.Write(@event, writer, channel.Encoding);
            channel.Writer.Write(buffer, writer.WrittenCount);
            return;
        }

    }

}
