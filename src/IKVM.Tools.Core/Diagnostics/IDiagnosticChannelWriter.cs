using System;
using System.Buffers;

namespace IKVM.Tools.Core.Diagnostics
{

    public interface IDiagnosticChannelWriter : IDisposable
    {

        /// <summary>
        /// Enqueues the specified memory to the diagnostic channel.
        /// </summary>
        /// <param name="memory"></param>
        /// <param name="length"></param>
        void Write(IMemoryOwner<byte> memory, int length);

    }

}