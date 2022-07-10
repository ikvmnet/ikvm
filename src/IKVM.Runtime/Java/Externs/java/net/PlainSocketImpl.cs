using System;

namespace IKVM.Java.Externs.java.net
{

#if !FIRST_PASS

    static class PlainSocketImpl
    {

        public static void initProto()
        {

        }

        public static void socketCreate(global::java.net.PlainSocketImpl self, bool isServer)
        {
            throw new NotImplementedException();
        }

        public static void socketConnect(global::java.net.PlainSocketImpl self, global::java.net.InetAddress address, int port, int timeout)
        {
            throw new NotImplementedException();
        }

        public static void socketBind(global::java.net.PlainSocketImpl self, global::java.net.InetAddress address, int port)
        {
            throw new NotImplementedException();
        }

        public static void socketListen(global::java.net.PlainSocketImpl self, int count)
        {
            throw new NotImplementedException();
        }

        public static void socketAccept(global::java.net.PlainSocketImpl self, global::java.net.SocketImpl s)
        {
            throw new NotImplementedException();
        }

        public static int socketAvailable(global::java.net.PlainSocketImpl self)
        {
            throw new NotImplementedException();
        }

        public static void socketClose0(global::java.net.PlainSocketImpl self, bool useDeferredClose)
        {
            throw new NotImplementedException();
        }

        public static void socketShutdown(global::java.net.PlainSocketImpl self, int howto)
        {
            throw new NotImplementedException();
        }

        public static void socketSetOption0(global::java.net.PlainSocketImpl self, int cmd, bool on, object value)
        {
            throw new NotImplementedException();
        }

        public static int socketGetOption(global::java.net.PlainSocketImpl self, int opt, object iaContainerObj)
        {
            throw new NotImplementedException();
        }

        public static void socketSendUrgentData(global::java.net.PlainSocketImpl self, int data)
        {
            throw new NotImplementedException();
        }

    }

#endif

}
