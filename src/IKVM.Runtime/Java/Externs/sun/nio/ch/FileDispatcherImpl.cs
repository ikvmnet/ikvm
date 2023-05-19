using System;
using System.Buffers;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

using IKVM.Runtime;
using IKVM.Runtime.Accessors.Java.Io;

using Microsoft.Win32.SafeHandles;

using Mono.Unix;
using Mono.Unix.Native;

using sun.nio.ch;

namespace IKVM.Java.Externs.sun.nio.ch
{

    /// <summary>
    /// Implements the native methods for <see cref="global::sun.nio.ch.FileDispatcherImpl"/>.
    /// </summary>
    static partial class FileDispatcherImpl
    {

#if FIRST_PASS == false

        static FileDescriptorAccessor fileDescriptorAccessor;

        static FileDescriptorAccessor FileDescriptorAccessor => JVM.BaseAccessors.Get(ref fileDescriptorAccessor);

#endif

        [StructLayout(LayoutKind.Sequential)]
        struct iovec
        {

            public IntPtr iov_base;
            public int iov_len;

        }

        /// <summary>
        /// Implements the native method 'read'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="fd"></param>
        /// <param name="address"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        /// <exception cref="global::java.io.IOException"></exception>
        public static unsafe int read(object self, object fd, long address, int len)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (len == 0)
                return 0;

            var stream = FileDescriptorAccessor.GetStream(fd);
            if (stream == null)
                throw new global::java.io.IOException("Stream closed.");

            if (stream.CanRead == false)
                return IOStatus.UNSUPPORTED;

            try
            {
                int r = -1;

#if NETFRAMEWORK
                var buf = ArrayPool<byte>.Shared.Rent(len);

                try
                {
                    r = stream.Read(buf, 0, len);
                    Marshal.Copy(buf, 0, (IntPtr)address, r);
                }
                finally
                {
                    ArrayPool<byte>.Shared.Return(buf);
                }
#else
                r = stream.Read(new Span<byte>((byte*)(IntPtr)address, len));
#endif

                return r == 0 ? IOStatus.EOF : r;
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
            finally
            {
                GC.KeepAlive(self);
            }
#endif
        }

        /// <summary>
        /// Implements the native method 'pread'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="fd"></param>
        /// <param name="address"></param>
        /// <param name="len"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        /// <exception cref="global::java.io.IOException"></exception>
        public static unsafe int pread(object self, object fd, long address, int len, long position)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (len == 0)
                return 0;

            var stream = FileDescriptorAccessor.GetStream(fd);
            if (stream == null)
                throw new global::java.io.IOException("Stream closed.");

            if (stream.CanRead == false)
                return IOStatus.UNSUPPORTED;
            if (stream.CanSeek == false)
                return IOStatus.UNSUPPORTED;

            if (RuntimeUtil.IsLinux && stream is FileStream fs)
            {
                try
                {
                    if (fs.SafeFileHandle.IsInvalid)
                        return IOStatus.UNAVAILABLE;

                    var n = Syscall.pread((int)fs.SafeFileHandle.DangerousGetHandle(), (IntPtr)address, (ulong)len, position);
                    if (n == -1)
                    {
                        // translate return values
                        var errno = Stdlib.GetLastError();
                        if (errno == Errno.EAGAIN)
                            return IOStatus.UNAVAILABLE;
                        if (errno == Errno.EINTR)
                            return IOStatus.INTERRUPTED;

                        throw new global::java.io.IOException("Read failed.", new UnixIOException(errno));
                    }

                    return n > 0 ? (int)n : IOStatus.EOF;
                }
                finally
                {
                    GC.KeepAlive(self);
                }
            }
            else
            {
                var p = stream.Position;

                try
                {
                    stream.Seek(position, SeekOrigin.Begin);
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
                    int length = -1;

#if NETFRAMEWORK
                    var buf = ArrayPool<byte>.Shared.Rent(len);

                    try
                    {
                        length = stream.Read(buf, 0, len);
                        Marshal.Copy(buf, 0, (IntPtr)address, length);
                    }
                    finally
                    {
                        ArrayPool<byte>.Shared.Return(buf);
                    }
#else
                    length = stream.Read(new Span<byte>((void*)(IntPtr)address, len));
#endif

                    stream.Seek(p, SeekOrigin.Begin);
                    return length == 0 ? IOStatus.EOF : length;
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
                finally
                {
                    GC.KeepAlive(self);
                }
            }
#endif
        }

        /// <summary>
        /// Implements the native method 'readv'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="fd"></param>
        /// <param name="address"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static unsafe long readv(object self, object fd, long address, int len)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            var stream = FileDescriptorAccessor.GetStream(fd);
            if (stream == null)
                throw new global::java.io.IOException("Stream closed.");

            if (stream.CanRead == false)
                return IOStatus.UNSUPPORTED;

            try
            {
                // map io vecors to read into
                var vecs = new ReadOnlySpan<iovec>((byte*)(IntPtr)address, len);
                var length = 0;

                // issue independent reads for each vector
                for (int i = 0; i < vecs.Length; i++)
                {
                    int l;

#if NETFRAMEWORK
                    var buf = ArrayPool<byte>.Shared.Rent(vecs[i].iov_len);

                    try
                    {
                        l = stream.Read(buf, 0, vecs[i].iov_len);
                        Marshal.Copy(buf, 0, vecs[i].iov_base, l);
                        length += l;

                    }
                    finally
                    {
                        ArrayPool<byte>.Shared.Return(buf);
                    }
#else
                    l = stream.Read(new Span<byte>((byte*)vecs[i].iov_base, vecs[i].iov_len));
                    length += l;
#endif

                    // we failed to read up to the requested amount, so we must be at the end
                    if (l < vecs[i].iov_len)
                        break;
                }

                // if we read a total of zero bytes, we must be at the end of the file
                return length == 0 ? IOStatus.EOF : length;
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
            finally
            {
                GC.KeepAlive(self);
            }
#endif
        }

        /// <summary>
        /// Implements the native method 'write'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="fd"></param>
        /// <param name="address"></param>
        /// <param name="len"></param>
        /// <param name="append"></param>
        /// <returns></returns>
        /// <exception cref="global::java.io.IOException"></exception>
        public static unsafe int write0(object self, object fd, long address, int len, bool append)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (len == 0)
                return 0;

            var stream = FileDescriptorAccessor.GetStream(fd);
            if (stream == null)
                throw new global::java.io.IOException("Stream closed.");

            if (stream.CanWrite == false)
                return IOStatus.UNSUPPORTED;

            try
            {
                if (append)
                {
                    if (stream.CanSeek == false)
                        return IOStatus.UNSUPPORTED;

                    stream.Seek(0, SeekOrigin.End);
                }
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
                var buf = ArrayPool<byte>.Shared.Rent(len);

                try
                {
                    Marshal.Copy((IntPtr)address, buf, 0, len);
                    stream.Write(buf, 0, len);
                }
                finally
                {
                    ArrayPool<byte>.Shared.Return(buf);
                }
#else
                stream.Write(new ReadOnlySpan<byte>((byte*)(IntPtr)address, len));
#endif

                return len;
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
            finally
            {
                GC.KeepAlive(self);
            }
#endif
        }

        /// <summary>
        /// Implements the native method 'pwrite'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="fd"></param>
        /// <param name="address"></param>
        /// <param name="len"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        /// <exception cref="global::java.io.IOException"></exception>
        public static unsafe int pwrite(object self, object fd, long address, int len, long position)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (len == 0)
                return 0;

            var stream = FileDescriptorAccessor.GetStream(fd);
            if (stream == null)
                throw new global::java.io.IOException("Stream closed.");

            if (stream.CanWrite == false)
                return IOStatus.UNSUPPORTED;
            if (stream.CanSeek == false)
                return IOStatus.UNSUPPORTED;

            if (RuntimeUtil.IsLinux && stream is FileStream fs)
            {
                try
                {
                    if (fs.SafeFileHandle.IsInvalid)
                        return IOStatus.UNAVAILABLE;

                    var n = Syscall.pwrite((int)fs.SafeFileHandle.DangerousGetHandle(), (void*)(IntPtr)address, (ulong)len, position);
                    if (n == -1)
                    {
                        var errno = Stdlib.GetLastError();
                        if (errno == Errno.EAGAIN)
                            return IOStatus.UNAVAILABLE;
                        if (errno == Errno.EINTR)
                            return IOStatus.INTERRUPTED;

                        throw new global::java.io.IOException("Write failed.", new UnixIOException(errno));
                    }

                    return (int)n;
                }
                finally
                {
                    GC.KeepAlive(self);
                }
            }
            else
            {
                var p = stream.Position;

                try
                {
                    stream.Seek(position, SeekOrigin.Begin);
                }
                catch (EndOfStreamException)
                {
                    return 0;
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
                    var buf = ArrayPool<byte>.Shared.Rent(len);

                    try
                    {
                        Marshal.Copy((IntPtr)address, buf, 0, len);
                        stream.Write(buf, 0, len);
                    }
                    finally
                    {
                        ArrayPool<byte>.Shared.Return(buf);
                    }
#else
                    stream.Write(new ReadOnlySpan<byte>((byte*)(IntPtr)address, len));
#endif

                    stream.Seek(p, SeekOrigin.Begin);
                    return len;
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
                finally
                {
                    GC.KeepAlive(self);
                }
            }
#endif
        }

        /// <summary>
        /// Implements the native method 'writev0'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="fd"></param>
        /// <param name="address"></param>
        /// <param name="len"></param>
        /// <param name="append"></param>
        /// <returns></returns>
        public static unsafe long writev0(object self, object fd, long address, int len, bool append)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (len == 0)
                return 0;

            var stream = FileDescriptorAccessor.GetStream(fd);
            if (stream == null)
                throw new global::java.io.IOException("Stream closed.");

            if (stream.CanWrite == false)
                return IOStatus.UNSUPPORTED;

            try
            {
                if (append)
                {
                    if (stream.CanSeek == false)
                        return IOStatus.UNSUPPORTED;

                    stream.Seek(0, SeekOrigin.End);
                }
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
                // map io vecors to read into
                var vecs = new ReadOnlySpan<iovec>((byte*)(IntPtr)address, len);
                var length = 0;

                // issue independent writes for each vector
                for (int i = 0; i < vecs.Length; i++)
                {
#if NETFRAMEWORK
                    var buf = ArrayPool<byte>.Shared.Rent(vecs[i].iov_len);

                    try
                    {
                        Marshal.Copy(vecs[i].iov_base, buf, 0, vecs[i].iov_len);
                        stream.Write(buf, 0, vecs[i].iov_len);
                        length += vecs[i].iov_len;

                    }
                    finally
                    {
                        ArrayPool<byte>.Shared.Return(buf);
                    }
#else
                    stream.Write(new ReadOnlySpan<byte>((byte*)vecs[i].iov_base, vecs[i].iov_len));
                    length += vecs[i].iov_len;
#endif
                }

                return length;
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
            finally
            {
                GC.KeepAlive(self);
            }
#endif
        }

        /// <summary>
        /// Implements the native method 'force'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="fd"></param>
        /// <param name="metaData"></param>
        /// <returns></returns>
        public static int force(object self, object fd, bool metaData)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            FileDescriptorAccessor.InvokeSync(fd);
            return 0;
#endif
        }

        /// <summary>
        /// Implements the native method 'truncate'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="fd"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static int truncate(object self, object fd, long size)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            var s = FileDescriptorAccessor.GetStream(fd);
            if (s == null)
                throw new global::java.io.IOException("Stream closed.");

            if (s.Length < size)
                return 0;

            try
            {
                s.SetLength(size);
                return 0;
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
        /// Implements the native method 'size'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="fd"></param>
        /// <returns></returns>
        public static long size(object self, object fd)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            var s = FileDescriptorAccessor.GetStream(fd);
            if (s == null)
                throw new global::java.io.IOException("Stream closed.");

            try
            {
                return s.Length;
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
                throw new global::java.io.IOException("Size failed.", e);
            }
#endif
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
        static extern unsafe int LockFileEx(SafeFileHandle hFile, int dwFlags, int dwReserved, int nNumberOfBytesToLockLow, int nNumberOfBytesToLockHigh, NativeOverlapped* lpOverlapped);

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
        static extern unsafe int UnlockFileEx(SafeFileHandle hFile, int dwReserved, int nNumberOfBytesToUnlockLow, int nNumberOfBytesToUnlockHigh, NativeOverlapped* lpOverlapped);

        /// <summary>
        /// Implements the native method 'lock'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="fd"></param>
        /// <param name="blocking"></param>
        /// <param name="pos"></param>
        /// <param name="size"></param>
        /// <param name="shared"></param>
        /// <returns></returns>
        public static unsafe int @lock(object self, object fd, bool blocking, long pos, long size, bool shared)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            var s = FileDescriptorAccessor.GetStream(fd);
            if (s == null)
                throw new global::java.io.IOException("Stream closed.");
            if (s is not FileStream fs)
                throw new global::java.io.IOException("Not supported on non-file.");

            try
            {
                if (RuntimeUtil.IsWindows)
                {
                    NativeOverlapped* optr = null;

                    try
                    {
                        const int LOCKFILE_FAIL_IMMEDIATELY = 1;
                        const int LOCKFILE_EXCLUSIVE_LOCK = 2;
                        const int ERROR_LOCK_VIOLATION = 33;

                        var o = new Overlapped();
                        o.OffsetLow = (int)pos;
                        o.OffsetHigh = (int)(pos >> 32);

                        int flags = 0;
                        if (blocking == false)
                            flags |= LOCKFILE_FAIL_IMMEDIATELY;
                        if (shared == false)
                            flags |= LOCKFILE_EXCLUSIVE_LOCK;

                        optr = o.Pack(null, null);
                        int result = LockFileEx(fs.SafeFileHandle, flags, 0, (int)size, (int)(size >> 32), optr);
                        if (result == 0)
                        {
                            var error = Marshal.GetLastWin32Error();
                            if (error != ERROR_LOCK_VIOLATION)
                                throw new Win32Exception(error);

                            if ((flags & LOCKFILE_FAIL_IMMEDIATELY) != 0)
                                return FileDispatcher.NO_LOCK;

                            throw new Win32Exception(error);
                        }

                        return FileDispatcher.LOCKED;
                    }
                    finally
                    {
                        Overlapped.Free(optr);
                    }
                }
                else if (RuntimeUtil.IsLinux || RuntimeUtil.IsOSX)
                {
                    var fl = new Flock();
                    fl.l_whence = SeekFlags.SEEK_SET;
                    fl.l_len = size == long.MaxValue ? 0 : size;
                    fl.l_start = pos;
                    fl.l_type = shared ? LockType.F_RDLCK : LockType.F_WRLCK;
                    var cmd = blocking ? FcntlCommand.F_SETLKW : FcntlCommand.F_SETLK;

                    var r = Syscall.fcntl((int)fs.SafeFileHandle.DangerousGetHandle(), cmd, ref fl);
                    if (r == -1)
                    {
                        var errno = Stdlib.GetLastError();
                        if ((cmd == FcntlCommand.F_SETLK) && (errno == Errno.EAGAIN || errno == Errno.EACCES))
                            return FileDispatcher.NO_LOCK;
                        if (errno == Errno.EINTR)
                            return FileDispatcher.INTERRUPTED;

                        UnixMarshal.ThrowExceptionForError(errno);
                    }

                    return FileDispatcher.LOCKED;
                }
                else
                {
                    // fallback to .NET version for non-Windows
                    fs.Lock(pos, size);
                    return shared ? FileDispatcher.RET_EX_LOCK : FileDispatcher.LOCKED;
                }
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
        /// Implements the native method 'release'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="fd"></param>
        /// <param name="pos"></param>
        /// <param name="size"></param>
        /// <exception cref="NotImplementedException"></exception>
        public static unsafe void release(object self, object fd, long pos, long size)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            var s = FileDescriptorAccessor.GetStream(fd);
            if (s == null)
                throw new global::java.io.IOException("Stream closed.");
            if (s is not FileStream fs)
                throw new global::java.io.IOException("Not supported on non-file.");

            try
            {
                if (RuntimeUtil.IsWindows)
                {
                    NativeOverlapped* optr = null;

                    try
                    {
                        const int ERROR_NOT_LOCKED = 158;

                        var o = new Overlapped();
                        o.OffsetLow = (int)pos;
                        o.OffsetHigh = (int)(pos >> 32);

                        optr = o.Pack(null, null);
                        int result = UnlockFileEx(fs.SafeFileHandle, 0, (int)size, (int)(size >> 32), optr);
                        if (result == 0)
                        {
                            var error = Marshal.GetLastWin32Error();
                            if (error != ERROR_NOT_LOCKED)
                                throw new Win32Exception(error);
                        }
                    }
                    finally
                    {
                        if (optr != null)
                            Overlapped.Free(optr);
                    }
                }
                else if (RuntimeUtil.IsLinux || RuntimeUtil.IsOSX)
                {
                    var fl = new Flock();
                    fl.l_whence = SeekFlags.SEEK_SET;
                    fl.l_len = size == long.MaxValue ? 0 : size;
                    fl.l_start = pos;
                    fl.l_type = LockType.F_UNLCK;

                    var r = Syscall.fcntl((int)fs.SafeFileHandle.DangerousGetHandle(), FcntlCommand.F_SETLK, ref fl);
                    if (r == -1)
                        UnixMarshal.ThrowExceptionForLastErrorIf(r);
                }
                else
                {
                    fs.Unlock(pos, size);
                }
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
        /// Implements the native method 'close'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="fd"></param>
        public static void close(object self, object fd)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            var s = FileDescriptorAccessor.GetStream(fd);
            if (s == null)
                throw new global::java.io.IOException("Stream closed.");

            try
            {
                FileDescriptorAccessor.SetStream(fd, null);
                s.Close();
            }
            catch
            {

            }
#endif
        }

        /// <summary>
        /// Implements the native method 'duplicateForMapping'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="fd"></param>
        /// <returns></returns>
        public static object duplicateForMapping(object self, object fd)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            var newfd = FileDescriptorAccessor.Init();
            return newfd;
#endif
        }

    }

}