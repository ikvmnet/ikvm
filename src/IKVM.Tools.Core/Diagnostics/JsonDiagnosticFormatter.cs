using System;
using System.Buffers;
using System.Text;
using System.Text.Json;

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
            var wrt = channel.Writer;
            var enc = channel.Encoding ?? Encoding.Default;

            // stage to a buffer so we can write it in one go
            var mem = MemoryPool<byte>.Shared.Rent(4096);
            var buf = new MemoryBufferWriter<byte>(mem.Memory);
            using var json = new Utf8JsonWriter(buf, new JsonWriterOptions() { Indented = false });

            try
            {
                json.WriteStartObject();
                json.WriteNumber("id", @event.Diagnostic.Id);
                json.WriteString("name", @event.Diagnostic.Name);
                json.WriteString("level", @event.Diagnostic.Level.ToString());
#if NET8_0_OR_GREATER
                json.WriteString("message", @event.Diagnostic.Message.Format);
#else
                json.WriteString("message", @event.Diagnostic.Message);
#endif
                json.WriteStartArray("args");

                // encode each argument
                foreach (var arg in @event.Args)
                    JsonSerializer.Serialize(json, arg, arg?.GetType() ?? typeof(object), JsonSerializerOptions.Default);

                json.WriteEndArray();
                json.WriteEndObject();

                // ensure the JSON is flushed out
                json.Flush();
                json.Dispose();

                // write EOL
                enc.GetBytes(Environment.NewLine.AsSpan(), buf);

                // destination encoding is not UTF8, so convert
                if (enc is not UTF8Encoding)
                {
                    // decode into chars
                    using var tmp = MemoryPool<char>.Shared.Rent(Encoding.UTF8.GetCharCount(mem.Memory.Span.Slice(0, buf.WrittenCount)));
                    var len = Encoding.UTF8.GetChars(mem.Memory.Span.Slice(0, buf.WrittenCount), tmp.Memory.Span);

                    // reencode into target encoding
                    buf.Clear();
                    enc.GetBytes(tmp.Memory.Span.Slice(0, len), buf);
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                // ignore large message
            }

            // write the memory
            wrt.Write(mem, buf.WrittenCount);
        }

    }

}
