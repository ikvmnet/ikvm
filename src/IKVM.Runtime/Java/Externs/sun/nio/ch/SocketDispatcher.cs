using System;
using System.Buffers;
using System.Net.Sockets;
using System.Runtime.InteropServices;

using IKVM.Runtime;
using IKVM.Runtime.Accessors.Java.Io;
using IKVM.Runtime.Util.Java.Net;

namespace IKVM.Java.Externs.sun.nio.ch
{

    /// <summary>
    /// Implements the native methods for 'SocketDispatcher'.
    /// </summary>
    static class SocketDispatcher
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
        public static unsafe int read(object self, object fd, long address, int len)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (len == 0)
                return 0;

            var socket = FileDescriptorAccessor.GetSocket(fd);
            if (socket == null)
                throw new global::java.net.SocketException("Socket closed.");

            try
            {
#if NETFRAMEWORK
                var buf = ArrayPool<byte>.Shared.Rent(len);

                try
                {
                    var n = socket.Receive(buf, 0, len, SocketFlags.None);
                    if (n == 0)
                        return global::sun.nio.ch.IOStatus.EOF;

                    Marshal.Copy(buf, 0, (IntPtr)address, n);
                    return n;
                }
                finally
                {
                    ArrayPool<byte>.Shared.Return(buf);
                }
#else
                var n = socket.Receive(new Span<byte>((byte*)(IntPtr)address, len));
                if (n == 0)
                    return global::sun.nio.ch.IOStatus.EOF;

                return n;
#endif
            }
            catch (SocketException e) when (e.SocketErrorCode == SocketError.Shutdown)
            {
                return global::sun.nio.ch.IOStatus.EOF;
            }
            catch (SocketException e) when (e.SocketErrorCode == SocketError.WouldBlock)
            {
                return global::sun.nio.ch.IOStatus.UNAVAILABLE;
            }
            catch (SocketException e)
            {
                throw e.ToIOException();
            }
            catch (ObjectDisposedException)
            {
                throw new global::java.net.SocketException("Socket closed.");
            }
            catch (Exception e)
            {
                throw new global::java.io.IOException(e);
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
            throw new NotSupportedException();
#else
            if (len == 0)
                return 0;

            var socket = FileDescriptorAccessor.GetSocket(fd);
            if (socket == null)
                throw new global::java.net.SocketException("Socket closed.");

            try
            {
                // allocate list of array segments to hold received data
                var vecs = new Span<iovec>((byte*)(IntPtr)address, len);
                var bufs = new ArraySegment<byte>[vecs.Length];

                try
                {
                    // prepare array segments for buffers
                    for (int i = 0; i < vecs.Length; i++)
                        bufs[i] = new ArraySegment<byte>(ArrayPool<byte>.Shared.Rent(vecs[i].iov_len), 0, vecs[i].iov_len);

                    // receive into segments
                    var length = socket.Receive(bufs);

                    // copy segments back into vectors
                    var l = length;
                    for (int i = 0; i < vecs.Length; i++)
                    {
                        var szz = Math.Min(l, vecs[i].iov_len);
                        Marshal.Copy(bufs[i].Array, 0, vecs[i].iov_base, szz);
                        l -= szz;
                    }

                    // we should have accounted for all of the bytes
                    if (l != 0)
                        throw new global::java.lang.RuntimeException("Bytes remaining after read.");

                    return length;
                }
                finally
                {
                    for (int i = 0; i < bufs.Length; i++)
                        ArrayPool<byte>.Shared.Return(bufs[i].Array);
                }
            }
            catch (SocketException e) when (e.SocketErrorCode == SocketError.Shutdown)
            {
                return global::sun.nio.ch.IOStatus.EOF;
            }
            catch (SocketException e) when (e.SocketErrorCode == SocketError.WouldBlock)
            {
                return global::sun.nio.ch.IOStatus.UNAVAILABLE;
            }
            catch (SocketException e)
            {
                throw e.ToIOException();
            }
            catch (ObjectDisposedException)
            {
                throw new global::java.net.SocketException("Socket closed.");
            }
            catch (Exception e)
            {
                throw new global::java.io.IOException(e);
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
        /// <returns></returns>
        public static unsafe int write(object self, object fd, long address, int len)
        {
#if FIRST_PASS
            throw new NotSupportedException();
#else
            var socket = FileDescriptorAccessor.GetSocket(fd);
            if (socket == null)
                throw new global::java.net.SocketException("Socket closed.");

            try
            {
#if NETFRAMEWORK
                var buf = ArrayPool<byte>.Shared.Rent(len);

                try
                {
                    Marshal.Copy((IntPtr)address, buf, 0, len);
                    return socket.Send(buf, 0, len, SocketFlags.None);
                }
                finally
                {
                    ArrayPool<byte>.Shared.Return(buf);
                }
#else
                return socket.Send(new ReadOnlySpan<byte>((byte*)(IntPtr)address, len));
#endif
            }
            catch (SocketException e) when (e.SocketErrorCode == SocketError.WouldBlock)
            {
                return global::sun.nio.ch.IOStatus.UNAVAILABLE;
            }
            catch (SocketException e)
            {
                throw e.ToIOException();
            }
            catch (ObjectDisposedException)
            {
                throw new global::java.net.SocketException("Socket closed.");
            }
            catch (Exception e)
            {
                throw new global::java.io.IOException(e);
            }
#endif
        }

        /// <summary>
        /// Implements the native method 'writev'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="fd"></param>
        /// <param name="address"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static unsafe long writev(object self, object fd, long address, int len)
        {
#if FIRST_PASS
            throw new NotSupportedException();
#else
            var socket = FileDescriptorAccessor.GetSocket(fd);
            if (socket == null)
                throw new global::java.net.SocketException("Socket closed.");

            try
            {
                // allocate list of array segments to hold incoming buffers
                var vecs = new ReadOnlySpan<iovec>((void*)(IntPtr)address, len);
                var bufs = new ArraySegment<byte>[vecs.Length];

                try
                {
                    // copy incoming buffers into segments
                    for (int i = 0; i < vecs.Length; i++)
                    {
                        bufs[i] = new ArraySegment<byte>(ArrayPool<byte>.Shared.Rent(vecs[i].iov_len), 0, vecs[i].iov_len);
                        Marshal.Copy(vecs[i].iov_base, bufs[i].Array, bufs[i].Offset, vecs[i].iov_len);
                    }

                    // send segments
                    return socket.Send(bufs);
                }
                finally
                {
                    // return allocated arrays
                    for (int i = 0; i < bufs.Length; i++)
                        if (bufs[i].Array != null)
                            ArrayPool<byte>.Shared.Return(bufs[i].Array);
                }
            }
            catch (SocketException e) when (e.SocketErrorCode == SocketError.WouldBlock)
            {
                return global::sun.nio.ch.IOStatus.UNAVAILABLE;
            }
            catch (SocketException e)
            {
                throw e.ToIOException();
            }
            catch (ObjectDisposedException)
            {
                throw new global::java.net.SocketException("Socket closed.");
            }
            catch (Exception e)
            {
                throw new global::java.io.IOException(e);
            }
#endif
        }

        /// <summary>
        /// Implements the native method 'preClose'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="fd"></param>
        public static void preClose(object self, object fd)
        {
#if FIRST_PASS
            throw new NotSupportedException();
#else
            var socket = FileDescriptorAccessor.GetSocket(fd);
            if (socket == null)
                return;

            try
            {
                if (socket.LingerState.Enabled == false)
                    if (socket.Connected)
                        socket.Shutdown(SocketShutdown.Send);
            }
            catch (ObjectDisposedException)
            {
                return;
            }
            catch (SocketException)
            {
                // ignore
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
            throw new NotSupportedException();
#else
            if (fd == null)
                return;

            try
            {
                var h = FileDescriptorAccessor.GetHandle(fd);
                FileDescriptorAccessor.SetHandle(fd, -1);
                LibIkvm.Instance.io_close_socket(h);
            }
            catch
            {
                // ignore
            }
#endif
        }

    }

}
