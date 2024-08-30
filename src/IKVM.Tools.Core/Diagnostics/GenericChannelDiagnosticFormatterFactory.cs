using System;
using System.Collections.Concurrent;
using System.Linq;

namespace IKVM.Tools.Core.Diagnostics
{

    /// <summary>
    /// Generate channel factory for a formatter that accepts the standard set of chanel options.
    /// </summary>
    /// <typeparam name="TFormatter"></typeparam>
    /// <typeparam name="TOptions"></typeparam>
    abstract class GenericChannelDiagnosticFormatterFactory<TFormatter, TOptions> : IDiagnosticFormatterFactory
        where TFormatter : IDiagnosticFormatter
        where TOptions : DiagnosticChannelFormatterOptions, new()
    {

        readonly DiagnosticChannelProvider _channels;
        readonly string _format;
        readonly string _formatPrefix;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="channels"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public GenericChannelDiagnosticFormatterFactory(DiagnosticChannelProvider channels, string format)
        {
            _channels = channels ?? throw new ArgumentNullException(nameof(channels));
            _format = format ?? throw new ArgumentNullException(nameof(format));
            _formatPrefix = $"{_format},";
        }

        /// <inheritdoc/>
        public IDiagnosticFormatter? GetFormatter(string spec)
        {
            if (spec != _format && spec.StartsWith(_formatPrefix) == false)
                return null;

            // strip format
            var specArray = spec.Split(',');
            if (specArray.Length == 0 || specArray[0] != _format)
                return null;

            // split and parse option information
            var opts = new TOptions();
            var chls = new ConcurrentDictionary<string, IDiagnosticChannel?>();
            foreach (var optionSpec in specArray.Skip(1))
                ParseOption(chls, opts, optionSpec);

            // set default channel if missing
            opts.TraceChannel ??= GetOrCreateChannel(chls, FileDiagnosticChannelFactory.STDOUT);
            opts.InformationChannel ??= GetOrCreateChannel(chls, FileDiagnosticChannelFactory.STDOUT);
            opts.WarningChannel ??= GetOrCreateChannel(chls, FileDiagnosticChannelFactory.STDERR);
            opts.ErrorChannel ??= GetOrCreateChannel(chls, FileDiagnosticChannelFactory.STDERR);
            opts.FatalChannel ??= GetOrCreateChannel(chls, FileDiagnosticChannelFactory.STDERR);

            // process options and create formatter
            return CreateFormatter(opts);
        }

        /// <summary>
        /// Creates the <see cref="TFormatter"/> instance with the specified options.
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        protected abstract TFormatter CreateFormatter(TOptions options);

        /// <summary>
        /// Parses a string option into multiple named components.
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="options"></param>
        /// <param name="spec"></param>
        void ParseOption(ConcurrentDictionary<string, IDiagnosticChannel?> cache, TOptions options, string spec)
        {
            var a = spec.Split(['='], 2, StringSplitOptions.None);
            var key = a.Length >= 1 ? a[0] : "";
            var val = a.Length >= 2 ? a[1] : "";

            if (string.IsNullOrWhiteSpace(key))
                return;

            if (key is "file" or "trace:file")
                options.TraceChannel = GetOrCreateChannel(cache, val);
            if (key is "file" or "info:file")
                options.InformationChannel = GetOrCreateChannel(cache, val);
            if (key is "file" or "warning:file")
                options.WarningChannel = GetOrCreateChannel(cache, val);
            if (key is "file" or "error:file")
                options.ErrorChannel = GetOrCreateChannel(cache, val);
            if (key is "file" or "fatal:file")
                options.FatalChannel = GetOrCreateChannel(cache, val);
        }

        /// <summary>
        /// Gets or creates the diagnostic channel established for the given spec.
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="spec"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        IDiagnosticChannel GetOrCreateChannel(ConcurrentDictionary<string, IDiagnosticChannel?> cache, string spec)
        {
            return cache.GetOrAdd(spec, _channels.FindChannel) ?? throw new InvalidOperationException($"Cannot find channel for specification '{spec}'.");
        }

    }

}
