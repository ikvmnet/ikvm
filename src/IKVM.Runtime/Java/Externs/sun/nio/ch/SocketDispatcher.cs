using System;
using System.Buffers;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Runtime.InteropServices;

using IKVM.Internal;
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
        public static unsafe long read0(object fd, long address, int len)
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
                var buf = new byte[len];
                var rec = socket.Receive(buf);
                if (rec == 0)
                    return global::sun.nio.ch.IOStatus.EOF;

                buf.CopyTo(new Span<byte>((void*)(IntPtr)address, rec));
                return rec;
#else
                var buf = new Span<byte>((void*)(IntPtr)address, len);
                var rec = socket.Receive(buf);
                if (rec == 0)
                    return global::sun.nio.ch.IOStatus.EOF;

                return rec;
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
                var iov = new Span<iovec>((void*)(IntPtr)address, len);
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
                        seg[i].AsSpan().CopyTo(new Span<byte>(iov[i].iov_base, iov[i].iov_len));

                    return n;
                }
                finally
                {
                    // return allocated arrays
                    for (int i = 0; i < seg.Length; i++)
                        if (seg[i].Array != null)
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
#endif

        }

        /// <summary>
        /// Implements the native method 'write0'.
        /// </summary>
        /// <param name="fd"></param>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static int write0(object fd, long address, int length)
        {
#if FIRST_PASS
            throw new NotSupportedException();
#else
            var socket = FileDescriptorAccessor.GetSocket(fd);
            if (socket == null)
                throw new global::java.net.SocketException("Socket closed.");

            try
            {
                var buf = new byte[length];
                Marshal.Copy((IntPtr)address, buf, 0, length);
                return socket.Send(buf);
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

            // if we're not configured to linger, disable sending, but continue to allow receive
            if (socket.LingerState.Enabled == false)
            {
                try
                {
                    socket.Disconnect(false);
                }
                catch (ObjectDisposedException)
                {
                    return;
                }
                catch (SocketException)
                {
                    // ignore
                }
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
                socket.Close();
            }
            catch (ObjectDisposedException)
            {
                return;
            }
            catch (SocketException e)
            {
                throw e.ToIOException();
            }
#endif
        }

    }

}
