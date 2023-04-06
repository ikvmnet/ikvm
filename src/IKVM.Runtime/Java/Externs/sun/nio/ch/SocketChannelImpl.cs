using System;
using System.Buffers;
using System.Net.Sockets;

using IKVM.Runtime;
using IKVM.Runtime.Accessors.Java.Io;
using IKVM.Runtime.Util.Java.Net;

namespace IKVM.Java.Externs.sun.nio.ch
{

    /// <summary>
    /// Implements the native methods for 'SocketChannelImpl'.
    /// </summary>
    static class SocketChannelImpl
    {

#if FIRST_PASS == false

        static FileDescriptorAccessor fileDescriptorAccessor;
        static FileDescriptorAccessor FileDescriptorAccessor => JVM.BaseAccessors.Get(ref fileDescriptorAccessor);

#endif

        /// <summary>
        /// Implements the native method 'checkConnect'.
        /// </summary>
        /// <param name="fd"></param>
        /// <param name="block"></param>
        /// <param name="ready"></param>
        /// <returns></returns>
        public static int checkConnect(object fd, bool block, bool ready)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            var socket = FileDescriptorAccessor.GetSocket(fd);
            if (socket == null)
                throw new global::java.io.IOException("Socket closed.");

            try
            {
                var task = FileDescriptorAccessor.GetTask(fd);
                if (block || ready || task.IsCompleted)
                {
                    FileDescriptorAccessor.SetTask(fd, null);
                    task.GetAwaiter().GetResult();
                    return 1;
                }
                else
                {
                    return global::sun.nio.ch.IOStatus.UNAVAILABLE;
                }
            }
            catch (SocketException e)
            {
                throw new global::java.net.ConnectException(e.Message);
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
        /// Implements the native method 'sendOutOfBandData'.
        /// </summary>
        /// <param name="fd"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int sendOutOfBandData(object fd, byte data)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            var socket = FileDescriptorAccessor.GetSocket(fd);
            if (socket == null)
                throw new global::java.io.IOException("Socket closed.");

            try
            {
#if NETFRAMEWORK
                var buf = ArrayPool<byte>.Shared.Rent(1);

                try
                {
                    buf[0] = data;
                    return socket.Send(buf, 1, SocketFlags.OutOfBand);
                }
                finally
                {
                    ArrayPool<byte>.Shared.Return(buf);
                }
#else
                unsafe
                {
                    var buf = (Span<byte>)stackalloc byte[1];
                    buf[0] = data;
                    return socket.Send(buf, SocketFlags.OutOfBand);
                }
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

    }

}
