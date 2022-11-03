using System;
using System.Linq.Expressions;
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

        /// <summary>
        /// Compiles a fast getter for a <see cref="FieldInfo"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="R"></typeparam>
        /// <param name="field"></param>
        /// <returns></returns>
        static Func<T, R> MakeFieldGetter<T, R>(FieldInfo field)
        {
            var p = Expression.Parameter(typeof(T));
            var e = Expression.Lambda<Func<T, R>>(Expression.Field(Expression.ConvertChecked(p, field.DeclaringType), field), p);
            return e.Compile();
        }

        static readonly FieldInfo AbstractPlainSocketImplTrafficClassField = typeof(global::java.net.AbstractPlainSocketImpl).GetField("trafficClass", BindingFlags.NonPublic | BindingFlags.Instance);
        static readonly Func<global::java.net.AbstractPlainSocketImpl, int> AbstractPlainSocketImplTrafficClassGetter = MakeFieldGetter<global::java.net.AbstractPlainSocketImpl, int>(AbstractPlainSocketImplTrafficClassField);
        static readonly FieldInfo AbstractPlainSocketImplServerSocketField = typeof(global::java.net.AbstractPlainSocketImpl).GetField("serverSocket", BindingFlags.NonPublic | BindingFlags.Instance);
        static readonly Func<global::java.net.AbstractPlainSocketImpl, global::java.net.ServerSocket> AbstractPlainSocketImplServerSocketGetter = MakeFieldGetter<global::java.net.AbstractPlainSocketImpl, global::java.net.ServerSocket>(AbstractPlainSocketImplServerSocketField);

#endif

        static readonly byte[] PeekBuffer = new byte[1];

        /// <summary>
        /// Implements the native method for 'initProto'.
        /// </summary>
        public static void initProto()
        {

        }

        /// <summary>
        /// Implements the native method for 'socketCreate'.
        /// </summary>
        public static void socketCreate(object this_, bool isServer)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            InvokeAction<global::java.net.PlainSocketImpl>(this_, impl =>
            {
                var socket = new Socket(isServer ? SocketType.Stream : SocketType.Dgram, ProtocolType.Tcp);

                // if this is a server socket then enable SO_REUSEADDR automatically and set to non blocking
                if (AbstractPlainSocketImplServerSocketGetter(impl) != null)
                {
                    socket.Blocking = false;
                    socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                }

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
            throw new NotImplementedException();
#else
            InvokeAction<global::java.net.PlainSocketImpl, global::java.net.InetAddress>(this_, address_, (impl, address) =>
            {
                InvokeActionWithSocket(impl, socket =>
                {
                    if (Socket.OSSupportsIPv6)
                    {
                        var trafficClass = AbstractPlainSocketImplTrafficClassGetter(impl);
                        if (trafficClass != 0)
                            socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.TypeOfService, trafficClass);
                    }

                    var prevBlocking = socket.Blocking;

                    try
                    {
                        if (timeout > 0)
                        {
                            socket.Blocking = false;
                            if (socket.Poll(timeout * 1000, SelectMode.SelectWrite) == false)
                                throw new global::java.net.SocketTimeoutException("Connect timed out.");
                        }
                        else
                        {
                            socket.Blocking = true;
                        }

                        socket.Connect(address.ToIPAddress(), port);
                        impl.address = address;
                        impl.port = port;

                        if (impl.localport == 0)
                            impl.localport = ((IPEndPoint)socket.LocalEndPoint).Port;
                    }
                    finally
                    {
                        socket.Blocking = prevBlocking;
                    }
                });
            });
#endif
        }

        /// <summary>
        /// Implements the native method for 'socketBind'.
        /// </summary>
        public static void socketBind(object this_, global::java.net.InetAddress address, int port)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            InvokeAction<global::java.net.PlainSocketImpl>(this_, impl =>
            {
                InvokeActionWithSocket(impl, socket =>
                {
                    socket.Bind(new IPEndPoint(address.isAnyLocalAddress() ? IPAddress.IPv6Any : address.ToIPAddress(), port));
                    impl.address = address;
                    impl.localport = port == 0 ? ((IPEndPoint)socket.LocalEndPoint).Port : port;
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
            throw new NotImplementedException();
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
            throw new NotImplementedException();
#else
            InvokeAction<global::java.net.PlainSocketImpl, global::java.net.SocketImpl>(this_, s_, (impl, s) =>
            {
                InvokeActionWithSocket(impl, socket =>
                {
                    var prevBlocking = socket.Blocking;

                    try
                    {
                        if (impl.timeout > 0)
                        {
                            // wait for connection attempt
                            socket.Blocking = false;
                            if (socket.Poll(impl.timeout * 1000, SelectMode.SelectRead) == false)
                                throw new global::java.net.SocketTimeoutException("Accept timed out.");
                        }
                        else
                        {
                            socket.Blocking = true;
                        }

                        // accept new socket
                        var newSocket = socket.Accept();
                        if (newSocket == null)
                            throw new global::java.net.SocketException("Invalid socket.");

                        // associate new FileDescriptor with socket
                        var newfd = new global::java.io.FileDescriptor();
                        newfd.setSocket(newSocket);

                        // populate newly accepted socket
                        var remoteIpEndPoint = (IPEndPoint)newSocket.RemoteEndPoint;
                        s.fd = newfd;
                        s.address = remoteIpEndPoint.ToInetAddress();
                        s.port = remoteIpEndPoint.Port;
                        s.localport = impl.port;
                    }
                    finally
                    {
                        socket.Blocking = prevBlocking;
                    }
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
            throw new NotImplementedException();
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
            throw new NotImplementedException();
#else
            InvokeAction<global::java.net.PlainSocketImpl>(this_, impl =>
            {
                // close fails early if already closed
                if (impl.fd == null)
                    throw new global::java.net.SocketException("Socket already closed.");

                // fd still present, but socket gone, silently exit
                if (impl.fd != null && impl.fd.getSocket() == null)
                    return;

                InvokeActionWithSocket(impl, socket =>
                {
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
            throw new NotImplementedException();
#else
            InvokeAction<global::java.net.PlainSocketImpl>(this_, impl =>
            {
                if (impl.fd == null)
                    throw new global::java.net.SocketException("Socket already closed.");

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
            throw new NotImplementedException();
#else
            InvokeAction<global::java.net.PlainSocketImpl>(this_, impl =>
            {
                InvokeActionWithSocket(impl, socket =>
                {
                    if (value == null)
                        throw new global::java.lang.NullPointerException(nameof(value));

                    // .NET provides property
                    if (cmd == global::java.net.SocketOptions.SO_TIMEOUT)
                    {
                        var timeout = (global::java.lang.Integer)value;
                        socket.ReceiveTimeout = timeout.intValue();
                        return;
                    }

                    // .NET provides property
                    if (cmd == global::java.net.SocketOptions.SO_LINGER)
                    {
                        var linger = (global::java.lang.Integer)value;
                        socket.LingerState = new LingerOption(on, linger.intValue());
                        return;
                    }

                    // .NET provides a property
                    if (cmd == global::java.net.SocketOptions.SO_RCVBUF)
                    {
                        var val = (global::java.lang.Integer)value;
                        socket.ReceiveBufferSize = val.intValue();
                        return;
                    }

                    // .NET provides a property
                    if (cmd == global::java.net.SocketOptions.SO_SNDBUF)
                    {
                        var val = (global::java.lang.Integer)value;
                        socket.SendBufferSize = val.intValue();
                        return;
                    }

                    // .NET provides a property
                    if (cmd == global::java.net.SocketOptions.TCP_NODELAY)
                    {
                        socket.NoDelay = on;
                        return;
                    }

                    // lookup option options
                    if (SocketOptionUtil.TryGetDotNetSocketOption(cmd, out var options) == false)
                        throw new global::java.net.SocketException("Invalid option.");

                    // configure socket
                    socket.SetSocketOption(options.Level, options.Name, value switch
                    {
                        global::java.lang.Boolean b => GetSocketOptionSetValue(options.Type, b.booleanValue()),
                        global::java.lang.Integer i => GetSocketOptionSetValue(options.Type, i.intValue()),
                        _ => throw new global::java.net.SocketException("Invalid option value."),
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
            throw new NotImplementedException();
#else
            return InvokeFunc<global::java.net.PlainSocketImpl, global::java.net.InetAddressContainer, int>(this_, iaContainerObj_, (impl, iaContainerObj) =>
            {
                return InvokeFuncWithSocket(impl, socket =>
                {
                    // .NET provides property
                    if (opt == global::java.net.SocketOptions.SO_TIMEOUT)
                        return socket.ReceiveTimeout;

                    // .NET provides property
                    if (opt == global::java.net.SocketOptions.SO_LINGER)
                        return socket.LingerState.Enabled ? socket.LingerState.LingerTime : -1;

                    // .NET provides property
                    if (opt == global::java.net.SocketOptions.SO_RCVBUF)
                        return socket.ReceiveBufferSize;

                    // .NET provides property
                    if (opt == global::java.net.SocketOptions.SO_SNDBUF)
                        return socket.SendBufferSize;

                    // .NET provides property
                    if (opt == global::java.net.SocketOptions.SO_BINDADDR)
                    {
                        iaContainerObj.addr = ((IPEndPoint)socket.LocalEndPoint).ToInetAddress();
                        return 0;
                    }

                    // .NET provides property
                    if (opt == global::java.net.SocketOptions.TCP_NODELAY)
                        return socket.NoDelay ? 1 : -1;

                    // lookup option options
                    if (SocketOptionUtil.TryGetDotNetSocketOption(opt, out var options) == false)
                        throw new global::java.net.SocketException("Invalid option.");

                    // configure socket
                    return socket.GetSocketOption(options.Level, options.Name) switch
                    {
                        bool b => GetSocketOptionGetValue(options.Type, b),
                        int i => GetSocketOptionGetValue(options.Type, i),
                        _ => throw new global::java.net.SocketException("Invalid option value."),
                    };
                });
            });
#endif
        }

#if !FIRST_PASS

        /// <summary>
        /// Handles socket options stored as a boolean.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="global::java.net.SocketException"></exception>
        static int GetSocketOptionSetValue(SocketOptionUtil.SocketOptionType type, bool value) => type switch
        {
            SocketOptionUtil.SocketOptionType.Boolean => value ? 1 : 0,
            SocketOptionUtil.SocketOptionType.Integer => value ? 1 : 0,
            _ => throw new global::java.net.SocketException("Invalid option value."),
        };

        /// <summary>
        /// Handles socket options stored as an integer.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="global::java.net.SocketException"></exception>
        static int GetSocketOptionSetValue(SocketOptionUtil.SocketOptionType type, int value) => type switch
        {
            SocketOptionUtil.SocketOptionType.Boolean => (value != -1) ? 1 : 0,
            SocketOptionUtil.SocketOptionType.Integer => value,
            _ => throw new global::java.net.SocketException("Invalid option value."),
        };

        /// <summary>
        /// Handles socket options stored as a boolean.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="global::java.net.SocketException"></exception>
        static int GetSocketOptionGetValue(SocketOptionUtil.SocketOptionType type, bool value) => type switch
        {
            SocketOptionUtil.SocketOptionType.Boolean => value ? 1 : -1,
            _ => throw new global::java.net.SocketException("Invalid option value."),
        };

        /// <summary>
        /// Handles socket options stored as an integer.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="global::java.net.SocketException"></exception>
        static int GetSocketOptionGetValue(SocketOptionUtil.SocketOptionType type, int value) => type switch
        {
            SocketOptionUtil.SocketOptionType.Boolean => value == 0 ? -1 : 1,
            SocketOptionUtil.SocketOptionType.Integer => value,
            SocketOptionUtil.SocketOptionType.Unknown => value,
            _ => throw new global::java.net.SocketException("Invalid option value."),
        };

#endif

        /// <summary>
        /// Implements the native method for 'socketSendUrgentData'.
        /// </summary>
        public static void socketSendUrgentData(object this_, int data)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            InvokeAction<global::java.net.PlainSocketImpl>(this_, impl =>
            {
                InvokeActionWithSocket(impl, socket =>
                {
                    var prevBlocking = socket.Blocking;
                    var prevTimeout = socket.SendTimeout;

                    try
                    {
                        socket.Blocking = true;
                        socket.SendTimeout = impl.timeout;

                        var buffer = new byte[] { (byte)data };
                        socket.Send(buffer, 0, 1, SocketFlags.OutOfBand);
                    }
                    finally
                    {
                        socket.Blocking = prevBlocking;
                        socket.SendTimeout = prevTimeout;
                    }
                });
            });
#endif
        }

#if !FIRST_PASS

        /// <summary>
        /// Invokes the given action with the current socket, catching and mapping any resulting .NET exceptions.
        /// </summary>
        /// <param name="impl"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        /// <exception cref="global::java.lang.NullPointerException"></exception>
        /// <exception cref="global::java.net.SocketException"></exception>
        static void InvokeActionWithSocket(global::java.net.PlainSocketImpl impl, Action<Socket> action)
        {
            var socket = impl.fd?.getSocket();
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
            var socket = impl.fd?.getSocket();
            if (socket == null)
                throw new global::java.net.SocketException("Socket closed.");

            return func(socket);
        }

#endif

    }

}
