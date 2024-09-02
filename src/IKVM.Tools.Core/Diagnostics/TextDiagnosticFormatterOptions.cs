namespace IKVM.Tools.Core.Diagnostics
{

    class TextDiagnosticFormatterOptions : DiagnosticChannelFormatterOptions
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public TextDiagnosticFormatterOptions() :
            base()
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="defaultChannel"></param>
        public TextDiagnosticFormatterOptions(IDiagnosticChannel defaultChannel) :
            base(defaultChannel)
        {

        }

    }

}
