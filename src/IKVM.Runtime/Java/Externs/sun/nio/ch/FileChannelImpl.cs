using System;
using System.IO;
using System.Runtime.InteropServices;

using IKVM.Internal;
using IKVM.Runtime;
using IKVM.Runtime.Accessors.Sun.Nio.Ch;

using Microsoft.Win32.SafeHandles;

using Mono.Unix.Native;

namespace IKVM.Java.Externs.sun.nio.ch
{

    /// <summary>
    /// Implements the external methods for <see cref="global::java.net.PlainSocketImpl"/>.
    /// </summary>
    static class FileChannelImpl
    {

        const int MAP_RO = 0;
        const int MAP_RW = 1;
        const int MAP_PV = 2;

#if FIRST_PASS == false

        static FileChannelImplAccessor fileChannelImplAccessor;

        static FileChannelImplAccessor FileChannelImplAccessor => JVM.BaseAccessors.Get<FileChannelImplAccessor>(ref fileChannelImplAccessor);

#endif

        [DllImport("kernel32", SetLastError = true)]
        static extern SafeFileHandle CreateFileMapping(SafeFileHandle hFile, IntPtr lpAttributes, int flProtect, int dwMaximumSizeHigh, int dwMaximumSizeLow, string lpName);

        [DllImport("kernel32", SetLastError = true)]
        static extern IntPtr MapViewOfFile(SafeFileHandle hFileMapping, int dwDesiredAccess, int dwFileOffsetHigh, int dwFileOffsetLow, IntPtr dwNumberOfBytesToMap);

        [DllImport("kernel32", SetLastError = true)]
        static extern int UnmapViewOfFile(IntPtr lpBaseAddress);

        /// <summary>
        /// Implements the native method for 'map0'.
        /// </summary>
        public static IntPtr map0(object self, int prot, long position, long length)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (RuntimeUtil.IsWindows)
                return MapWindows((global::sun.nio.ch.FileChannelImpl)self, prot, position, length);
            else if (RuntimeUtil.IsLinux)
                return MapLinux((global::sun.nio.ch.FileChannelImpl)self, prot, position, length);
            else if (RuntimeUtil.IsOSX)
                return MapViewOfFileOSX((global::sun.nio.ch.FileChannelImpl)self, prot, position, length);
            else
                throw new global::java.io.IOException("Unsupported operation on platform.");
#endif
        }

        /// <summary>
        /// Implements the native method for 'unmap0'.
        /// </summary>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static int unmap0(long address, long length)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (RuntimeUtil.IsWindows)
                return UnmapWindows((IntPtr)address, length);
            else if (RuntimeUtil.IsLinux)
                return UnmapLinux((IntPtr)address, length);
            else if (RuntimeUtil.IsOSX)
                return UnmapOSX((IntPtr)address, length);
            else
                throw new global::java.io.IOException("Unsupported operation on platform.");
#endif
        }

        /// <summary>
        /// Implements the native method for 'unmap0'.
        /// </summary>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static int transferTo0(global::java.io.FileDescriptor src, long position, long count, global::java.io.FileDescriptor dst)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            throw new NotImplementedException();
#endif
        }

        /// <summary>
        /// Implements the native method for 'unmap0'.
        /// </summary>
        /// <param name="src"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static int position0(global::java.io.FileDescriptor fd, long offset)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            throw new NotImplementedException();
#endif
        }

#if FIRST_PASS == false

        static IntPtr MapWindows(global::sun.nio.ch.FileChannelImpl self, int prot, long position, long length)
        {
            var fs = (FileStream)((global::java.io.FileDescriptor)FileChannelImplAccessor.GetFd(self)).getStream();

            try
            {
                const int PAGE_READONLY = 2;
                const int PAGE_READWRITE = 4;
                const int PAGE_WRITECOPY = 8;

                const int FILE_MAP_WRITE = 2;
                const int FILE_MAP_READ = 4;
                const int FILE_MAP_COPY = 1;

                int fileProtect;
                int mapAccess;

                switch (prot)
                {
                    case MAP_RO:
                        fileProtect = PAGE_READONLY;
                        mapAccess = FILE_MAP_READ;
                        break;
                    case MAP_RW:
                        fileProtect = PAGE_READWRITE;
                        mapAccess = FILE_MAP_WRITE;
                        break;
                    case MAP_PV:
                        fileProtect = PAGE_WRITECOPY;
                        mapAccess = FILE_MAP_COPY;
                        break;
                    default:
                        throw new global::java.lang.Error();
                }

                var maxSize = length + position;
                var hFileMapping = CreateFileMapping(fs.SafeFileHandle, IntPtr.Zero, fileProtect, (int)(maxSize >> 32), (int)maxSize, null);
                var err = Marshal.GetLastWin32Error();
                if (hFileMapping.IsInvalid)
                    throw new global::java.io.IOException("Win32 error " + err);

                var h = MapViewOfFile(hFileMapping, mapAccess, (int)(position >> 32), (int)position, (IntPtr)(length));
                err = Marshal.GetLastWin32Error();
                hFileMapping.Close();

                if (h == IntPtr.Zero)
                {
                    if (err == 8 /*ERROR_NOT_ENOUGH_MEMORY*/)
                        throw new global::java.lang.OutOfMemoryError("file mapping failed");

                    throw new global::java.io.IOException("Win32 error " + err);
                }

                GC.AddMemoryPressure(length);
                return h;
            }
            finally
            {
                GC.KeepAlive(fs);
            }
        }

        static IntPtr MapLinux(global::sun.nio.ch.FileChannelImpl self, int prot, long position, long length)
        {
            var fs = (FileStream)((global::java.io.FileDescriptor)FileChannelImplAccessor.GetFd(self)).getStream();

            try
            {
                var p = MmapProts.PROT_NONE;
                if ((prot & MAP_RO) != 0)
                    p |= MmapProts.PROT_READ;
                if ((prot & MAP_RW) != 0)
                    p |= MmapProts.PROT_READ | MmapProts.PROT_WRITE;

                var f = MmapFlags.MAP_SHARED;
                if ((prot & MAP_PV) != 0)
                    f |= MmapFlags.MAP_PRIVATE;

                var i = Syscall.mmap(IntPtr.Zero, (ulong)length, p, f, (int)fs.SafeFileHandle.DangerousGetHandle(), position);
                if (i == IntPtr.Zero)
                    throw new global::java.io.IOException("file mapping failed");

                GC.AddMemoryPressure(length);
                return i;
            }
            finally
            {
                GC.KeepAlive(fs);
            }
        }

        static IntPtr MapViewOfFileOSX(global::sun.nio.ch.FileChannelImpl self, int prot, long position, long length)
        {
            throw new global::java.io.IOException("Unsupported operation on platform.");
        }

        static int UnmapWindows(IntPtr address, long length)
        {
            UnmapViewOfFile(address);
            GC.RemoveMemoryPressure(length);
            return 0;
        }

        static int UnmapLinux(IntPtr address, long length)
        {
            Syscall.munmap(address, (ulong)length);
            GC.RemoveMemoryPressure(length);
            return 0;
        }

        static int UnmapOSX(IntPtr address, long length)
        {
            throw new global::java.io.IOException("Unsupported operation on platform.");
        }

#endif

    }

}