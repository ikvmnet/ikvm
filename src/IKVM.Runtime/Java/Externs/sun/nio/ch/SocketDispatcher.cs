using System;
using System.Net.Sockets;
using System.Runtime.InteropServices;

using IKVM.Runtime.Util.Java.Net;

namespace IKVM.Java.Externs.sun.nio.ch
{

    /// <summary>
    /// Implements the native methods for 'SocketDispatcher'.
    /// </summary>
    static class SocketDispatcher
    {

        /// <summary>
        /// Implements the native funtionality for 'read0'.
        /// </summary>
        /// <param name="fd"></param>
        /// <param name="address"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        /// <exception cref="global::java.net.SocketException"></exception>
        public static unsafe long read0(global::java.io.FileDescriptor fd, long address, int len)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            try
            {
                var buf = new byte[len];
                var rec = fd.getSocket().Receive(buf);
                buf.CopyTo(new Span<byte>((void*)(IntPtr)address, rec));
                return rec;
            }
            catch (SocketException e) when (e.SocketErrorCode == SocketError.WouldBlock)
            {
                return 0;
            }
            catch (SocketException e)
            {
                throw e.ToIOException();
            }
            catch (ObjectDisposedException)
            {
                throw new global::java.net.SocketException("Socket is closed");
            }
#endif
        }

        /// <summary>
        /// Implements the native funtionality for 'readv0'.
        /// </summary>
        /// <param name="fd"></param>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static long readv0(global::java.io.FileDescriptor fd, long address, int length)
        {
#if FIRST_PASS
            throw new NotSupportedException();
#else
            throw new NotImplementedException();
#endif

        }

        /// <summary>
        /// Implements the native funtionality for 'write0'.
        /// </summary>
        /// <param name="fd"></param>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        /// <exception cref="global::java.net.SocketException"></exception>
        public static int write0(global::java.io.FileDescriptor fd, long address, int length)
        {
#if FIRST_PASS
            throw new NotSupportedException();
#else
            try
            {
                var buf = new byte[length];
                Marshal.Copy((IntPtr)address, buf, 0, length);
                return fd.getSocket().Send(buf);
            }
            catch (SocketException e) when (e.SocketErrorCode == SocketError.WouldBlock)
            {
                return 0;
            }
            catch (SocketException e)
            {
                throw e.ToIOException();
            }
            catch (ObjectDisposedException)
            {
                throw new global::java.net.SocketException("Socket is closed");
            }
#endif
        }

        /// <summary>
        /// Implements the native funtionality for 'writev0'.
        /// </summary>
        /// <param name="fd"></param>
        /// <param name="address"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static long writev0(global::java.io.FileDescriptor fd, long address, int len)
        {
#if FIRST_PASS
            throw new NotSupportedException();
#else
            throw new NotImplementedException();
#endif
        }

        /// <summary>
        /// Implements the native funtionality for 'preClose0'.
        /// </summary>
        /// <param name="fd"></param>
        /// <exception cref="NotImplementedException"></exception>
        public static void preClose0(global::java.io.FileDescriptor fd)
        {
#if FIRST_PASS
            throw new NotSupportedException();
#else
            throw new NotImplementedException();
#endif
        }

        /// <summary>
        /// Implements the native funtionality for 'close0'.
        /// </summary>
        /// <param name="fd"></param>
        /// <exception cref="NotImplementedException"></exception>
        public static void close0(global::java.io.FileDescriptor fd)
        {
#if FIRST_PASS
            throw new NotSupportedException();
#else
            throw new NotImplementedException();
#endif
        }

    }

}
