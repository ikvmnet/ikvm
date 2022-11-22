using System;
using System.IO;
using System.Runtime.InteropServices;

using Mono.Unix.Native;

namespace IKVM.Java.Externs.sun.nio.ch
{

    /// <summary>
    /// Implements the external methods for <see cref="global::java.net.PlainSocketImpl"/>.
    /// </summary>
    static class FileChannelImpl
    {

        /// <summary>
        /// Implements the native method for 'ikvm_mmap'.
        /// </summary>
        public static IntPtr ikvm_mmap(FileStream stream, bool writeable, bool copy_on_write, long position, int size)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) == false)
                throw new global::java.lang.UnsupportedOperationException();

            return Syscall.mmap(IntPtr.Zero, (ulong)size, writeable ? MmapProts.PROT_WRITE | MmapProts.PROT_READ : MmapProts.PROT_READ, copy_on_write ? MmapFlags.MAP_PRIVATE : MmapFlags.MAP_SHARED, (int)stream.SafeFileHandle.DangerousGetHandle(), position);
#endif
        }

    }

}
