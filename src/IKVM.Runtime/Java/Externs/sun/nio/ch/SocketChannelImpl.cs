using System;
using System.Net.Sockets;

using IKVM.Internal;
using IKVM.Runtime.Accessors.Java.Io;

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
                    task.Wait();
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
                socket.Send(new byte[] { data }, 1, SocketFlags.OutOfBand);
                return 1;
            }
            catch (SocketException e)
            {
                throw new global::java.net.ConnectException(e.Message);
            }
            catch (ObjectDisposedException)
            {
                throw new global::java.net.SocketException("Socket closed");
            }
#endif
        }

    }

}
