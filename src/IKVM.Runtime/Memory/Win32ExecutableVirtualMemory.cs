using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace IKVM.Runtime.JNI.Memory
{

    /// <summary>
    /// Maintains a reference to a region of memory allocated by VirtualAlloc.
    /// </summary>
    class Win32ExecutableVirtualMemory : ExecutableMemory
    {

        [Flags]
        public enum AllocationType
        {
            Commit = 0x1000,
            Reserve = 0x2000,
            Decommit = 0x4000,
            Release = 0x8000,
            Reset = 0x80000,
            Physical = 0x400000,
            TopDown = 0x100000,
            WriteWatch = 0x200000,
            LargePages = 0x20000000
        }

        [Flags]
        public enum MemoryProtection
        {
            Execute = 0x10,
            ExecuteRead = 0x20,
            ExecuteReadWrite = 0x40,
            ExecuteWriteCopy = 0x80,
            NoAccess = 0x01,
            ReadOnly = 0x02,
            ReadWrite = 0x04,
            WriteCopy = 0x08,
            GuardModifierflag = 0x100,
            NoCacheModifierflag = 0x200,
            WriteCombineModifierflag = 0x400
        }

        [Flags]
        public enum FreeType
        {
            MEM_DECOMMIT = 0x00004000,
            MEM_RELEASE = 0x00008000,
            MEM_COALESCE_PLACEHOLDERS = 0x00000001,
            MEM_PRESERVE_PLACEHOLDER = 0x00000002
        }

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        static extern IntPtr VirtualAlloc(IntPtr lpAddress, IntPtr dwSize, AllocationType flAllocationType, MemoryProtection flProtect);

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        static extern int VirtualProtect(IntPtr lpAddress, IntPtr dwSize, MemoryProtection flNewProtect, out MemoryProtection lpflOldProtect);

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        static extern int VirtualFree(IntPtr lpAddress, IntPtr dwSize, FreeType freeType);

        /// <summary>
        /// Creates a new region of executable virtual memory.
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public new static Win32ExecutableVirtualMemory Allocate(int size)
        {
            if (size <= 0)
                throw new ArgumentOutOfRangeException(nameof(size));

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) == false)
                throw new PlatformNotSupportedException();

            var handle = VirtualAlloc(IntPtr.Zero, (IntPtr)size, AllocationType.Commit | AllocationType.Reserve, MemoryProtection.ReadWrite);
            if (handle == IntPtr.Zero)
                throw new Win32Exception(Marshal.GetLastWin32Error());

            return new Win32ExecutableVirtualMemory(handle, size);
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="size"></param>
        /// <exception cref="Win32Exception"></exception>
        public Win32ExecutableVirtualMemory(IntPtr handle, int size) :
            base(handle, size)
        {

        }

        /// <summary>
        /// Sets the memory to executable.
        /// </summary>
        /// <exception cref="Win32Exception"></exception>
        public override void SetExecutable()
        {
            var r = VirtualProtect(handle, (IntPtr)Size, MemoryProtection.ExecuteRead, out _);
            if (r == 0)
                throw new Win32Exception(Marshal.GetLastWin32Error());
        }

        /// <summary>
        /// Releases the allocated memory.
        /// </summary>
        /// <returns></returns>
        protected override bool ReleaseHandle()
        {
            return VirtualFree(DangerousGetHandle(), IntPtr.Zero, FreeType.MEM_RELEASE) != 0;
        }

    }

}
