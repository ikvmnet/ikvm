using System;
using System.Buffers;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

using IKVM.CoreLib.Buffers;

namespace IKVM.CoreLib.Diagnostics
{

    /// <summary>
    /// Formats diagnostic events into a stream of JSON objects.
    /// </summary>
    static class JsonDiagnosticFormat
    {

        /// <summary>
        /// Writes the given event data to the specified <see cref="StringWriter"/>.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="level"></param>
        /// <param name="message"></param>
        /// <param name="args"></param>
        /// <param name="exception"></param>
        /// <param name="location"></param>
        /// <param name="writer"></param>
        public static void Write(int id, DiagnosticLevel level, string message, object?[] args, Exception? exception, DiagnosticLocation location, TextWriter writer)
        {
            var buffer = MemoryPool<byte>.Shared.Rent(8192);

            try
            {
                var wrt = new MemoryBufferWriter<byte>(buffer.Memory);
                Write(id, level, message, args, exception, location, ref wrt, writer.Encoding);
                writer.Write(writer.Encoding.GetString(buffer.Memory.Span[..wrt.WrittenCount]));
            }
            finally
            {
                buffer.Dispose();
            }
        }

        /// <summary>
        /// Writes the given event data to the specified <see cref="StringWriter"/>.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="level"></param>
        /// <param name="message"></param>
        /// <param name="args"></param>
        /// <param name="exception"></param>
        /// <param name="location"></param>
        /// <param name="writer"></param>
        public static async ValueTask WriteAsync(int id, DiagnosticLevel level, string message, object?[] args, Exception? exception, DiagnosticLocation location, TextWriter writer, CancellationToken cancellationToken)
        {
            var buffer = MemoryPool<byte>.Shared.Rent(8192);

            try
            {
                var wrt = new MemoryBufferWriter<byte>(buffer.Memory);
                Write(id, level, message, args, exception, location, ref wrt, writer.Encoding);
                await writer.WriteAsync(writer.Encoding.GetString(buffer.Memory.Span[..wrt.WrittenCount]));
            }
            finally
            {
                buffer.Dispose();
            }
        }

        /// <summary>
        /// Writes the given event data to the specified <typeparamref name="TWriter"/>.
        /// </summary>
        /// <typeparam name="TWriter"></typeparam>
        /// <param name="id"></param>
        /// <param name="level"></param>
        /// <param name="message"></param>
        /// <param name="args"></param>
        /// <param name="exception"></param>
        /// <param name="location"></param>
        /// <param name="writer"></param>
        /// <param name="encoding"></param>
        /// <exception cref="NotImplementedException"></exception>
        public static void Write<TWriter>(int id, DiagnosticLevel level, string message, object?[] args, Exception? exception, DiagnosticLocation location, ref TWriter writer, Encoding? encoding = null)
            where TWriter : IBufferWriter<byte>
        {
            encoding ??= Encoding.UTF8;

            var mem = default(IMemoryOwner<byte>);
            var buf = (IBufferWriter<byte>)writer;

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
                    json.WriteNumber("id", id);
                    json.WriteString("level", level switch
                    {
                        DiagnosticLevel.Trace => "trace",
                        DiagnosticLevel.Info => "info",
                        DiagnosticLevel.Warning => "warning",
                        DiagnosticLevel.Error => "error",
                        DiagnosticLevel.Fatal => "fatal",
                        _ => throw new NotImplementedException(),
                    });

                    json.WriteString("message", message);
                    json.WriteStartArray("args");

                    // encode each argument
                    foreach (var arg in args)
                        JsonSerializer.Serialize(json, arg, arg?.GetType() ?? typeof(object), JsonSerializerOptions.Default);

                    json.WriteEndArray();

                    if (location.Path != null ||
                        location.StartLine != 0 ||
                        location.StartColumn != 0 ||
                        location.EndLine != 0 ||
                        location.EndColumn != 0)
                    {
                        json.WriteStartObject("location");
                        json.WriteString("path", location.Path);
                        json.WriteStartArray("position");
                        json.WriteNumberValue(location.StartLine);
                        json.WriteNumberValue(location.StartColumn);
                        json.WriteNumberValue(location.EndLine);
                        json.WriteNumberValue(location.EndColumn);
                        json.WriteEndArray();

                        json.WriteEndObject();
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
            }
            finally
            {
                // dispose of the temporary buffer for non-UTF8
                mem?.Dispose();
            }
        }

    }

}
