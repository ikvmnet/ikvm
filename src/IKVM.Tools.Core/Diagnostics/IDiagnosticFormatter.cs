using IKVM.CoreLib.Diagnostics;

namespace IKVM.Tools.Core.Diagnostics
{

    interface IDiagnosticFormatter
    {

        /// <summary>
        /// Returns <c>true</c> if the given event should be written.
        /// </summary>
        /// <param name="event"></param>
        /// <returns></returns>
        bool CanWrite(in DiagnosticEvent @event);

        /// <summary>
        /// Writes the event.
        /// </summary>
        /// <param name="event"></param>
        void Write(in DiagnosticEvent @event);

    }


}
