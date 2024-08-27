namespace IKVM.Tools.Core.Diagnostics
{

    class JsonDiagnosticFormatterOptions : FormatterOptionsBase
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="defaultChannel"></param>
        public JsonDiagnosticFormatterOptions() :
            base()
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="defaultChannel"></param>
        public JsonDiagnosticFormatterOptions(IDiagnosticChannel defaultChannel) :
            base(defaultChannel)
        {

        }

    }

}
