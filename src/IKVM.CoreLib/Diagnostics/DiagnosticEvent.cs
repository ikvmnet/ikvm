using System;

namespace IKVM.CoreLib.Diagnostics
{

    readonly ref partial struct DiagnosticEvent(Diagnostic Diagnostic, ReadOnlySpan<object?> Args, DiagnosticLocation Location = default)
    {

        /// <summary>
        /// Gets the <see cref="Diagnostic"/> this is an event of.
        /// </summary>
        public readonly Diagnostic Diagnostic = Diagnostic;

        /// <summary>
        /// Gets the arguments of the event.
        /// </summary>
        public readonly ReadOnlySpan<object?> Args = Args;

        /// <summary>
        /// Gets the location of the event.
        /// </summary>
        public readonly DiagnosticLocation Location = Location;

        /// <summary>
        /// Formats the diagnostic event message.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
#if NET8_0_OR_GREATER
            return string.Format(null, Diagnostic.Message, Args);
#else
            return string.Format(Diagnostic.Message, Args.ToArray());
#endif
        }

    }

}
