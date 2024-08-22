using System;

namespace IKVM.CoreLib.Diagnostics
{

    readonly ref struct DiagnosticEvent(Diagnostic Diagnostic, ReadOnlySpan<object?> Args, Exception? Exception)
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
        /// Gets the <see cref="Exception"/> attached to the event.
        /// </summary>
        public readonly Exception? Exception = Exception;

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
