using System;
using System.Collections.Concurrent;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using IKVM.CoreLib.Diagnostics;

namespace IKVM.Tools.Core.Diagnostics
{

    /// <summary>
    /// Handles writing diagnostic events to text output.
    /// </summary>
    class TextDiagnosticFormatter : IDiagnosticFormatter, IDisposable
    {

        readonly TextDiagnosticFormatterOptions _options;
        readonly ConcurrentDictionary<IDiagnosticChannel?, TextWriter?> writers = new ConcurrentDictionary<IDiagnosticChannel?, TextWriter?>();
        TextWriter? _traceWriter;
        TextWriter? _infoWriter;
        TextWriter? _warningWriter;
        TextWriter? _errorWriter;
        TextWriter? _fatalWriter;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="options"></param>
        public TextDiagnosticFormatter(TextDiagnosticFormatterOptions options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        /// <inheritdoc />
        public ValueTask WriteAsync(in DiagnosticEvent @event, CancellationToken cancellationToken)
        {
            return WriteAsyncImpl(@event, cancellationToken);
        }

        /// <summary>
        /// Accepts a copy of a diagnostic event.
        /// </summary>
        /// <param name="event"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        async ValueTask WriteAsyncImpl(DiagnosticEvent @event, CancellationToken cancellationToken)
        {
            // find the writer for the event's level
            var dst = @event.Diagnostic.Level switch
            {
                DiagnosticLevel.Trace => GetOrCreateWriter(ref _traceWriter, _options.TraceChannel),
                DiagnosticLevel.Informational => GetOrCreateWriter(ref _infoWriter, _options.InformationChannel),
                DiagnosticLevel.Warning => GetOrCreateWriter(ref _warningWriter, _options.WarningChannel),
                DiagnosticLevel.Error => GetOrCreateWriter(ref _errorWriter, _options.ErrorChannel),
                DiagnosticLevel.Fatal => GetOrCreateWriter(ref _fatalWriter, _options.FatalChannel),
                _ => throw new InvalidOperationException(),
            };

            if (dst == null)
                return;

            // write tag
            await dst.WriteAsync(@event.Diagnostic.Level switch
            {
                DiagnosticLevel.Trace => "trace",
                DiagnosticLevel.Informational => "info",
                DiagnosticLevel.Warning => "warning",
                DiagnosticLevel.Error => "error",
                DiagnosticLevel.Fatal => "error",
                _ => throw new InvalidOperationException(),
            });

            // write event ID
            await dst.WriteAsync($" IKVM{@event.Diagnostic.Id:D4}: ");

            // write message
#if NET8_0_OR_GREATER
            await dst.WriteAsync(string.Format(null, @event.Diagnostic.Message, @event.Args));
#else
            await dst.WriteAsync(string.Format(null, @event.Diagnostic.Message, @event.Args.ToArray()));
#endif

            await dst.WriteLineAsync();
            await dst.FlushAsync();
        }

        /// <summary>
        /// Allocates the text writer for the specified channel.
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="channel"></param>
        /// <returns></returns>
        TextWriter? GetOrCreateWriter(ref TextWriter? writer, IDiagnosticChannel? channel)
        {
            return writer ??= writers.GetOrAdd(channel, c => c != null ? new StreamWriter(c.Writer.AsStream(true), c.Encoding ?? Encoding.UTF8) : null);
        }

        /// <summary>
        /// Disposes of the instance.
        /// </summary>
        public void Dispose()
        {
            foreach (var kvp in writers)
                kvp.Value.Dispose();
        }

    }

}
