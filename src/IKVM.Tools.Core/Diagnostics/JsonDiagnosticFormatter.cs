using System;
using System.IO;
using System.IO.Pipelines;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

using IKVM.CoreLib.Diagnostics;

namespace IKVM.Tools.Core.Diagnostics
{

    /// <summary>
    /// Formats diagnostic events into a stream of JSON objects.
    /// </summary>
    class JsonDiagnosticFormatter : IDiagnosticFormatter
    {

#if NETFRAMEWORK

        readonly static byte[] Utf8Eol = Encoding.UTF8.GetBytes("\n");

#endif

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
        public ValueTask WriteAsync(in DiagnosticEvent @event, CancellationToken cancellationToken)
        {
            var channel = @event.Diagnostic.Level switch
            {
                DiagnosticLevel.Trace => _options.TraceChannel,
                DiagnosticLevel.Informational => _options.InformationChannel,
                DiagnosticLevel.Warning => _options.WarningChannel,
                DiagnosticLevel.Error => _options.ErrorChannel,
                DiagnosticLevel.Fatal => _options.FatalChannel,
                _ => null,
            };

            // exit immediately if we cannot write
            if (channel == null || channel.Writer == null)
                return new ValueTask(Task.CompletedTask);

            var tmp = new MemoryStream(1024);
            var wrt = new Utf8JsonWriter(tmp);

            wrt.WriteStartObject();
            wrt.WriteNumber("id", @event.Diagnostic.Id);
            wrt.WriteString("name", @event.Diagnostic.Name);
            wrt.WriteString("level", @event.Diagnostic.Level.ToString());
#if NET8_0_OR_GREATER
            wrt.WriteString("message", @event.Diagnostic.Message.Format);
#else
            wrt.WriteString("message", @event.Diagnostic.Message);
#endif
            wrt.WriteStartArray("args");

            // encode each argument
            foreach (var arg in @event.Args.Span)
                JsonSerializer.Serialize(wrt, arg, arg?.GetType() ?? typeof(object), JsonSerializerOptions.Default);

            wrt.WriteEndArray();
            wrt.WriteEndObject();
            wrt.Flush();

#if NETFRAMEWORK
            tmp.Write(Utf8Eol, 0, Utf8Eol.Length);
#else
            tmp.Write("\n"u8);
#endif

            // we can directly write the UTF8 if allowed, else we need to reencode
            if (channel.Encoding is null or { EncodingName: "UTF8" })
                return new ValueTask(tmp.CopyToAsync(channel.Writer, cancellationToken));
            else
                return WriteEncodedAsync(tmp, channel.Encoding, channel.Writer, cancellationToken);
        }

        /// <summary>
        /// Writes the reencoded stream to the output channel.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="encoding"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        async ValueTask WriteEncodedAsync(MemoryStream stream, Encoding encoding, PipeWriter writer, CancellationToken cancellationToken)
        {
            var str = Encoding.UTF8.GetString(stream.ToArray());
            var buf = encoding.GetBytes(str);
            await writer.WriteAsync(buf, cancellationToken);
        }

    }

}
