using System;

using IKVM.CoreLib.Diagnostics;

namespace IKVM.Tools.Importer
{

    sealed class FatalCompilerErrorException : Exception
    {

        /// <summary>
        /// Returns the output text for the given <see cref="DiagnosticLevel"/>.
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        static string FormatDiagnosticLevel(DiagnosticLevel level)
        {
            return level switch
            {
                DiagnosticLevel.Trace => "trace",
                DiagnosticLevel.Info => "info",
                DiagnosticLevel.Warning => "warning",
                DiagnosticLevel.Error => "error",
                DiagnosticLevel.Fatal => "fatal",
                _ => throw new InvalidOperationException(),
            };
        }

        readonly DiagnosticEvent _event;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="evt"></param>
        internal FatalCompilerErrorException(in DiagnosticEvent evt) :
#if NET8_0_OR_GREATER
            base($"{FormatDiagnosticLevel(evt.Diagnostic.Level)} IKVM{evt.Diagnostic.Id:D4}: {string.Format(null, evt.Diagnostic.Message, evt.Args)}")
#else
            base($"{FormatDiagnosticLevel(evt.Diagnostic.Level)} IKVM{evt.Diagnostic.Id:D4}: {string.Format(null, evt.Diagnostic.Message, evt.Args)}")
#endif
        {
            _event = evt;
        }

        /// <summary>
        /// Gets the event that triggered this exception.
        /// </summary>
        public DiagnosticEvent Event => _event;

    }

}
