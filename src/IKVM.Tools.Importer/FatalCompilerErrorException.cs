using System;

using IKVM.CoreLib.Diagnostics;

namespace IKVM.Tools.Importer
{

    sealed class FatalCompilerErrorException : Exception
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="evt"></param>
        internal FatalCompilerErrorException(in DiagnosticEvent evt) :
#if NET8_0_OR_GREATER
            base($"fatal error IKVMC{evt.Diagnostic.Id}: {string.Format(null, evt.Diagnostic.Message, evt.Args)}")
#else
            base($"fatal error IKVMC{evt.Diagnostic.Id}: {string.Format(evt.Diagnostic.Message, evt.Args.ToArray())}")
#endif
        {

        }

    }

}
