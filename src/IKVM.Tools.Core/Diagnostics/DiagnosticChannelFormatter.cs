using System;
using System.Collections.Generic;

using IKVM.CoreLib.Diagnostics;

namespace IKVM.Tools.Core.Diagnostics
{

    abstract class DiagnosticChannelFormatter<TOptions> : IDiagnosticFormatter, IDisposable
        where TOptions : DiagnosticChannelFormatterOptions
    {

        readonly TOptions _options;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="options"></param>
        public DiagnosticChannelFormatter(TOptions options)
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
                DiagnosticLevel.Info => _options.InfoChannel,
                DiagnosticLevel.Warning => _options.WarningChannel,
                DiagnosticLevel.Error => _options.ErrorChannel,
                DiagnosticLevel.Fatal => _options.FatalChannel,
                _ => throw new InvalidOperationException(),
            };

            // no channel found for level
            if (channel == null)
                return;

            WriteImpl(@event, channel);
        }

        /// <summary>
        /// Implement this method to write the diagnostic event to the channel.
        /// </summary>
        /// <param name="event"></param>
        /// <param name="channel"></param>
        protected abstract void WriteImpl(in DiagnosticEvent @event, IDiagnosticChannel channel);

        /// <summary>
        /// Disposes of the instance.
        /// </summary>
        public virtual void Dispose()
        {
            var hs = new HashSet<IDisposable>();

            if (_options.TraceChannel is IDisposable trace && hs.Add(trace))
                trace.Dispose();
            if (_options.InfoChannel is IDisposable info && hs.Add(info))
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
