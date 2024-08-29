using System;
using System.Buffers;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

using IKVM.CoreLib.Buffers;
using IKVM.CoreLib.Diagnostics;
using IKVM.CoreLib.Text;

namespace IKVM.Tools.Core.Diagnostics
{

    /// <summary>
    /// Formats diagnostic events into a stream of JSON objects.
    /// </summary>
    class JsonDiagnosticFormatter : IDiagnosticFormatter, IDisposable
    {

        readonly JsonDiagnosticFormatterOptions _options;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="options"></param>
        public JsonDiagnosticFormatter(JsonDiagnosticFormatterOptions options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        /// <inheritdoc />
        public void Write(in DiagnosticEvent @event)
        {
            // find the writer for the event's level
            var channel = @event.Diagnostic.Level switch
            {
                DiagnosticLevel.Trace => _options.TraceChannel,
                DiagnosticLevel.Informational => _options.InformationChannel,
                DiagnosticLevel.Warning => _options.WarningChannel,
                DiagnosticLevel.Error => _options.ErrorChannel,
                DiagnosticLevel.Fatal => _options.FatalChannel,
                _ => null,
            };

            if (channel == null)
                return;

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
                foreach (var arg in @event.Args.Span)
                    JsonSerializer.Serialize(json, arg, arg?.GetType() ?? typeof(object), JsonSerializerOptions.Default);

                json.WriteEndArray();
                json.WriteEndObject();

                // ensure the JSON is flushed out
                json.Flush();
                json.Dispose();

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

        /// <summary>
        /// Disposes of the instance.
        /// </summary>
        public void Dispose()
        {
            var hs = new HashSet<IDisposable>();

            if (_options.TraceChannel is IDisposable trace && hs.Add(trace))
                trace.Dispose();
            if (_options.InformationChannel is IDisposable info && hs.Add(info))
                info.Dispose();
            if (_options.WarningChannel is IDisposable warning && hs.Add(warning))
                warning.Dispose();
            if (_options.ErrorChannel is IDisposable error && hs.Add(error))
                error.Dispose();
            if (_options.FatalChannel is IDisposable fatal && hs.Add(fatal))
                fatal.Dispose();
        }

    }

}
