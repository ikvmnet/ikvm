using System;
using System.Collections.Concurrent;
using System.Linq;

namespace IKVM.Tools.Core.Diagnostics
{

    /// <summary>
    /// Provides a <see cref="TextDiagnosticFormatter"/>
    /// </summary>
    class TextDiagnosticFormatterFactory : IDiagnosticFormatterFactory
    {

        readonly DiagnosticChannelProvider _channels;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="channels"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public TextDiagnosticFormatterFactory(DiagnosticChannelProvider channels)
        {
            _channels = channels ?? throw new ArgumentNullException(nameof(channels));
        }

        /// <inheritdoc />
        public IDiagnosticFormatter? GetFormatter(string spec)
        {
            if (spec == "text" || spec.StartsWith("text,"))
                return TryHandle(spec);

            return null;
        }

        /// <summary>
        /// Parses a diagnostic specification into the appropriate formatter.
        /// </summary>
        /// <param name="spec"></param>
        /// <returns></returns>
        IDiagnosticFormatter? TryHandle(string spec)
        {
            // check string for json format
            var specArray = spec.Split(',');
            if (specArray.Length == 0 || specArray[0] != "text")
                return null;

            // split and parse option information
            var opts = new TextDiagnosticFormatterOptions();
            var chls = new ConcurrentDictionary<string, IDiagnosticChannel?>();
            foreach (var optionSpec in specArray.Skip(1))
                ParseOption(chls, opts, optionSpec);

            // process options and create formatter
            return new TextDiagnosticFormatter(opts);
        }

        /// <summary>
        /// Parses a string option into multiple named components.
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="options"></param>
        /// <param name="spec"></param>
        void ParseOption(ConcurrentDictionary<string, IDiagnosticChannel?> cache, TextDiagnosticFormatterOptions options, string spec)
        {
            var a = spec.Split(['='], 2, StringSplitOptions.None);
            var key = a.Length >= 1 ? a[0] : "";
            var val = a.Length >= 2 ? a[1] : "";

            if (string.IsNullOrWhiteSpace(key))
                return;

            if (key is "file" or "trace:file")
                options.TraceChannel = cache.GetOrAdd(val, _channels.FindChannel);
            if (key is "file" or "info:file")
                options.InformationChannel = cache.GetOrAdd(val, _channels.FindChannel);
            if (key is "file" or "warning:file")
                options.WarningChannel = cache.GetOrAdd(val, _channels.FindChannel);
            if (key is "file" or "error:file")
                options.ErrorChannel = cache.GetOrAdd(val, _channels.FindChannel);
            if (key is "file" or "fatal:file")
                options.FatalChannel = cache.GetOrAdd(val, _channels.FindChannel);
        }

    }

}
