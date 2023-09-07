using System;
using System.Buffers;
using System.IO;
using System.Runtime.InteropServices;

using IKVM.Runtime;
using IKVM.Runtime.Accessors.Java.Io;

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
        /// <param name="fd"></param>
        /// <param name="address"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        /// <exception cref="global::java.io.IOException"></exception>
        public static unsafe int read0(object fd, long address, int len)
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
#endif
        }

        /// <summary>
        /// Implements the native method 'pread'.
        /// </summary>
        /// <param name="fd"></param>
        /// <param name="address"></param>
        /// <param name="len"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        /// <exception cref="global::java.io.IOException"></exception>
        public static unsafe int pread0(object fd, long address, int len, long position)
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
            }
#endif
        }

        /// <summary>
        /// Implements the native method 'readv'.
        /// </summary>
        /// <param name="fd"></param>
        /// <param name="address"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static unsafe long readv0(object fd, long address, int len)
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
#endif
        }

        /// <summary>
        /// Implements the native method 'write'.
        /// </summary>
        /// <param name="fd"></param>
        /// <param name="address"></param>
        /// <param name="len"></param>
        /// <param name="append"></param>
        /// <returns></returns>
        /// <exception cref="global::java.io.IOException"></exception>
        public static unsafe int write0(object fd, long address, int len, bool append)
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
                    stream.Flush();
                }
                finally
                {
                    ArrayPool<byte>.Shared.Return(buf);
                }
#else
                stream.Write(new ReadOnlySpan<byte>((byte*)(IntPtr)address, len));
                stream.Flush();
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
#endif
        }

        /// <summary>
        /// Implements the native method 'pwrite'.
        /// </summary>
        /// <param name="fd"></param>
        /// <param name="address"></param>
        /// <param name="len"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        /// <exception cref="global::java.io.IOException"></exception>
        public static unsafe int pwrite0(object fd, long address, int len, long position)
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
                        stream.Flush();
                    }
                    finally
                    {
                        ArrayPool<byte>.Shared.Return(buf);
                    }
#else
                    stream.Write(new ReadOnlySpan<byte>((byte*)(IntPtr)address, len));
                    stream.Flush();
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
        public static unsafe long writev0(object fd, long address, int len, bool append)
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
                        stream.Flush();
                        length += vecs[i].iov_len;

                    }
                    finally
                    {
                        ArrayPool<byte>.Shared.Return(buf);
                    }
#else
                    stream.Write(new ReadOnlySpan<byte>((byte*)vecs[i].iov_base, vecs[i].iov_len));
                    stream.Flush();
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
#endif
        }

        /// <summary>
        /// Implements the native method 'force'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="fd"></param>
        /// <param name="metaData"></param>
        /// <returns></returns>
        public static int force0(object fd, bool metaData)
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
        /// <param name="fd"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static int truncate0(object fd, long size)
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
        /// <param name="fd"></param>
        /// <returns></returns>
        public static long size0(object fd)
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
        /// Implements the native method 'close'.
        /// </summary>
        /// <param name="fd"></param>
        public static void close0(object fd)
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
        /// Implements the native method 'transferToDirectlyNeedsPositionLock0'.
        /// </summary>
        /// <returns></returns>
        public static bool transferToDirectlyNeedsPositionLock0()
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            return RuntimeUtil.IsWindows;
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