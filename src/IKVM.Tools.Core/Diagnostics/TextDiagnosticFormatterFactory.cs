using System;

namespace IKVM.Tools.Core.Diagnostics
{

    /// <summary>
    /// Provides a <see cref="TextDiagnosticFormatter"/>
    /// </summary>
    class TextDiagnosticFormatterFactory :
        GenericChannelDiagnosticFormatterFactory<TextDiagnosticFormatter, TextDiagnosticFormatterOptions>
    {


        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="channels"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public TextDiagnosticFormatterFactory(DiagnosticChannelProvider channels) :
            base(channels, "text")
        {

        }

        /// <inheritdoc />
        protected override TextDiagnosticFormatter CreateFormatter(TextDiagnosticFormatterOptions options)
        {
            return new TextDiagnosticFormatter(options);
        }

    }

}
