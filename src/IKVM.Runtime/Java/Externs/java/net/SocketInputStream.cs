using System;
using System.Net.Sockets;

namespace IKVM.Java.Externs.java.net
{

    static class SocketInputStream
    {

        public static void init()
        {

        }

        public static int socketRead0(object self, global::java.io.FileDescriptor fd, byte[] b, int off, int len, int timeout)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (fd == null)
                throw new global::java.net.SocketException("Socket closed.");

            var socket = fd.getSocket();
            if (socket == null)
                throw new global::java.net.SocketException("Socket closed.");

            int prevRecv = socket.ReceiveTimeout;

            try
            {
                socket.ReceiveTimeout = timeout;
                return socket.Receive(b, off, len, SocketFlags.None);
            }
            catch (SocketException e)
            {
                throw e.ToIOException();
            }
            finally
            {
                socket.ReceiveTimeout = prevRecv;
            }
#endif
        }

    }

}
