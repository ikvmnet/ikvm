using System;
using System.Collections.Generic;

using IKVM.CoreLib.Diagnostics;

namespace IKVM.Tools.Core.Diagnostics
{

    abstract class DiagnosticChannelFormatterOptions : DiagnosticFormatterOptions
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public DiagnosticChannelFormatterOptions()
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="defaultChannel"></param>
        public DiagnosticChannelFormatterOptions(IDiagnosticChannel defaultChannel)
        {
            TraceChannel = defaultChannel;
            InfoChannel = defaultChannel;
            WarningChannel = defaultChannel;
            ErrorChannel = defaultChannel;
            FatalChannel = defaultChannel;
        }

        /// <summary>
        /// Gets the channel into which to direct <see cref="DiagnosticLevel.Trace"/> messages.
        /// </summary>
        public IDiagnosticChannel? TraceChannel { get; set; }

        /// <summary>
        /// Gets the channel into which to direct <see cref="DiagnosticLevel.Info"/> messages.
        /// </summary>
        public IDiagnosticChannel? InfoChannel { get; set; }

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