using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ikvm.debugger
{
    class DebuggerUtils
    {
        /// <summary>
        /// Read the buffer full. 
        /// </summary>
        /// <param name="stream">the stream to read</param>
        /// <param name="buffer">the buffer that should be fill</param>
        /// <exception cref="IOException">
        /// If the stream ends.
        /// </exception>
        internal static void ReadFully(Stream stream, byte[] buffer)
        {
            int offset = 0;
            int needed = buffer.Length;
            while (needed > 0)
            {
                int count = stream.Read(buffer, offset, needed - offset);
                if (count == 0)
                {
                    throw new IOException("protocol error - premature EOF");
                }
                needed -= count;
                offset += count;
            }
        }

    }
}
