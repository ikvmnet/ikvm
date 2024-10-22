using System;
using System.Buffers;

namespace IKVM.Tools.Core.Diagnostics
{

    public static class DiagnosticChannelWriterExtensions
    {

        /// <summary>
        /// Writes the given buffer.
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="buffer"></param>
        public static void Write(this IDiagnosticChannelWriter writer, ReadOnlySpan<byte> buffer)
        {
            var mem = MemoryPool<byte>.Shared.Rent(buffer.Length);
            buffer.CopyTo(mem.Memory.Span);
            writer.Write(mem, buffer.Length);
        }

    }

}
