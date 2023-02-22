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
        unsafe struct iovec
        {

            public void* iov_base;
            public int iov_len;

        }

        /// <summary>
        /// Implements the native method 'read0'.
        /// </summary>
        /// <param name="fd"></param>
        /// <param name="address"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static unsafe int read0(object fd, long address, int len)
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
        /// Implements the native method 'readv0'.
        /// </summary>
        /// <param name="fd"></param>
        /// <param name="address"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static unsafe long readv0(object fd, long address, int len)
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
                var iov = new Span<iovec>((byte*)(IntPtr)address, len);
                var seg = new ArraySegment<byte>[iov.Length];

                try
                {
                    // allocate temporary segments
                    for (int i = 0; i < iov.Length; i++)
                        seg[i] = new ArraySegment<byte>(ArrayPool<byte>.Shared.Rent(iov[i].iov_len), 0, iov[i].iov_len);

                    // receive into segments
                    var n = socket.Receive(seg);

                    // copy segments into original buffers
                    for (int i = 0; i < seg.Length; i++)
                        Marshal.Copy(seg[i].Array, seg[i].Offset, (IntPtr)iov[i].iov_base, n);

                    return n;
                }
                finally
                {
                    // return allocated arrays
                    for (int i = 0; i < seg.Length; i++)
                        ArrayPool<byte>.Shared.Return(seg[i].Array);
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
        /// Implements the native method 'write0'.
        /// </summary>
        /// <param name="fd"></param>
        /// <param name="address"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static unsafe int write0(object fd, long address, int len)
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
        /// Implements the native method 'writev0'.
        /// </summary>
        /// <param name="fd"></param>
        /// <param name="address"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static unsafe long writev0(object fd, long address, int len)
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
                var iov = new ReadOnlySpan<iovec>((void*)(IntPtr)address, len);
                var seg = new ArraySegment<byte>[iov.Length];

                try
                {
                    // copy incoming buffers into segments
                    for (int i = 0; i < iov.Length; i++)
                    {
                        seg[i] = new ArraySegment<byte>(ArrayPool<byte>.Shared.Rent(iov[i].iov_len), 0, iov[i].iov_len);
                        new ReadOnlySpan<byte>(iov[i].iov_base, iov[i].iov_len).CopyTo(seg[i]);
                    }

                    // send segments
                    return socket.Send(seg);
                }
                finally
                {
                    // return allocated arrays
                    for (int i = 0; i < seg.Length; i++)
                        if (seg[i].Array != null)
                            ArrayPool<byte>.Shared.Return(seg[i].Array);
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
        /// Implements the native method 'preClose0'.
        /// </summary>
        /// <param name="fd"></param>
        public static void preClose0(object fd)
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
        /// Implements the native method 'close0'.
        /// </summary>
        /// <param name="fd"></param>
        public static void close0(object fd)
        {
#if FIRST_PASS
            throw new NotSupportedException();
#else
            var socket = FileDescriptorAccessor.GetSocket(fd);
            if (socket == null)
                return;

            try
            {
                FileDescriptorAccessor.SetSocket(fd, null);
                socket.Close();
            }
            catch (SocketException e)
            {
                return;
            }
            catch (ObjectDisposedException)
            {
                return;
            }
            catch (Exception e)
            {
                throw new global::java.io.IOException(e);
            }
#endif
        }

    }

}
