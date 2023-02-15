using System;
using System.IO;
using System.Runtime.InteropServices;

using IKVM.Runtime;

using java.io;

using Microsoft.Win32.SafeHandles;

using sun.nio.ch;

namespace IKVM.Java.Externs.sun.nio.ch
{

    /// <summary>
    /// Implements the native methods for <see cref="global::sun.nio.ch.FileDispatcherImpl"/>.
    /// </summary>
    static class FileDispatcherImpl
    {

        /// <summary>
        /// Implements the native method 'read0'.
        /// </summary>
        /// <param name="fd"></param>
        /// <param name="address"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        /// <exception cref="global::java.io.IOException"></exception>
        public static unsafe int read0(FileDescriptor fd, long address, int len)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            var s = (FileStream)fd.getStream();
            if (s == null)
                throw new global::java.io.IOException("Stream closed.");

            if (s.CanRead == false)
                return IOStatus.UNSUPPORTED;

            try
            {
#if NETFRAMEWORK
                var b = new byte[len];
                var r = ((FileStream)fd.getStream()).Read(b, 0, len);
                b.CopyTo(new Span<byte>((void*)(IntPtr)address, len));
#else
                var r = ((FileStream)fd.getStream()).Read(new Span<byte>((void*)(IntPtr)address, len));
#endif

                return r == 0 ? IOStatus.EOF : r;
            }
            catch (global::java.io.IOException)
            {
                throw;
            }
            catch (EndOfStreamException)
            {
                return IOStatus.EOF;
            }
            catch (NotSupportedException)
            {
                return IOStatus.UNSUPPORTED;
            }
            catch (ObjectDisposedException)
            {
                return IOStatus.UNAVAILABLE;
            }
            catch (Exception e)
            {
                throw new global::java.io.IOException("Read failed.", e);
            }
#endif
        }

        /// <summary>
        /// Implements the native method 'pread0'.
        /// </summary>
        /// <param name="fd"></param>
        /// <param name="address"></param>
        /// <param name="len"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        /// <exception cref="global::java.io.IOException"></exception>
        public static unsafe int pread0(FileDescriptor fd, long address, int len, long position)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            var s = (FileStream)fd.getStream();
            if (s == null)
                throw new global::java.io.IOException("Stream closed.");

            if (s.CanRead == false)
                return IOStatus.UNSUPPORTED;

            try
            {
                if (s.Position != position)
                {
                    if (s.CanSeek == false)
                        return IOStatus.UNSUPPORTED;

                    s.Seek(position, SeekOrigin.Begin);
                }
            }
            catch (global::java.io.IOException)
            {
                throw;
            }
            catch (EndOfStreamException)
            {
                return IOStatus.EOF;
            }
            catch (NotSupportedException)
            {
                return IOStatus.UNSUPPORTED;
            }
            catch (ObjectDisposedException)
            {
                return IOStatus.UNAVAILABLE;
            }
            catch (Exception e)
            {
                throw new global::java.io.IOException("Seek failed.", e);
            }

            try
            {
#if NETFRAMEWORK
                var b = new byte[len];
                var r = s.Read(b, 0, len);
                b.CopyTo(new Span<byte>((void*)(IntPtr)address, len));
#else
                var r = s.Read(new Span<byte>((void*)(IntPtr)address, len));
#endif

                return r == 0 ? IOStatus.EOF : r;
            }
            catch (global::java.io.IOException)
            {
                throw;
            }
            catch (EndOfStreamException)
            {
                return IOStatus.EOF;
            }
            catch (NotSupportedException)
            {
                return IOStatus.UNSUPPORTED;
            }
            catch (ObjectDisposedException)
            {
                return IOStatus.UNAVAILABLE;
            }
            catch (Exception e)
            {
                throw new global::java.io.IOException("Read failed.", e);
            }
#endif
        }

        /// <summary>
        /// Implements the native method 'readv0'.
        /// </summary>
        /// <param name="fd"></param>
        /// <param name="address"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static long readv0(FileDescriptor fd, long address, int len)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            var s = (FileStream)fd.getStream();
            if (s == null)
                throw new global::java.io.IOException("Stream closed.");

            if (s.CanRead == false)
                return IOStatus.UNSUPPORTED;

            throw new NotImplementedException();
#endif
        }

        /// <summary>
        /// Implements the native method 'write0'.
        /// </summary>
        /// <param name="fd"></param>
        /// <param name="address"></param>
        /// <param name="len"></param>
        /// <param name="append"></param>
        /// <returns></returns>
        /// <exception cref="global::java.io.IOException"></exception>
        public static unsafe int write0(FileDescriptor fd, long address, int len, bool append)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            var s = (FileStream)fd.getStream();
            if (s == null)
                throw new global::java.io.IOException("Stream closed.");

            if (s.CanWrite == false)
                return IOStatus.UNSUPPORTED;

            try
            {
                if (append)
                {
                    if (s.CanSeek == false)
                        return IOStatus.UNSUPPORTED;

                    s.Seek(0, SeekOrigin.End);
                }
            }
            catch (global::java.io.IOException)
            {
                throw;
            }
            catch (EndOfStreamException)
            {
                return IOStatus.EOF;
            }
            catch (NotSupportedException)
            {
                return IOStatus.UNSUPPORTED;
            }
            catch (ObjectDisposedException)
            {
                return IOStatus.UNAVAILABLE;
            }
            catch (Exception e)
            {
                throw new global::java.io.IOException("Seek failed.", e);
            }

            try
            {

#if NETFRAMEWORK
                var b = new byte[len];
                new ReadOnlySpan<byte>((void*)(IntPtr)address, len).CopyTo(b);
                s.Write(b, 0, len);
#else
                s.Write(new Span<byte>((void*)(IntPtr)address, len));
#endif

                return len;
            }
            catch (global::java.io.IOException)
            {
                throw;
            }
            catch (EndOfStreamException)
            {
                return IOStatus.EOF;
            }
            catch (NotSupportedException)
            {
                return IOStatus.UNSUPPORTED;
            }
            catch (ObjectDisposedException)
            {
                return IOStatus.UNAVAILABLE;
            }
            catch (Exception e)
            {
                throw new global::java.io.IOException("Write failed.", e);
            }
#endif
        }

        /// <summary>
        /// Implements the native method 'pwrite0'.
        /// </summary>
        /// <param name="fd"></param>
        /// <param name="address"></param>
        /// <param name="len"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        /// <exception cref="global::java.io.IOException"></exception>
        public static unsafe int pwrite0(FileDescriptor fd, long address, int len, long position)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            var s = (FileStream)fd.getStream();
            if (s == null)
                throw new global::java.io.IOException("Stream closed.");

            if (s.CanWrite == false)
                return IOStatus.UNSUPPORTED;

            try
            {
                if (s.Position != position)
                {
                    if (s.CanSeek == false)
                        return IOStatus.UNSUPPORTED;

                    s.Seek(position, SeekOrigin.Begin);
                }
            }
            catch (global::java.io.IOException)
            {
                throw;
            }
            catch (EndOfStreamException)
            {
                return IOStatus.EOF;
            }
            catch (NotSupportedException)
            {
                return IOStatus.UNSUPPORTED;
            }
            catch (ObjectDisposedException)
            {
                return IOStatus.UNAVAILABLE;
            }
            catch (Exception e)
            {
                throw new global::java.io.IOException("Seek failed.", e);
            }

            try
            {

#if NETFRAMEWORK
                var b = new byte[len];
                new ReadOnlySpan<byte>((void*)(IntPtr)address, len).CopyTo(b);
                s.Write(b, 0, len);
#else
                s.Write(new Span<byte>((void*)(IntPtr)address, len));
#endif

                return len;
            }
            catch (global::java.io.IOException)
            {
                throw;
            }
            catch (EndOfStreamException)
            {
                return IOStatus.EOF;
            }
            catch (NotSupportedException)
            {
                return IOStatus.UNSUPPORTED;
            }
            catch (ObjectDisposedException)
            {
                return IOStatus.UNAVAILABLE;
            }
            catch (Exception e)
            {
                throw new global::java.io.IOException("Write failed.", e);
            }
#endif
        }

        /// <summary>
        /// Implements the native method 'writev0'.
        /// </summary>
        /// <param name="fd"></param>
        /// <param name="address"></param>
        /// <param name="len"></param>
        /// <param name="append"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static long writev0(FileDescriptor fd, long address, int len, bool append)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            var s = (FileStream)fd.getStream();
            if (s == null)
                throw new global::java.io.IOException("Stream closed.");

            if (s.CanWrite == false)
                return IOStatus.UNSUPPORTED;

            throw new NotImplementedException();
#endif
        }

        /// <summary>
        /// Implements the native method 'force0'.
        /// </summary>
        /// <param name="fd"></param>
        /// <param name="metaData"></param>
        /// <returns></returns>
        public static int force0(FileDescriptor fd, bool metaData)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            fd.sync();
            return 0;
#endif
        }

        /// <summary>
        /// Implements the native method 'truncate0'.
        /// </summary>
        /// <param name="fd"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static int truncate0(FileDescriptor fd, long size)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            var s = (FileStream)fd.getStream();
            if (s == null)
                throw new global::java.io.IOException("Stream closed.");

            if (s.Length < size)
                return 0;

            try
            {
                s.SetLength(size);
                return 0;
            }
            catch (global::java.io.IOException)
            {
                throw;
            }
            catch (EndOfStreamException)
            {
                return IOStatus.EOF;
            }
            catch (NotSupportedException)
            {
                return IOStatus.UNSUPPORTED;
            }
            catch (ObjectDisposedException)
            {
                return IOStatus.UNAVAILABLE;
            }
            catch (Exception e)
            {
                throw new global::java.io.IOException("Truncate failed.", e);
            }
#endif
        }

        /// <summary>
        /// Implements the native method 'size0'.
        /// </summary>
        /// <param name="fd"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static long size0(FileDescriptor fd)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            var s = (FileStream)fd.getStream();
            if (s == null)
                throw new global::java.io.IOException("Stream closed.");

            try
            {
                return s.Length;
            }
            catch (global::java.io.IOException)
            {
                throw;
            }
            catch (EndOfStreamException)
            {
                return IOStatus.EOF;
            }
            catch (NotSupportedException)
            {
                return IOStatus.UNSUPPORTED;
            }
            catch (ObjectDisposedException)
            {
                return IOStatus.UNAVAILABLE;
            }
            catch (Exception e)
            {
                throw new global::java.io.IOException("Truncate failed.", e);
            }
#endif
        }

        [StructLayout(LayoutKind.Sequential)]
        struct OVERLAPPED
        {
            public IntPtr Internal;
            public IntPtr InternalHigh;
            public int OffsetLow;
            public int OffsetHigh;
            public IntPtr hEvent;
        }

        /// <summary>
        /// Invokes the LOCKFILEEX Win32 function.
        /// </summary>
        /// <param name="hFile"></param>
        /// <param name="dwFlags"></param>
        /// <param name="dwReserved"></param>
        /// <param name="nNumberOfBytesToLockLow"></param>
        /// <param name="nNumberOfBytesToLockHigh"></param>
        /// <param name="lpOverlapped"></param>
        /// <returns></returns>
        [DllImport("kernel32", SetLastError = true)]
        static extern int LockFileEx(SafeFileHandle hFile, int dwFlags, int dwReserved, int nNumberOfBytesToLockLow, int nNumberOfBytesToLockHigh, OVERLAPPED lpOverlapped);

        /// <summary>
        /// Invokes the UNLOCKFILEEX Win32 function.
        /// </summary>
        /// <param name="hFile"></param>
        /// <param name="dwReserved"></param>
        /// <param name="nNumberOfBytesToUnlockLow"></param>
        /// <param name="nNumberOfBytesToUnlockHigh"></param>
        /// <param name="lpOverlapped"></param>
        /// <returns></returns>
        [DllImport("kernel32", SetLastError = true)]
        static extern int UnlockFileEx(SafeFileHandle hFile, int dwReserved, int nNumberOfBytesToUnlockLow, int nNumberOfBytesToUnlockHigh, OVERLAPPED lpOverlapped);

        /// <summary>
        /// Implements the native method 'lock0'.
        /// </summary>
        /// <param name="fd"></param>
        /// <param name="blocking"></param>
        /// <param name="pos"></param>
        /// <param name="size"></param>
        /// <param name="shared"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static int lock0(FileDescriptor fd, bool blocking, long pos, long size, bool shared)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            var s = (FileStream)fd.getStream();
            if (s == null)
                throw new global::java.io.IOException("Stream closed.");

            try
            {
                if (RuntimeUtil.IsWindows)
                {
                    const int LOCKFILE_FAIL_IMMEDIATELY = 1;
                    const int LOCKFILE_EXCLUSIVE_LOCK = 2;
                    const int ERROR_LOCK_VIOLATION = 33;

                    var o = new OVERLAPPED();
                    o.OffsetLow = (int)pos;
                    o.OffsetHigh = (int)(pos >> 32);

                    int flags = 0;
                    if (blocking == false)
                        flags |= LOCKFILE_FAIL_IMMEDIATELY;
                    if (shared == false)
                        flags |= LOCKFILE_EXCLUSIVE_LOCK;

                    int result = LockFileEx(s.SafeFileHandle, flags, 0, (int)size, (int)(size >> 32), o);
                    if (result == 0)
                    {
                        var error = Marshal.GetLastWin32Error();
                        if (!blocking && error == ERROR_LOCK_VIOLATION)
                            return FileDispatcher.NO_LOCK;

                        throw new global::java.io.IOException("Lock failed.");
                    }

                    return FileDispatcher.LOCKED;
                }
                else
                {
                    // fallback to .NET version for non-Windows
                    s.Lock(pos, size);
                    return shared ? FileDispatcher.RET_EX_LOCK : FileDispatcher.LOCKED;
                }
            }
            catch (global::java.io.IOException)
            {
                throw;
            }
            catch (EndOfStreamException)
            {
                return IOStatus.EOF;
            }
            catch (NotSupportedException)
            {
                return IOStatus.UNSUPPORTED;
            }
            catch (ObjectDisposedException)
            {
                return IOStatus.UNAVAILABLE;
            }
            catch (Exception e)
            {
                throw new global::java.io.IOException("Lock failed.", e);
            }
#endif
        }

        /// <summary>
        /// Implements the native method 'release0'.
        /// </summary>
        /// <param name="fd"></param>
        /// <param name="pos"></param>
        /// <param name="size"></param>
        /// <exception cref="NotImplementedException"></exception>
        public static void release0(FileDescriptor fd, long pos, long size)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            var s = (FileStream)fd.getStream();
            if (s == null)
                throw new global::java.io.IOException("Stream closed.");

            try
            {
                if (RuntimeUtil.IsWindows)
                {
                    const int ERROR_NOT_LOCKED = 158;

                    var o = new OVERLAPPED();
                    o.OffsetLow = (int)pos;
                    o.OffsetHigh = (int)(pos >> 32);
                    int result = UnlockFileEx(s.SafeFileHandle, 0, (int)size, (int)(size >> 32), o);
                    if (result == 0 && Marshal.GetLastWin32Error() != ERROR_NOT_LOCKED)
                        throw new global::java.io.IOException("Release failed.");
                }
                else
                {
                    s.Unlock(pos, size);
                }
            }
            catch (global::java.io.IOException)
            {
                throw;
            }
            catch (System.IO.IOException e) when (NotLockedHack.IsErrorNotLocked(e) == false)
            {
                throw new global::java.io.IOException("Release failed.", e);
            }
            catch (Exception e)
            {
                throw new global::java.io.IOException("Release failed.", e);
            }
#endif
        }

        /// <summary>
        /// Attempts to lock an unlocked file to capture teh error message, since IOException provides no way to know
        /// whether the reason corresponds to not being locked.
        /// </summary>
        static class NotLockedHack
        {

            static readonly string msg;

            /// <summary>
            /// Initializes the static instance.
            /// </summary>
            static NotLockedHack()
            {
                try
                {
                    var tmp = Path.GetTempFileName();

                    using (var fs = new FileStream(tmp, FileMode.Create))
                    {
                        try
                        {
                            fs.Unlock(0, 1);
                        }
                        catch (System.IO.IOException e)
                        {
                            msg = e.Message;
                        }
                    }

                    System.IO.File.Delete(tmp);
                }
                catch (Exception)
                {

                }
            }

            /// <summary>
            /// Returns <c>true</c> if the exception represents the file not being locked.
            /// </summary>
            /// <param name="e"></param>
            /// <returns></returns>
            public static bool IsErrorNotLocked(System.IO.IOException e)
            {
                return e.Message == msg;
            }

        }

        /// <summary>
        /// Implements the native method 'close0'.
        /// </summary>
        /// <param name="fd"></param>
        /// <exception cref="NotImplementedException"></exception>
        public static void close0(FileDescriptor fd)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            var s = (FileStream)fd.getStream();
            if (s == null)
                throw new global::java.io.IOException("Stream closed.");

            try
            {
                fd.close();
            }
            catch
            {

            }
#endif
        }

        /// <summary>
        /// Implements the native method 'closeByHandle'.
        /// </summary>
        /// <param name="fd"></param>
        /// <exception cref="NotImplementedException"></exception>
        public static void closeByHandle(long fd)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            throw new NotSupportedException();
#endif
        }

        /// <summary>
        /// Implements the native method 'duplicateHandle'.
        /// </summary>
        /// <param name="fd"></param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        public static long duplicateHandle(long fd)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            throw new NotSupportedException();
#endif
        }

    }

}