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
    static class JsonDiagnosticFormat
    {

        /// <summary>
        /// Writes the <see cref="DiagnosticEvent"/> in the common JSON format to the specified writer.
        /// </summary>
        /// <param name="event"></param>
        /// <param name="writer"></param>
        /// <param name="encoding"></param>
        /// <exception cref="NotImplementedException"></exception>
        public static void Write(in DiagnosticEvent @event, IBufferWriter<byte> writer, Encoding? encoding = null)
        {
            encoding ??= Encoding.UTF8;

            var mem = default(IMemoryOwner<byte>);
            var buf = writer;

            try
            {
                // if we're not writing UTF8, stage to a temporary buffer
                if (encoding is not UTF8Encoding)
                {
                    mem = MemoryPool<byte>.Shared.Rent(8192);
                    buf = new MemoryBufferWriter<byte>(mem.Memory);
                }

                using var json = new Utf8JsonWriter(buf, new JsonWriterOptions() { Indented = false });

                try
                {
                    json.WriteStartObject();
                    json.WriteNumber("id", @event.Diagnostic.Id);
                    json.WriteString("name", @event.Diagnostic.Name);
                    json.WriteString("level", @event.Diagnostic.Level switch
                    {
                        DiagnosticLevel.Trace => "trace",
                        DiagnosticLevel.Info => "info",
                        DiagnosticLevel.Warning => "warning",
                        DiagnosticLevel.Error => "error",
                        DiagnosticLevel.Fatal => "fatal",
                        _ => throw new NotImplementedException(),
                    });

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

                    if (@event.Location.StartLine != 0 ||
                        @event.Location.StartColumn != 0 ||
                        @event.Location.EndLine != 0 ||
                        @event.Location.EndColumn != 0)
                    {
                        json.WriteStartArray("location");
                        json.WriteNumberValue(@event.Location.StartLine);
                        json.WriteNumberValue(@event.Location.StartColumn);
                        json.WriteNumberValue(@event.Location.EndLine);
                        json.WriteNumberValue(@event.Location.EndColumn);
                        json.WriteEndArray();
                    }

                    json.WriteEndObject();

                    // ensure the JSON is flushed out
                    json.Flush();

                    // destination encoding is not UTF8, so convert
                    if (mem != null)
                    {
                        // decode into chars
                        var len = Encoding.UTF8.GetCharCount(mem.Memory.Span[..(int)json.BytesCommitted]);
                        using var tmp = MemoryPool<char>.Shared.Rent(len);
                        len = Encoding.UTF8.GetChars(mem.Memory.Span[..len], tmp.Memory.Span);

                        // reencode into target encoding
                        encoding.GetBytes(tmp.Memory.Span[..len], writer);
                    }
                }
                catch (ArgumentOutOfRangeException)
                {
                    // ignore large message
                }

                // append a newline to the end of the buffer
                var nlLen = encoding.GetByteCount(Environment.NewLine);
                var nlBuf = (Span<byte>)stackalloc byte[nlLen];
                encoding.GetBytes(Environment.NewLine, nlBuf);
                writer.Write(nlBuf);
            }
            finally
            {
                // dispose of the temporary buffer for non-UTF8
                mem?.Dispose();
            }
        }

    }

}
