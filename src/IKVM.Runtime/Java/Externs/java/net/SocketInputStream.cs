using System;
using System.Net.Sockets;

using static IKVM.Java.Externs.java.net.SocketImplUtil;

namespace IKVM.Java.Externs.java.net
{

    static class SocketInputStream
    {

        public static void init()
        {

        }

        public static int socketRead0(object this_, global::java.io.FileDescriptor fd, byte[] b, int off, int len, int timeout)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            return InvokeFunc<global::java.net.SocketInputStream, int>(this_, impl =>
            {
                return InvokeFuncWithSocket(fd, socket =>
                {
                    var previousBlocking = socket.Blocking;
                    var previousReceiveTimeout = socket.ReceiveTimeout;

                    try
                    {
                        if (timeout > 0)
                        {
                            socket.Blocking = true;
                            socket.ReceiveTimeout = timeout;
                            return socket.Receive(b, off, len, SocketFlags.None);
                        }
                        else
                        {
                            socket.Blocking = true;
                            socket.ReceiveTimeout = 0;
                            return socket.Receive(b, off, len, SocketFlags.None);
                        }
                    }
                    finally
                    {
                        socket.Blocking = previousBlocking;
                        socket.ReceiveTimeout = previousReceiveTimeout;
                    }
                });
            });
#endif
        }

#if !FIRST_PASS

        /// <summary>
        /// Invokes the given function with the current socket, catching and mapping any resulting .NET exceptions.
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="fd"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        /// <exception cref="global::java.lang.NullPointerException"></exception>
        /// <exception cref="global::java.net.SocketException"></exception>
        static TResult InvokeFuncWithSocket<TResult>(global::java.io.FileDescriptor fd, Func<Socket, TResult> func)
        {
            var socket = fd?.getSocket();
            if (socket == null)
                throw new global::java.net.SocketException("Socket closed");

            return func(socket);
        }

#endif

    }

}
