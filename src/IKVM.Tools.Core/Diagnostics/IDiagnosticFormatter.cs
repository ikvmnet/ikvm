using System.Threading;
using System.Threading.Tasks;

using IKVM.CoreLib.Diagnostics;

namespace IKVM.Tools.Core.Diagnostics
{

    interface IDiagnosticFormatter
    {

        /// <summary>
        /// Writes the event.
        /// </summary>
        /// <param name="event"></param>
        void Write(in DiagnosticEvent @event);

    }


}
