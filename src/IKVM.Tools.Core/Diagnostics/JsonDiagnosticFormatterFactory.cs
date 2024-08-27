using System;
using System.Linq;

using Microsoft.Extensions.DependencyInjection;

namespace IKVM.Tools.Core.Diagnostics
{

    /// <summary>
    /// Provides a <see cref="JsonDiagnosticFormatter"/>
    /// </summary>
    class JsonDiagnosticFormatterFactory : IDiagnosticFormatterFactory
    {

        /// <summary>
        /// Adds the JSON diagnostic formater to the <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection Add(IServiceCollection services)
        {
            return services.AddSingleton<IDiagnosticFormatterFactory, JsonDiagnosticFormatterFactory>();
        }

        readonly DiagnosticChannelProvider _channels;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="channels"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public JsonDiagnosticFormatterFactory(DiagnosticChannelProvider channels)
        {
            _channels = channels ?? throw new ArgumentNullException(nameof(channels));
        }

        /// <inheritdoc />
        public IDiagnosticFormatter? GetFormatter(string spec)
        {
            if (spec == "json" || spec.StartsWith("json,"))
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
            if (specArray.Length == 0 || specArray[0] != "json")
                return null;

            // split and parse option information
            var opts = new JsonDiagnosticFormatterOptions();
            foreach (var optionSpec in specArray.Skip(1))
                ParseOption(opts, optionSpec);

            // process options and create formatter
            return new JsonDiagnosticFormatter(opts);
        }

        /// <summary>
        /// Parses a string option into multiple named components.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="spec"></param>
        void ParseOption(JsonDiagnosticFormatterOptions options, string spec)
        {
            var a = spec.Split(['='], 2, StringSplitOptions.None);
            var key = a.Length >= 1 ? a[0] : "";
            var val = a.Length >= 2 ? a[1] : "";

            if (string.IsNullOrWhiteSpace(key))
                return;

            if (key is "file" or "trace:file")
                options.TraceChannel = _channels.FindChannel(val);
            else if (key is "file" or "info:file")
                options.InformationChannel = _channels.FindChannel(val);
            else if (key is "file" or "warning:file")
                options.WarningChannel = _channels.FindChannel(val);
            else if (key is "file" or "error:file")
                options.ErrorChannel = _channels.FindChannel(val);
            else if (key is "file" or "fatal:file")
                options.FatalChannel = _channels.FindChannel(val);
        }

    }

}
