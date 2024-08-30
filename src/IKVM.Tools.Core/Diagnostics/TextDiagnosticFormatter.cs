using System;
using System.Buffers;
using System.Collections.Generic;
using System.Text;

using IKVM.CoreLib.Diagnostics;
using IKVM.CoreLib.Text;

namespace IKVM.Tools.Core.Diagnostics
{

    /// <summary>
    /// Handles writing diagnostic events to text output.
    /// </summary>
    class TextDiagnosticFormatter : IDiagnosticFormatter, IDisposable
    {

        readonly TextDiagnosticFormatterOptions _options;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="options"></param>
        public TextDiagnosticFormatter(TextDiagnosticFormatterOptions options)
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
                _ => throw new InvalidOperationException(),
            };

            if (channel == null)
                return;

            var wrt = channel.Writer;
            var enc = channel.Encoding ?? Encoding.Default;

            // stage to a string so we can write it in one go
            var buf = MemoryPool<byte>.Shared.Rent(4096);
            var utf = new EncodingSpanWriter(enc, buf.Memory.Span);

            try
            {
                // write tag
                utf.Write(@event.Diagnostic.Level switch
                {
                    DiagnosticLevel.Trace => "trace",
                    DiagnosticLevel.Informational => "info",
                    DiagnosticLevel.Warning => "warning",
                    DiagnosticLevel.Error => "error",
                    DiagnosticLevel.Fatal => "fatal",
                    _ => throw new InvalidOperationException(),
                });

                // write event ID
                utf.Write($" IKVM{@event.Diagnostic.Id:D4}: ");

                // write message
#if NET8_0_OR_GREATER
                utf.Write(string.Format(null, @event.Diagnostic.Message, @event.Args));
#else
                utf.Write(string.Format(null, @event.Diagnostic.Message, @event.Args.ToArray()));
#endif

                utf.WriteLine();
            }
            catch (ArgumentOutOfRangeException)
            {
                // ignore message
            }

            channel.Writer.Write(buf, utf.BytesWritten);
            return;
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
