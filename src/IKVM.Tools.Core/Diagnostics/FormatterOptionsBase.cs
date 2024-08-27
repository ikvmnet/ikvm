using IKVM.CoreLib.Diagnostics;

namespace IKVM.Tools.Core.Diagnostics
{

    abstract class FormatterOptionsBase
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public FormatterOptionsBase()
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="defaultChannel"></param>
        public FormatterOptionsBase(IDiagnosticChannel defaultChannel)
        {
            TraceChannel = defaultChannel;
            InformationChannel = defaultChannel;
            WarningChannel = defaultChannel;
            ErrorChannel = defaultChannel;
            FatalChannel = defaultChannel;
        }

        /// <summary>
        /// Gets the channel into which to direct <see cref="DiagnosticLevel.Trace"/> messages.
        /// </summary>
         public IDiagnosticChannel? TraceChannel { get; set; }

        /// <summary>
        /// Gets the channel into which to direct <see cref="DiagnosticLevel.Informational"/> messages.
        /// </summary>
         public IDiagnosticChannel? InformationChannel { get; set; }

        /// <summary>
        /// Gets the channel into which to direct <see cref="DiagnosticLevel.Warning"/> messages.
        /// </summary>
         public IDiagnosticChannel? WarningChannel { get; set; }

        /// <summary>
        /// Gets the channel into which to direct <see cref="DiagnosticLevel.Error"/> messages.
        /// </summary>
         public IDiagnosticChannel? ErrorChannel { get; set; }

        /// <summary>
        /// Gets the channel into which to direct <see cref="DiagnosticLevel.Fatal"/> messages.
        /// </summary>
         public IDiagnosticChannel? FatalChannel { get; set; }

    }

}