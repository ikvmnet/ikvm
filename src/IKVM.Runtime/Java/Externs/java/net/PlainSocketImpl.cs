using System;
using System.Net;
using System.Net.Sockets;
using System.Reflection;

using IKVM.Runtime.Java.Externs.java.net;

using static IKVM.Java.Externs.java.net.SocketImplUtil;

namespace IKVM.Java.Externs.java.net
{

    /// <summary>
    /// Implements the external methods for <see cref="global::java.net.PlainSocketImpl"/>.
    /// </summary>
    static class PlainSocketImpl
    {

#if !FIRST_PASS

        static readonly byte[] PeekBuffer = new byte[1];
        static readonly FieldInfo PlainSocketImplLocalPortField = typeof(global::java.net.PlainSocketImpl).GetField("localport", BindingFlags.NonPublic | BindingFlags.Instance);
        static readonly FieldInfo PlainSocketImplTimeoutField = typeof(global::java.net.PlainSocketImpl).GetField("timeout", BindingFlags.NonPublic | BindingFlags.Instance);
        static readonly FieldInfo AbstractPlainSocketImplTrafficClassField = typeof(global::java.net.AbstractPlainSocketImpl).GetField("trafficClass", BindingFlags.NonPublic | BindingFlags.Instance);
        static readonly FieldInfo PlainSocketImplServerSocketField = typeof(global::java.net.PlainSocketImpl).GetField("serverSocket", BindingFlags.NonPublic | BindingFlags.Instance);

#endif

        /// <summary>
        /// Implements the native method for 'initProto'.
        /// </summary>
        public static void initProto()
        {

        }

        /// <summary>
        /// Implements the native method for 'socketCreate'.
        /// </summary>
        public static void socketCreate(object this_, bool stream)
        {
#if FIRST_PASS
            throw new NotSupportedException();
#else
            InvokeAction<global::java.net.PlainSocketImpl>(this_, impl =>
            {
                var socket = new Socket(stream ? SocketType.Stream : SocketType.Dgram, ProtocolType.Unspecified);

                // disable IPV6_V6ONLY to ensure dual-socket support
                if (Socket.OSSupportsIPv6)
                    socket.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.IPv6Only, false);

                // if this is a server socket then enable SO_REUSEADDR automatically
                if (PlainSocketImplServerSocketField.GetValue(impl) != null)
                    socket.ExclusiveAddressUse = false;

                impl.fd.setSocket(socket);
            });
#endif
        }

        /// <summary>
        /// Implements the native method for 'socketConnect'.
        /// </summary>
        public static void socketConnect(object this_, object address_, int port, int timeout)
        {
#if FIRST_PASS
            throw new NotSupportedException();
#else
            InvokeAction<global::java.net.PlainSocketImpl, global::java.net.InetAddress>(this_, address_, (impl, address) =>
            {
                InvokeActionWithSocket(impl, socket =>
                {
                    var localPort = (int)PlainSocketImplLocalPortField.GetValue(impl);

                    if (Socket.OSSupportsIPv6)
                    {
                        var trafficClass = (int)AbstractPlainSocketImplTrafficClassField.GetValue(impl);
                        if (trafficClass != 0)
                            socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.TypeOfService, trafficClass);
                    }

                    if (timeout <= 0)
                    {
                        socket.Connect(address.ToIPAddress(), port);
                    }
                    else
                    {
                        // use asynchronous completion to simulate a blocking timeout
                        var result = socket.BeginConnect(address.ToIPAddress(), port, null, null);
                        if (result.AsyncWaitHandle.WaitOne(timeout, true) == false)
                        {
                            socket.Close();
                            throw new global::java.net.SocketTimeoutException("Connection timed out.");
                        }

                        socket.EndConnect(result);
                    }

                    impl.setAddress(address);
                    impl.setPort(port);

                    if (localPort == 0)
                        impl.setLocalPort(((IPEndPoint)socket.LocalEndPoint).Port);
                });
            });
#endif
        }

        /// <summary>
        /// Implements the native method for 'socketBind'.
        /// </summary>
        public static void socketBind(object this_, global::java.net.InetAddress address_, int port)
        {
#if FIRST_PASS
            throw new NotSupportedException();
#else
            InvokeAction<global::java.net.PlainSocketImpl, global::java.net.InetAddress>(this_, address_, (impl, address) =>
            {
                InvokeActionWithSocket(impl, socket =>
                {
                    socket.Bind(new IPEndPoint(address.ToIPAddress(), port));

                    if (port == 0)
                        impl.setLocalPort(((IPEndPoint)socket.LocalEndPoint).Port);
                    else
                        impl.setLocalPort(port);
                });
            });
#endif
        }

        /// <summary>
        /// Implements the native method for 'socketListen'.
        /// </summary>
        public static void socketListen(object this_, int count)
        {
#if FIRST_PASS
            throw new NotSupportedException();
#else
            InvokeAction<global::java.net.PlainSocketImpl>(this_, impl =>
            {
                InvokeActionWithSocket(impl, socket =>
                {
                    socket.Listen(count);
                });
            });
#endif
        }

        /// <summary>
        /// Implements the native method for 'socketAccept'.
        /// </summary>
        public static void socketAccept(object this_, object s_)
        {
#if FIRST_PASS
            throw new NotSupportedException();
#else
            InvokeAction<global::java.net.PlainSocketImpl, global::java.net.AbstractPlainSocketImpl>(this_, s_, (impl, s) =>
            {
                InvokeActionWithSocket(impl, socket =>
                {
                    Socket newSocket;

                    var timeout = (int)PlainSocketImplTimeoutField.GetValue(impl);
                    if (timeout <= 0)
                    {
                        newSocket = socket.Accept();
                    }
                    else
                    {
                        // use asynchronous completion to simulate a blocking timeout
                        var result = socket.BeginAccept(null, null);
                        if (result.AsyncWaitHandle.WaitOne(timeout, true) == false)
                            throw new global::java.net.SocketTimeoutException("Accept timed out.");

                        newSocket = socket.EndAccept(result);
                    }

                    if (newSocket == null)
                        throw new global::java.net.SocketException("Invalid socket.");

                    // associate new FileDescriptor with socket
                    var newfd = new global::java.io.FileDescriptor();
                    newfd.setSocket(newSocket);

                    // populate newly accepted socket
                    var remoteIpEndPoint = (IPEndPoint)newSocket.RemoteEndPoint;
                    s.setFileDescriptor(newfd);
                    s.setAddress(remoteIpEndPoint.ToInetAddress());
                    s.setPort(remoteIpEndPoint.Port);
                    s.setLocalPort(impl.port);
                });
            });
#endif
        }

        /// <summary>
        /// Implements the native method for 'socketAvailable'.
        /// </summary>
        public static int socketAvailable(object this_)
        {
#if FIRST_PASS
            throw new NotSupportedException();
#else
            return InvokeFunc<global::java.net.PlainSocketImpl, int>(this_, impl =>
            {
                return InvokeFuncWithSocket(impl, socket =>
                {
                    return socket.Available;
                });
            });
#endif
        }

        /// <summary>
        /// Implements the native method for 'socketClose0'.
        /// </summary>
        public static void socketClose0(object this_, bool useDeferredClose)
        {
#if FIRST_PASS
            throw new NotSupportedException();
#else
            InvokeAction<global::java.net.PlainSocketImpl>(this_, impl =>
            {
                InvokeActionWithSocket(impl, socket =>
                {
                    // intended to "pre-close" the file descriptor, not useful on .NET
                    if (useDeferredClose)
                        return;

                    socket.Close();
                    impl.fd.setSocket(null);
                });
            });
#endif
        }

        /// <summary>
        /// Implements the native method for 'socketShutdown'.
        /// </summary>
        public static void socketShutdown(object this_, int howto)
        {
#if FIRST_PASS
            throw new NotSupportedException();
#else
            InvokeAction<global::java.net.PlainSocketImpl>(this_, impl =>
            {
                InvokeActionWithSocket(impl, socket =>
                {
                    socket.Shutdown((SocketShutdown)howto);
                });
            });
#endif
        }

        /// <summary>
        /// Implements the native method for 'socketSetOption0'.
        /// </summary>
        public static void socketSetOption0(object this_, int cmd, bool on, object value)
        {
#if FIRST_PASS
            throw new NotSupportedException();
#else
            InvokeAction<global::java.net.PlainSocketImpl>(this_, impl =>
            {
                InvokeActionWithSocket(impl, socket =>
                {
                    if (value == null)
                        throw new global::java.lang.NullPointerException(nameof(value));

                    // .NET provides property
                    if (cmd == global::java.net.SocketOptions.SO_LINGER)
                    {
                        var linger = (global::java.lang.Integer)value;
                        socket.LingerState = new LingerOption(on, linger.intValue());
                        return;
                    }

                    if (SocketOptionUtil.TryGetDotNetSocketOption(cmd, out var options) == false)
                        throw new global::java.net.SocketException("Invalid option.");

                    socket.SetSocketOption(options.Level, options.Name, value switch
                    {
                        global::java.lang.Integer i => i.intValue(),
                        global::java.lang.Boolean b => b.booleanValue(),
                        _ => new global::java.net.SocketException("Invalid option value."),
                    });
                });
            });
#endif
        }

        /// <summary>
        /// Implements the native method for 'socketGetOption'.
        /// </summary>
        public static int socketGetOption(object this_, int opt, object iaContainerObj_)
        {
#if FIRST_PASS
            throw new NotSupportedException();
#else
            return InvokeFunc<global::java.net.PlainSocketImpl, global::java.net.InetAddressContainer, int>(this_, iaContainerObj_, (impl, iaContainerObj) =>
            {
                return InvokeFuncWithSocket<int>(impl, socket =>
                {
                    // .NET provides property
                    if (opt == global::java.net.SocketOptions.SO_BINDADDR)
                    {
                        iaContainerObj.addr = ((IPEndPoint)socket.LocalEndPoint).ToInetAddress();
                        return 0;
                    }

                    // .NET provides property
                    if (opt == global::java.net.SocketOptions.SO_LINGER)
                        return socket.LingerState.Enabled ? socket.LingerState.LingerTime : -1;

                    if (SocketOptionUtil.TryGetDotNetSocketOption(opt, out var options) == false)
                        throw new global::java.net.SocketException("Invalid option.");

                    return socket.GetSocketOption(options.Level, options.Name) switch
                    {
                        bool b => b ? 1 : -1,
                        int i => i,
                        _ => throw new global::java.net.SocketException("Invalid option value."),
                    };
                });
            });
#endif
        }

        /// <summary>
        /// Implements the native method for 'socketSendUrgentData'.
        /// </summary>
        public static void socketSendUrgentData(object this_, int data)
        {
#if FIRST_PASS
            throw new NotSupportedException();
#else
            InvokeAction<global::java.net.PlainSocketImpl>(this_, impl =>
            {
                InvokeActionWithSocket(impl, socket =>
                {
                    throw new NotImplementedException();
                });
            });
#endif
        }

#if !FIRST_PASS

        /// <summary>
        /// Invokes the given action with the current socket, catching and mapping any resulting .NET exceptions.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="impl"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        /// <exception cref="global::java.lang.NullPointerException"></exception>
        /// <exception cref="global::java.net.SocketException"></exception>
        static void InvokeActionWithSocket(global::java.net.PlainSocketImpl impl, Action<Socket> action)
        {
            var socket = (Socket)impl.fd?.getSocket();
            if (socket == null)
                throw new global::java.net.SocketException("Socket closed.");

            action(socket);
        }

        /// <summary>
        /// Invokes the given function with the current socket, catching and mapping any resulting .NET exceptions.
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="impl"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        /// <exception cref="global::java.lang.NullPointerException"></exception>
        /// <exception cref="global::java.net.SocketException"></exception>
        static TResult InvokeFuncWithSocket<TResult>(global::java.net.PlainSocketImpl impl, Func<Socket, TResult> func)
        {
            var socket = (Socket)impl.fd?.getSocket();
            if (socket == null)
                throw new global::java.net.SocketException("Socket closed.");

            return func(socket);
        }

#endif

    }

}
