using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

using IKVM.Runtime;
using IKVM.Runtime.Accessors.Java.Io;
using IKVM.Runtime.Accessors.Sun.Nio.Ch;

using Microsoft.Win32.SafeHandles;

using Mono.Unix.Native;

using sun.nio.ch;

namespace IKVM.Java.Externs.sun.nio.ch
{

    /// <summary>
    /// Implements the external methods for <see cref="global::sun.nio.ch.FileChannelImpl"/>.
    /// </summary>
    static class FileChannelImpl
    {

        const int MAP_RO = 0;
        const int MAP_RW = 1;
        const int MAP_PV = 2;

#if FIRST_PASS == false

        static FileDescriptorAccessor fileDescriptorAccessor;
        static FileChannelImplAccessor fileChannelImplAccessor;

        static FileDescriptorAccessor FileDescriptorAccessor => JVM.BaseAccessors.Get(ref fileDescriptorAccessor);

        static FileChannelImplAccessor FileChannelImplAccessor => JVM.BaseAccessors.Get(ref fileChannelImplAccessor);

#endif

        /// <summary>
        /// Implements the native method 'initIDs'.
        /// </summary>
        public static long initIDs()
        {
            return Environment.SystemPageSize;
        }

        /// <summary>
        /// Implements the native method for 'map0'.
        /// </summary>
        public static long map0(object self, int prot, long position, long length)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (RuntimeUtil.IsWindows)
                return (long)MapFileWindows((global::sun.nio.ch.FileChannelImpl)self, prot, position, length);
            else if (RuntimeUtil.IsLinux)
                return (long)MapFileLinux((global::sun.nio.ch.FileChannelImpl)self, prot, position, length);
            else if (RuntimeUtil.IsOSX)
                return (long)MapFileOSX((global::sun.nio.ch.FileChannelImpl)self, prot, position, length);
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
        /// Invokes the TransmitFile Win32 function.
        /// </summary>
        /// <param name="hSocket"></param>
        /// <param name="hFile"></param>
        /// <param name="nNumberOfBytesToWrite"></param>
        /// <param name="nNumberOfBytesPerSend"></param>
        /// <param name="lpOverlapped"></param>
        /// <param name="lpTransmitBuffers"></param>
        /// <param name="dwReserved"></param>
        /// <returns></returns>
        [DllImport("kernel32", SetLastError = true)]
        static unsafe extern int TransmitFile(IntPtr hSocket, IntPtr hFile, int nNumberOfBytesToWrite, int nNumberOfBytesPerSend, NativeOverlapped* lpOverlapped, void* lpTransmitBuffers, int dwReserved);

        /// <summary>
        /// Implements the native method for 'transferTo0'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="src"></param>
        /// <param name="position"></param>
        /// <param name="count"></param>
        /// <param name="dst"></param>
        /// <returns></returns>
        public static unsafe long transferTo0(object self, global::java.io.FileDescriptor src, long position, long count, global::java.io.FileDescriptor dst)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            var stream = FileDescriptorAccessor.GetStream(src);
            if (stream == null)
                throw new global::java.io.IOException("Stream closed.");
            if (stream is not FileStream fs)
                throw new global::java.io.IOException("Transfor not supported.");

            var socket = FileDescriptorAccessor.GetSocket(dst);
            if (socket == null)
                throw new global::java.io.IOException("Socket closed.");

            try
            {
                if (RuntimeUtil.IsWindows)
                {
                    const int WSAEINVAL = 10022;
                    const int WSAENOTSOCK = 10038;
                    const int TF_USE_KERNEL_APC = 32;
                    const int PACKET_SIZE = 524288;

                    // win32 expets 
                    var chunkSize = (int)Math.Min(count, int.MaxValue);

                    // move file to specified position
                    if (stream.Position != position)
                    {
                        if (stream.CanSeek == false)
                            return global::sun.nio.ch.IOStatus.UNSUPPORTED;

                        stream.Seek(position, SeekOrigin.Begin);
                    }

                    int result = TransmitFile(socket.Handle, fs.SafeFileHandle.DangerousGetHandle(), chunkSize, PACKET_SIZE, null, null, TF_USE_KERNEL_APC);
                    if (result == 0)
                    {
                        return Marshal.GetLastWin32Error() switch
                        {
                            WSAEINVAL when count >= 0 => global::sun.nio.ch.IOStatus.UNSUPPORTED_CASE,
                            WSAENOTSOCK => global::sun.nio.ch.IOStatus.UNSUPPORTED_CASE,
                            _ => throw new global::java.io.IOException("Transfer failed.")
                        };
                    }

                    return chunkSize;
                }
                else
                {
                    var result = Syscall.sendfile((int)socket.Handle, (int)fs.SafeFileHandle.DangerousGetHandle(), ref position, (ulong)count);
                    if (result < 0)
                    {
                        return Stdlib.GetLastError() switch
                        {
                            Errno.EAGAIN => global::sun.nio.ch.IOStatus.UNAVAILABLE,
                            Errno.EINVAL when count >= 0 => global::sun.nio.ch.IOStatus.UNSUPPORTED_CASE,
                            Errno.EINTR => (long)global::sun.nio.ch.IOStatus.INTERRUPTED,
                            _ => throw new global::java.io.IOException("Transfer failed."),
                        };
                    }

                    return result;
                }
            }
            catch (global::java.io.IOException)
            {
                throw;
            }
            catch (EndOfStreamException)
            {
                return global::sun.nio.ch.IOStatus.EOF;
            }
            catch (NotSupportedException)
            {
                return global::sun.nio.ch.IOStatus.UNSUPPORTED;
            }
            catch (ObjectDisposedException)
            {
                return global::sun.nio.ch.IOStatus.UNAVAILABLE;
            }
            catch (Exception e)
            {
                throw new global::java.io.IOException("Transfer failed.", e);
            }
#endif
        }

        /// <summary>
        /// Implements the native method for 'position0'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="fd"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static long position0(object self, global::java.io.FileDescriptor fd, long offset)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            var s = FileDescriptorAccessor.GetStream(fd);
            if (s == null)
                throw new global::java.io.IOException("Stream closed.");

            try
            {
                if (offset >= 0)
                {
                    if (s.CanSeek == false)
                        return global::sun.nio.ch.IOStatus.UNSUPPORTED;
                    else
                        return s.Seek(offset, SeekOrigin.Begin);
                }

                return s.Position;
            }
            catch (global::java.io.IOException)
            {
                throw;
            }
            catch (EndOfStreamException)
            {
                return global::sun.nio.ch.IOStatus.EOF;
            }
            catch (NotSupportedException)
            {
                return global::sun.nio.ch.IOStatus.UNSUPPORTED;
            }
            catch (ObjectDisposedException)
            {
                return global::sun.nio.ch.IOStatus.UNAVAILABLE;
            }
            catch (Exception e)
            {
                throw new global::java.io.IOException("Position failed.", e);
            }
#endif
        }

#if FIRST_PASS == false

        /// <summary>
        /// Invokes the CreateFileMapping Win32 function.
        /// </summary>
        /// <param name="hFile"></param>
        /// <param name="lpAttributes"></param>
        /// <param name="flProtect"></param>
        /// <param name="dwMaximumSizeHigh"></param>
        /// <param name="dwMaximumSizeLow"></param>
        /// <param name="lpName"></param>
        /// <returns></returns>
        [DllImport("kernel32", SetLastError = true)]
        static extern SafeFileHandle CreateFileMapping(SafeFileHandle hFile, IntPtr lpAttributes, int flProtect, int dwMaximumSizeHigh, int dwMaximumSizeLow, string lpName);

        /// <summary>
        /// Invokes the MapViewOfFile Win32 function.
        /// </summary>
        /// <param name="hFileMapping"></param>
        /// <param name="dwDesiredAccess"></param>
        /// <param name="dwFileOffsetHigh"></param>
        /// <param name="dwFileOffsetLow"></param>
        /// <param name="dwNumberOfBytesToMap"></param>
        /// <returns></returns>
        [DllImport("kernel32", SetLastError = true)]
        static extern IntPtr MapViewOfFile(SafeFileHandle hFileMapping, int dwDesiredAccess, int dwFileOffsetHigh, int dwFileOffsetLow, IntPtr dwNumberOfBytesToMap);

        /// <summary>
        /// Invokes the UnmapViewOfFile Win32 function.
        /// </summary>
        /// <param name="lpBaseAddress"></param>
        /// <returns></returns>
        [DllImport("kernel32", SetLastError = true)]
        static extern int UnmapViewOfFile(IntPtr lpBaseAddress);

        /// <summary>
        /// Implements memory mapped files on Windows.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="prot"></param>
        /// <param name="position"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        /// <exception cref="global::java.lang.Error"></exception>
        /// <exception cref="global::java.io.IOException"></exception>
        /// <exception cref="global::java.lang.OutOfMemoryError"></exception>
        static IntPtr MapFileWindows(global::sun.nio.ch.FileChannelImpl self, int prot, long position, long length)
        {
            var s = FileDescriptorAccessor.GetStream(FileChannelImplAccessor.GetFd(self));
            if (s == null)
                throw new global::java.io.IOException("Stream closed.");
            if (s is not FileStream fs)
                throw new global::java.io.IOException("Map not supported.");

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

        /// <summary>
        /// Implements the unmapping of a file on Windows.
        /// </summary>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        static int UnmapWindows(IntPtr address, long length)
        {
            UnmapViewOfFile(address);
            GC.RemoveMemoryPressure(length);
            return 0;
        }

        /// <summary>
        /// Implements memory mapped files on Linux.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="prot"></param>
        /// <param name="position"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        /// <exception cref="global::java.io.IOException"></exception>
        static IntPtr MapFileLinux(global::sun.nio.ch.FileChannelImpl self, int prot, long position, long length)
        {
            var s = FileDescriptorAccessor.GetStream(FileChannelImplAccessor.GetFd(self));
            if (s == null)
                throw new global::java.io.IOException("Stream closed.");
            if (s is not FileStream fs)
                throw new global::java.io.IOException("Map not supported.");

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

        /// <summary>
        /// Implements the unmapping of a mapped file on Linux.
        /// </summary>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        static int UnmapLinux(IntPtr address, long length)
        {
            Syscall.munmap(address, (ulong)length);
            GC.RemoveMemoryPressure(length);
            return 0;
        }

        /// <summary>
        /// Implements memory mapped files on OS X.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="prot"></param>
        /// <param name="position"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        /// <exception cref="global::java.io.IOException"></exception>
        static IntPtr MapFileOSX(global::sun.nio.ch.FileChannelImpl self, int prot, long position, long length)
        {
            throw new global::java.io.IOException("Unsupported operation on platform.");
        }

        /// <summary>
        /// Implements the umapping of a mapped file on OS X.
        /// </summary>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        /// <exception cref="global::java.io.IOException"></exception>
        static int UnmapOSX(IntPtr address, long length)
        {
            throw new global::java.io.IOException("Unsupported operation on platform.");
        }

#endif

    }

}