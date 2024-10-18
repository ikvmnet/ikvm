using System;

namespace IKVM.Tools.Core.Diagnostics
{

    /// <summary>
    /// Provides a <see cref="JsonDiagnosticFormatter"/>
    /// </summary>
    class JsonDiagnosticFormatterFactory :
        GenericChannelDiagnosticFormatterFactory<JsonDiagnosticFormatter, JsonDiagnosticFormatterOptions>
    {


        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="channels"></param>
        /// <param name="options"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public JsonDiagnosticFormatterFactory(DiagnosticChannelProvider channels, DiagnosticOptions options) :
            base(channels, options, "json")
        {

        }

        /// <inheritdoc />
        protected override JsonDiagnosticFormatter CreateFormatter(JsonDiagnosticFormatterOptions options)
        {
            return new JsonDiagnosticFormatter(options);
        }

    }

}
