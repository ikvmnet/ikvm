using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

using Mono.Unix;
using Mono.Unix.Native;

namespace IKVM.Runtime.JNI.Memory
{

    /// <summary>
    /// Maintains a reference to a region of memory allocated by VirtualAlloc.
    /// </summary>
    class PosixExecutableVirtualMemory : ExecutableMemory
    {

        /// <summary>
        /// Creates a new region of executable virtual memory.
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public new static PosixExecutableVirtualMemory Allocate(int size)
        {
            if (size <= 0)
                throw new ArgumentOutOfRangeException(nameof(size));

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) == false &&
                RuntimeInformation.IsOSPlatform(OSPlatform.OSX) == false)
                throw new PlatformNotSupportedException();

            var handle = Syscall.mmap(IntPtr.Zero, (ulong)size, MmapProts.PROT_READ | MmapProts.PROT_WRITE | MmapProts.PROT_EXEC, MmapFlags.MAP_PRIVATE | MmapFlags.MAP_ANON, -1, 0);
            if (handle == (IntPtr)(-1))
                UnixMarshal.ThrowExceptionForError(Stdlib.GetLastError());

            return new PosixExecutableVirtualMemory(handle, size);
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="size"></param>
        /// <exception cref="Win32Exception"></exception>
        public PosixExecutableVirtualMemory(IntPtr handle, int size) :
            base(handle, size)
        {

        }

        /// <summary>
        /// Releases the allocated memory.
        /// </summary>
        /// <returns></returns>
        protected override bool ReleaseHandle()
        {
            Syscall.munmap(handle, (ulong)Size);
            return true;
        }

    }

}
