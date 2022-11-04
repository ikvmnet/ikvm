using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.InteropServices;

using IKVM.Runtime.Java.Externs.java.net;

using static IKVM.Java.Externs.java.net.SocketImplUtil;

namespace IKVM.Java.Externs.java.net
{

    /// <summary>
    /// Implements the external methods for <see cref="global::java.net.PlainDatagramSocketImpl"/>.
    /// </summary>
    static class PlainDatagramSocketImpl
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

        /// <summary>
        /// Compiles a fast setter for a <see cref="FieldInfo"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="field"></param>
        /// <returns></returns>
        static Action<T, V> MakeFieldSetter<T, V>(FieldInfo field)
        {
            var p = Expression.Parameter(typeof(T));
            var v = Expression.Parameter(typeof(V));
            var e = Expression.Lambda<Action<T, V>>(Expression.Assign(Expression.Field(field.DeclaringType.IsValueType ? Expression.Unbox(p, field.DeclaringType) : Expression.ConvertChecked(p, field.DeclaringType), field), v), p, v);
            return e.Compile();
        }

        /// <summary>
        /// Compiles a fast method invoker for a <see cref="MethodInfo"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TArg"></typeparam>
        /// <param name="method"></param>
        /// <returns></returns>
        static Action<T, TArg> MakeAnonymousTypeDelegate<T, TArg>(MethodInfo method)
        {
            var p = Expression.Parameter(typeof(T));
            var arg = Expression.Parameter(typeof(TArg));
            var e = Expression.Lambda<Action<T, TArg>>(Expression.Call(Expression.ConvertChecked(p, method.DeclaringType), method, arg), p, arg);
            return e.Compile();
        }

        static readonly MethodInfo InetAddressHolderMethod = typeof(global::java.net.InetAddress).GetMethod("holder", BindingFlags.NonPublic | BindingFlags.Instance);
        static readonly Func<global::java.net.InetAddress, object> InetAddressHolderDelegate = (Func<global::java.net.InetAddress, object>)InetAddressHolderMethod.CreateDelegate(typeof(Func<global::java.net.InetAddress, object>));
        static readonly FieldInfo InetAddressHolderAddressField = typeof(global::java.net.InetAddress).GetNestedType("InetAddressHolder", BindingFlags.NonPublic).GetField("address", BindingFlags.NonPublic | BindingFlags.Instance);
        static readonly Action<object, int> InetAddressHolderAddressSetter = MakeFieldSetter<object, int>(InetAddressHolderAddressField);
        static readonly FieldInfo Inet6AddressHolderField = typeof(global::java.net.Inet6Address).GetField("holder6", BindingFlags.NonPublic | BindingFlags.Instance);
        static readonly Func<global::java.net.Inet6Address, object> Inet6AddressHolderGetter = MakeFieldGetter<global::java.net.Inet6Address, object>(Inet6AddressHolderField);
        static readonly MethodInfo Inet6AddressHolderSetIpAddressMethod = typeof(global::java.net.Inet6Address).GetNestedType("Inet6AddressHolder", BindingFlags.NonPublic).GetMethod("setAddr", BindingFlags.NonPublic | BindingFlags.Instance);
        static readonly Action<object, byte[]> Inet6AddressHolderSetIpAddressDelegate = MakeAnonymousTypeDelegate<object, byte[]>(Inet6AddressHolderSetIpAddressMethod);
        static readonly FieldInfo NetworkInterfaceIndexField = typeof(global::java.net.NetworkInterface).GetField("index", BindingFlags.NonPublic | BindingFlags.Instance);
        static readonly Action<global::java.net.NetworkInterface, int> NetworkInterfaceIndexSetter = MakeFieldSetter<global::java.net.NetworkInterface, int>(NetworkInterfaceIndexField);
        static readonly FieldInfo NetworkInterfaceAddrsField = typeof(global::java.net.NetworkInterface).GetField("addrs", BindingFlags.NonPublic | BindingFlags.Instance);
        static readonly Action<global::java.net.NetworkInterface, global::java.net.InetAddress[]> NetworkInterfaceAddrsSetter = MakeFieldSetter<global::java.net.NetworkInterface, global::java.net.InetAddress[]>(NetworkInterfaceAddrsField);
        static readonly FieldInfo NetworkInterfaceNameField = typeof(global::java.net.NetworkInterface).GetField("name", BindingFlags.NonPublic | BindingFlags.Instance);
        static readonly Action<global::java.net.NetworkInterface, string> NetworkInterfaceNameSetter = MakeFieldSetter<global::java.net.NetworkInterface, string>(NetworkInterfaceNameField);
        static readonly FieldInfo AbstractPlainDatagramSocketImplTrafficClassField = typeof(global::java.net.AbstractPlainDatagramSocketImpl).GetField("trafficClass", BindingFlags.NonPublic | BindingFlags.Instance);
        static readonly Func<global::java.net.AbstractPlainDatagramSocketImpl, int> AbstractPlainDatagramSocketImplTrafficClassGetter = MakeFieldGetter<global::java.net.AbstractPlainDatagramSocketImpl, int>(AbstractPlainDatagramSocketImplTrafficClassField);

#endif

        const IOControlCode SIO_UDP_CONNRESET = (IOControlCode)(-1744830452);
        static readonly byte[] IOControlTrueBuffer = BitConverter.GetBytes(1);
        static readonly byte[] IOControlFalseBuffer = BitConverter.GetBytes(0);
        static readonly byte[] TempBuffer = new byte[1];

        /// <summary>
        /// Implements the native method for 'init'.
        /// </summary>
        public static void init()
        {

        }

        /// <summary>
        /// Implements the native method for 'datagramSocketCreate'.
        /// </summary>
        /// <param name="this_"></param>
        /// <exception cref="global::java.net.SocketException"></exception>
        public static void datagramSocketCreate(object this_)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            InvokeAction<global::java.net.PlainDatagramSocketImpl>(this_, impl =>
            {
                var socket = new Socket(SocketType.Dgram, ProtocolType.Udp);

                // enable broadcast
                socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, true);

                // SIO_UDP_CONNRESET fixes a "bug" introduced in Windows 2000, which
                // returns connection reset errors on unconnected UDP sockets (as well
                // as connected sockets). The solution is to only enable this feature
                // when the socket is connected.
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    socket.IOControl(SIO_UDP_CONNRESET, IOControlFalseBuffer, null);

                impl.fd.setSocket(socket);
            });
#endif
        }

        /// <summary>
        /// Implements the native method for 'datagramSocketClose'.
        /// </summary>
        /// <param name="this_"></param>
        /// <exception cref="global::java.lang.NullPointerException"></exception>
        /// <exception cref="global::java.net.SocketException"></exception>
        public static void datagramSocketClose(object this_)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            InvokeAction<global::java.net.PlainDatagramSocketImpl>(this_, (impl) =>
            {
                var socket = impl.fd?.getSocket();
                if (socket == null)
                    return;

                InvokeWithSocket(impl, socket =>
                {
                    socket.Close();
                    impl.fd.setSocket(null);
                });
            });
#endif
        }

        /// <summary>
        /// Implements the native method for 'connect0'.
        /// </summary>
        /// <param name="this_"></param>
        /// <param name="address_"></param>
        /// <param name="port"></param>
        /// <exception cref="global::java.lang.NullPointerException"></exception>
        /// <exception cref="global::java.net.SocketException"></exception>
        public static void connect0(object this_, object address_, int port)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            InvokeAction<global::java.net.PlainDatagramSocketImpl, global::java.net.InetAddress>(this_, address_, (impl, address) =>
            {
                InvokeWithSocket(impl, socket =>
                {
                    if (address == null)
                        throw new global::java.lang.NullPointerException(nameof(address));

                    socket.Connect(address.ToIPAddress(), port);

                    // see comment in in socketCreate
                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                        socket.IOControl(SIO_UDP_CONNRESET, IOControlTrueBuffer, null);
                });
            });
#endif
        }

        /// <summary>
        /// Implements the native method for 'disconnect0'.
        /// </summary>
        /// <param name="this_"></param>
        /// <param name="family"></param>
        /// <exception cref="global::java.lang.NullPointerException"></exception>
        /// <exception cref="global::java.net.SocketException"></exception>
        public static void disconnect0(object this_, int family)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            InvokeAction<global::java.net.PlainDatagramSocketImpl>(this_, impl =>
            {
                if (impl.fd == null || impl.fd.getSocket() == null)
                    return;

                InvokeWithSocket(impl, socket =>
                {
                    // for a UDP socket, disconnect is just connecting to the any address
                    socket.Connect(new IPEndPoint(IPAddress.IPv6Any, 0));

                    // see comment in in socketCreate
                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                        socket.IOControl(SIO_UDP_CONNRESET, IOControlFalseBuffer, null);
                });
            });
#endif
        }

        /// <summary>
        /// Implements the native method for 'bind0'.
        /// </summary>
        /// <param name="this_"></param>
        /// <param name="localPort"></param>
        /// <param name="localAddress_"></param>
        /// <exception cref="global::java.lang.NullPointerException"></exception>
        /// <exception cref="global::java.net.SocketException"></exception>
        public static void bind0(object this_, int localPort, object localAddress_)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            InvokeAction<global::java.net.PlainDatagramSocketImpl, global::java.net.InetAddress>(this_, localAddress_, (impl, localAddress) =>
            {
                InvokeWithSocket(impl, socket =>
                {
                    if (localAddress == null)
                        throw new global::java.lang.NullPointerException(nameof(localAddress));

                    // bind to appropriate address
                    socket.Bind(new IPEndPoint(localAddress.isAnyLocalAddress() ? IPAddress.IPv6Any : localAddress.ToIPAddress(), localPort));

                    // check that we bound to an IP endpoint
                    var localEndpoint = socket.LocalEndPoint;
                    if (localEndpoint is not IPEndPoint ipLocalEndpoint)
                        throw new global::java.net.SocketException("Unexpected resulting endpoint type.");

                    // set localport
                    impl.localPort = ipLocalEndpoint.Port;
                });
            });
#endif
        }

        /// <summary>
        /// Implements the native method for 'send'.
        /// </summary>
        /// <param name="this_"></param>
        /// <param name="packet_"></param>
        /// <exception cref="global::java.lang.NullPointerException"></exception>
        /// <exception cref="global::java.net.SocketException"></exception>
        public static void send(object this_, object packet_)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            InvokeAction<global::java.net.PlainDatagramSocketImpl, global::java.net.DatagramPacket>(this_, packet_, (impl, packet) =>
            {
                InvokeWithSocket(impl, socket =>
                {
                    if (packet == null)
                        throw new global::java.lang.NullPointerException(nameof(packet));

                    if (Socket.OSSupportsIPv6)
                    {
                        var trafficClass = AbstractPlainDatagramSocketImplTrafficClassGetter(impl);
                        if (trafficClass != 0)
                            socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.TypeOfService, trafficClass);
                    }

                    var prevBlocking = socket.Blocking;
                    var prevSendTimeout = socket.SendTimeout;

                    try
                    {
                        socket.Blocking = true;
                        socket.SendTimeout = 0;
                        socket.SendTo(packet.buf, packet.offset, packet.length, SocketFlags.None, new IPEndPoint(packet.getAddress().ToIPAddress(), packet.getPort()));
                    }
                    finally
                    {
                        socket.Blocking = prevBlocking;
                        socket.SendTimeout = prevSendTimeout;
                    }
                });
            });
#endif
        }

        /// <summary>
        /// Implements the native method for 'dataAvailable'.
        /// </summary>
        /// <param name="this_"></param>
        /// <returns></returns>
        /// <exception cref="global::java.lang.NullPointerException"></exception>
        /// <exception cref="global::java.net.SocketException"></exception>
        public static int dataAvailable(object this_)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            return InvokeFunc<global::java.net.PlainDatagramSocketImpl, int>(this_, impl =>
            {
                return InvokeWithSocket(impl, socket =>
                {
                    return socket.Available;
                });
            });
#endif
        }

        /// <summary>
        /// Implements the native method for 'peek'.
        /// </summary>
        /// <param name="this_"></param>
        /// <param name="address_"></param>
        /// <returns></returns>
        /// <exception cref="global::java.lang.NullPointerException"></exception>
        /// <exception cref="global::java.net.SocketException"></exception>
        public static int peek(object this_, object address_)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            return InvokeFunc<global::java.net.PlainDatagramSocketImpl, global::java.net.InetAddress, int>(this_, address_, (impl, address) =>
            {
                return InvokeWithSocket(impl, socket =>
                {
                    if (address == null)
                        throw new global::java.lang.NullPointerException(nameof(address));

                    var prevBlocking = socket.Blocking;
                    var prevReceiveTimeout = socket.ReceiveTimeout;

                    try
                    {
                        if (impl.timeout > 0)
                        {
                            // wait for data to be available
                            socket.Blocking = false;
                            socket.ReceiveTimeout = impl.timeout;
                            if (socket.Poll(impl.timeout * 1000, SelectMode.SelectRead) == false)
                                throw new global::java.net.SocketTimeoutException("Peek timed out.");
                        }
                        else
                        {
                            socket.Blocking = true;
                            socket.ReceiveTimeout = 0;
                        }

                        var remoteEndpoint = (EndPoint)new IPEndPoint(IPAddress.IPv6Any, 0);
                        var length = socket.ReceiveFrom(TempBuffer, 0, 1, SocketFlags.Peek, ref remoteEndpoint);

                        // check that we received an IP endpoint
                        if (remoteEndpoint is not IPEndPoint ipRemoteEndpoint)
                            throw new global::java.net.SocketException("Unexpected resulting endpoint type.");

                        if (ipRemoteEndpoint.AddressFamily == AddressFamily.InterNetworkV6)
                        {
                            if (address is global::java.net.Inet6Address address6)
                            {
                                // InetAddress stores its data on a nested 'holder' instance
                                var holder = Inet6AddressHolderGetter(address6);
                                if (holder == null)
                                    throw new global::java.net.SocketException("Could not get Inet6Address holder.");

                                // set the address directly onto the holder
                                var bytes = ipRemoteEndpoint.Address.GetAddressBytes();
                                Inet6AddressHolderSetIpAddressDelegate(holder, bytes);
                            }
                            else if (ipRemoteEndpoint.Address.IsIPv4MappedToIPv6)
                            {
                                // InetAddress stores its data on a nested 'holder' instance
                                var holder = InetAddressHolderDelegate(address);
                                if (holder == null)
                                    throw new global::java.net.SocketException("Could not get InetAddress holder.");

                                // set the address directly onto the holder
                                var bytes = BinaryPrimitives.ReadInt32BigEndian(ipRemoteEndpoint.Address.MapToIPv4().GetAddressBytes());
                                InetAddressHolderAddressSetter(holder, bytes);
                            }
                        }
                        else if (ipRemoteEndpoint.AddressFamily == AddressFamily.InterNetwork)
                        {
                            if (address is global::java.net.Inet6Address address6)
                            {
                                // InetAddress stores its data on a nested 'holder' instance
                                var holder = Inet6AddressHolderGetter(address6);
                                if (holder == null)
                                    throw new global::java.net.SocketException("Could not get Inet6Address holder.");

                                // set the address directly onto the holder
                                var bytes = ipRemoteEndpoint.Address.MapToIPv6().GetAddressBytes();
                                Inet6AddressHolderSetIpAddressDelegate(holder, bytes);
                            }
                            else
                            {
                                // InetAddress stores its data on a nested 'holder' instance
                                var holder = InetAddressHolderDelegate(address);
                                if (holder == null)
                                    throw new global::java.net.SocketException("Could not get InetAddress holder.");

                                // set the address directly onto the holder
                                var bytes = BinaryPrimitives.ReadInt32BigEndian(ipRemoteEndpoint.Address.GetAddressBytes());
                                InetAddressHolderAddressSetter(holder, bytes);
                            }
                        }

                        return ipRemoteEndpoint.Port;
                    }
                    finally
                    {
                        socket.Blocking = prevBlocking;
                        socket.ReceiveTimeout = prevReceiveTimeout;
                    }
                });
            });
#endif
        }

        /// <summary>
        /// Peek at the queue to see if there is an ICMP port unreachable. If there is, then receive it.
        /// </summary>
        /// <param name="socket"></param>
        static void PurgeOutstandingICMP(Socket socket)
        {
            while (true)
            {
                // check for outstanding packet
                if (socket.Poll(0, SelectMode.SelectRead) == false)
                    break;

                try
                {
                    var ep = (EndPoint)new IPEndPoint(IPAddress.IPv6Any, 0);
                    socket.ReceiveFrom(TempBuffer, SocketFlags.Peek, ref ep);
                }
                catch (SocketException e) when (e.SocketErrorCode == SocketError.ConnectionReset)
                {
                    try
                    {
                        var ep = (EndPoint)new IPEndPoint(IPAddress.IPv6Any, 0);
                        socket.ReceiveFrom(TempBuffer, SocketFlags.None, ref ep);
                    }
                    catch (SocketException e2) when (e2.SocketErrorCode == SocketError.ConnectionReset)
                    {

                    }

                    continue;
                }

                break;
            }
        }

        /// <summary>
        /// Implements the native method for 'peekData'.
        /// </summary>
        /// <param name="this_"></param>
        /// <param name="packet_"></param>
        /// <returns></returns>
        /// <exception cref="global::java.lang.NullPointerException"></exception>
        /// <exception cref="global::java.net.SocketException"></exception>
        public static int peekData(object this_, object packet_)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            return InvokeFunc<global::java.net.PlainDatagramSocketImpl, global::java.net.DatagramPacket, int>(this_, packet_, (impl, packet) =>
            {
                return InvokeWithSocket<int>(impl, socket =>
                {
                    if (packet == null)
                        throw new global::java.lang.NullPointerException("packet");
                    if (packet.buf == null)
                        throw new global::java.lang.NullPointerException("packet buffer");

                    var prevBlocking = socket.Blocking;
                    var prevReceiveTimeout = socket.ReceiveTimeout;

                    try
                    {
                        var remoteEndpoint = (EndPoint)new IPEndPoint(IPAddress.IPv6Any, 0);
                        var length = 0;

                        if (impl.timeout > 0)
                        {
                            // wait for data to be available
                            socket.Blocking = false;
                            socket.ReceiveTimeout = impl.timeout;
                            if (socket.Poll(impl.timeout * 1000, SelectMode.SelectRead) == false)
                                throw new global::java.net.SocketTimeoutException("Peek data timed out.");
                        }
                        else
                        {
                            socket.Blocking = true;
                            socket.ReceiveTimeout = 0;
                        }

                        try
                        {
                            length = socket.ReceiveFrom(packet.buf, packet.offset, packet.bufLength, SocketFlags.Peek, ref remoteEndpoint);
                        }
                        catch (SocketException e) when (e.SocketErrorCode == SocketError.MessageSize)
                        {
                            // message size error indicates we did read something, but not the whole message
                            // just return what we did read, since it's UDP and nobody cares anyways
                            length = packet.bufLength;
                        }
                        catch (SocketException e) when (e.SocketErrorCode == SocketError.ConnectionReset)
                        {
                            // Windows might leave more of these around, clear them all
                            PurgeOutstandingICMP(socket);
                            throw new global::java.net.PortUnreachableException("ICMP Port Unreachable");
                        }
                        catch
                        {
                            // some other exception, report no data read
                            packet.port = 0;
                            packet.length = 0;
                            throw;
                        }

                        // check that we received an IP endpoint
                        if (remoteEndpoint is not IPEndPoint ipRemoteEndpoint)
                            throw new global::java.net.SocketException("Unexpected resulting endpoint type.");

                        var remoteAddress = ipRemoteEndpoint.ToInetAddress();
                        var packetAddress = packet.address;
                        if (packetAddress == null || packetAddress.equals(remoteAddress) == false)
                            packet.address = ipRemoteEndpoint.ToInetAddress();

                        packet.port = ipRemoteEndpoint.Port;
                        packet.length = length;
                        return ipRemoteEndpoint.Port;
                    }
                    finally
                    {
                        socket.Blocking = prevBlocking;
                        socket.ReceiveTimeout = prevReceiveTimeout;
                    }
                });
            });
#endif
        }

        /// <summary>
        /// Implements the native method for 'receive0'.
        /// </summary>
        /// <param name="this_"></param>
        /// <param name="packet_"></param>
        /// <exception cref="global::java.lang.NullPointerException"></exception>
        /// <exception cref="global::java.net.SocketException"></exception>
        public static void receive0(object this_, object packet_)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            InvokeAction<global::java.net.PlainDatagramSocketImpl, global::java.net.DatagramPacket>(this_, packet_, (impl, packet) =>
            {
                InvokeWithSocket(impl, socket =>
                {
                    if (packet == null)
                        throw new global::java.lang.NullPointerException("packet");
                    if (packet.buf == null)
                        throw new global::java.lang.NullPointerException("packet buffer");

                    var prevBlocking = socket.Blocking;
                    var prevReceiveTimeout = socket.ReceiveTimeout;

                    try
                    {
                        var remoteEndpoint = (EndPoint)new IPEndPoint(IPAddress.IPv6Any, 0);
                        var length = 0;

                        if (impl.timeout > 0)
                        {
                            // wait for data to be available
                            socket.Blocking = false;
                            socket.ReceiveTimeout = impl.timeout;
                            if (socket.Poll(impl.timeout * 1000, SelectMode.SelectRead) == false)
                                throw new global::java.net.SocketTimeoutException("Receive timed out.");
                        }
                        else
                        {
                            socket.Blocking = true;
                            socket.ReceiveTimeout = 0;
                        }

                        try
                        {
                            length = socket.ReceiveFrom(packet.buf, packet.offset, packet.bufLength, SocketFlags.None, ref remoteEndpoint);
                        }
                        catch (SocketException e) when (e.SocketErrorCode == SocketError.MessageSize)
                        {
                            // message size error indicates we did read something, but not the whole message
                            // just return what we did read, since it's UDP and nobody cares anyways
                            length = packet.bufLength;
                        }
                        catch (SocketException e) when (e.SocketErrorCode == SocketError.ConnectionReset)
                        {
                            // Windows might leave more of these around, clear them all
                            PurgeOutstandingICMP(socket);
                            throw new global::java.net.PortUnreachableException("ICMP Port Unreachable");
                        }
                        catch
                        {
                            // some other exception, report no data read
                            packet.offset = 0;
                            packet.length = 0;
                            throw;
                        }

                        // check that we received an IP endpoint
                        if (remoteEndpoint is not IPEndPoint ipRemoteEndpoint)
                            throw new global::java.net.SocketException("Unexpected resulting endpoint type.");

                        var remoteAddress = ipRemoteEndpoint.ToInetAddress();
                        var packetAddress = packet.address;
                        if (packetAddress == null || packetAddress.equals(remoteAddress) == false)
                            packet.address = ipRemoteEndpoint.ToInetAddress();

                        packet.port = ipRemoteEndpoint.Port;
                        packet.length = length;
                        return ipRemoteEndpoint.Port;
                    }
                    finally
                    {
                        socket.Blocking = prevBlocking;
                        socket.ReceiveTimeout = prevReceiveTimeout;
                    }
                });
            });
#endif
        }

        /// <summary>
        /// Implements the native method for 'setTimeToLive'.
        /// </summary>
        /// <param name="this_"></param>
        /// <param name="ttl"></param>
        /// <exception cref="global::java.lang.NullPointerException"></exception>
        /// <exception cref="global::java.net.SocketException"></exception>
        public static void setTimeToLive(object this_, int ttl)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            InvokeAction<global::java.net.PlainDatagramSocketImpl>(this_, (impl) =>
            {
                InvokeWithSocket(impl, socket =>
                {
                    socket.Ttl = (short)ttl;
                });
            });
#endif
        }

        /// <summary>
        /// Implements the native method for 'getTimeToLive'.
        /// </summary>
        /// <param name="this_"></param>
        /// <returns></returns>
        /// <exception cref="global::java.lang.NullPointerException"></exception>
        /// <exception cref="global::java.net.SocketException"></exception>
        public static int getTimeToLive(object this_)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            return InvokeFunc<global::java.net.PlainDatagramSocketImpl, int>(this_, (impl) =>
            {
                return InvokeWithSocket(impl, socket =>
                {
                    return (int)socket.Ttl;
                });
            });
#endif
        }

        /// <summary>
        /// Implements the native method for 'setTTL'.
        /// </summary>
        /// <param name="this_"></param>
        /// <param name="ttl"></param>
        /// <exception cref="global::java.lang.NullPointerException"></exception>
        /// <exception cref="global::java.net.SocketException"></exception>
        public static void setTTL(object this_, byte ttl)
        {
            setTimeToLive(this_, ttl);
        }

        /// <summary>
        /// Implements the native method for 'getTTL'.
        /// </summary>
        /// <param name="this_"></param>
        /// <returns></returns>
        /// <exception cref="global::java.lang.NullPointerException"></exception>
        /// <exception cref="global::java.net.SocketException"></exception>
        public static byte getTTL(object this_)
        {
            return (byte)getTimeToLive(this_);
        }

#if !FIRST_PASS

        /// <summary>
        /// Implements the backend for 'join' and 'leave' methods.
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="inetaddr"></param>
        /// <param name="netIf"></param>
        /// <param name="action"></param>
        /// <exception cref="global::java.lang.NullPointerException"></exception>
        /// <exception cref="global::java.net.SocketException"></exception>
        static void JoinOrLeave(Socket socket, global::java.net.InetAddress inetaddr, global::java.net.NetworkInterface netIf, SocketOptionName action)
        {
            if (inetaddr == null)
                throw new global::java.lang.NullPointerException(nameof(inetaddr));

            var addr = inetaddr.ToIPAddress();
            var ipv6 = Socket.OSSupportsIPv6 && addr.AddressFamily == AddressFamily.InterNetworkV6;

            // attempt to join IPv6 multicast
            if (ipv6)
            {
                var o = new IPv6MulticastOption(addr);
                if (netIf != null)
                    o.InterfaceIndex = netIf.getIndex();

                try
                {
                    socket.SetSocketOption(SocketOptionLevel.IPv6, action, o);
                }
                catch (SocketException e) when (e.SocketErrorCode == SocketError.ProtocolNotSupported)
                {
                    ipv6 = false; // fallback to IPv4
                }
            }

            if (ipv6 == false)
            {
                var o = new MulticastOption(addr);
                if (netIf != null)
                    o.InterfaceIndex = netIf.getIndex();

                socket.SetSocketOption(SocketOptionLevel.IP, action, o);
            }
        }

#endif

        /// <summary>
        /// Implements the native method for 'join'.
        /// </summary>
        /// <param name="this_"></param>
        /// <param name="inetaddr_"></param>
        /// <param name="netIf_"></param>
        /// <exception cref="global::java.lang.NullPointerException"></exception>
        /// <exception cref="global::java.net.SocketException"></exception>
        public static void join(object this_, object inetaddr_, object netIf_)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            InvokeAction<global::java.net.PlainDatagramSocketImpl, global::java.net.InetAddress, global::java.net.NetworkInterface>(this_, inetaddr_, netIf_, (impl, inetaddr, netIf) =>
            {
                InvokeWithSocket(impl, socket =>
                {
                    JoinOrLeave(socket, inetaddr, netIf, SocketOptionName.AddMembership);
                });
            });
#endif
        }

        /// <summary>
        /// Implements the native method for 'leave'.
        /// </summary>
        /// <param name="this_"></param>
        /// <param name="inetaddr_"></param>
        /// <param name="netIf_"></param>
        /// <exception cref="global::java.lang.NullPointerException"></exception>
        /// <exception cref="global::java.net.SocketException"></exception>
        public static void leave(object this_, object inetaddr_, object netIf_)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            InvokeAction<global::java.net.PlainDatagramSocketImpl, global::java.net.InetAddress, global::java.net.NetworkInterface>(this_, inetaddr_, netIf_, (impl, inetaddr, netIf) =>
            {
                InvokeWithSocket(impl, socket =>
                {
                    JoinOrLeave(socket, inetaddr, netIf, SocketOptionName.DropMembership);
                });
            });
#endif
        }

#if !FIRST_PASS

        /// <summary>
        /// Implements the getter of the socket options IP_MULTICAST_IF and IP_MULTICAST_IF2.
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        /// <exception cref="global::java.lang.NullPointerException"></exception>
        /// <exception cref="global::java.net.SocketException"></exception>
        static object GetMulticastInterfaceOption(Socket socket, int option)
        {
            if (socket is null)
                throw new ArgumentNullException(nameof(socket));

            var index = (int)socket.GetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.MulticastInterface);
            if (index < 0)
                throw new global::java.net.SocketException("Error getting socket option.");

            // index of socket is set
            if (index > 0)
            {
                // find network interface with the index
                var iface = global::java.net.NetworkInterface.getByIndex(index);
                if (iface == null)
                    throw new global::java.net.SocketException($"IPV6_MULTICAST_IF returned index to unrecognized interface: {index}.");

                // IP_MULTICAST_IF2 wants the NetworkInterface directly
                if (option == global::java.net.SocketOptions.IP_MULTICAST_IF2)
                    return iface;

                // otherwise collect the InetAddresses and return the first entry
                var addresses = new List<global::java.net.InetAddress>();
                for (var e = iface.getInetAddresses(); e.hasMoreElements();)
                    addresses.Add((global::java.net.InetAddress)e.nextElement());
                if (addresses.Count == 0)
                    throw new global::java.net.SocketException("IPV6_MULTICAST_IF returned interface without IP bindings.");

                return addresses[0];
            }

            var addr = global::java.net.InetAddress.anyLocalAddress();
            if (option == global::java.net.SocketOptions.IP_MULTICAST_IF)
                return addr;

            // return a new anonymous network interface with index -1 and empty address array
            var anon = new global::java.net.NetworkInterface();
            NetworkInterfaceIndexSetter(anon, -1);
            NetworkInterfaceAddrsSetter(anon, Array.Empty<global::java.net.InetAddress>());
            NetworkInterfaceNameSetter(anon, "");
            return anon;
        }

#endif

        /// <summary>
        /// Implements the native method for 'socketGetOption'.
        /// </summary>
        /// <param name="this_"></param>
        /// <param name="opt"></param>
        /// <returns></returns>
        /// <exception cref="global::java.lang.NullPointerException"></exception>
        /// <exception cref="global::java.net.SocketException"></exception>
        public static object socketGetOption(object this_, int opt)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            return InvokeFunc<global::java.net.PlainDatagramSocketImpl, object>(this_, impl =>
            {
                return InvokeWithSocket(impl, socket =>
                {
                    // IP_MULTICAST_IF returns an InetAddress while IP_MULTICAST_IF2 returns a NetworkInterface
                    if (opt == global::java.net.SocketOptions.IP_MULTICAST_IF ||
                        opt == global::java.net.SocketOptions.IP_MULTICAST_IF2)
                        return GetMulticastInterfaceOption(socket, opt);

                    // .NET provides property
                    if (opt == global::java.net.SocketOptions.SO_BINDADDR)
                        return ((IPEndPoint)socket.LocalEndPoint).ToInetAddress();

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
        /// Implements the setter of the socket options IP_MULTICAST_IF and IP_MULTICAST_IF2.
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="option"></param>
        /// <param name="value"></param>
        /// <exception cref="global::java.lang.NullPointerException"></exception>
        /// <exception cref="global::java.net.SocketException"></exception>
        static void SetMulticastInterface(Socket socket, int option, object value)
        {
            if (socket is null)
                throw new ArgumentNullException(nameof(socket));

            // IP_MULTICAST_IF accepts an InetAddress, which must be converted to an interface index
            if (option == global::java.net.SocketOptions.IP_MULTICAST_IF)
            {
                if (value is not global::java.net.InetAddress inetAddr)
                    throw new global::java.net.SocketException("Bad argument for IP_MULTICAST_IF: value is not an address");

                // find interface that matches address
                var iface = global::java.net.NetworkInterface.getByInetAddress(inetAddr);
                if (iface == null)
                    throw new global::java.net.SocketException("Bad argument for IP_MULTICAST_IF: address not bound to any interface.");

                if (Socket.OSSupportsIPv6)
                    socket.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.MulticastInterface, iface.getIndex());
                else
                    socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastInterface, iface.getIndex());

                return;
            }

            // IP_MULTICAST_IF accepts a NetworkInterface, which must be converted to an interface index
            if (option == global::java.net.SocketOptions.IP_MULTICAST_IF2)
            {
                if (value is not global::java.net.NetworkInterface ni)
                    throw new global::java.net.SocketException("Bad argument for IP_MULTICAST_IF2: value is not an interface");

                if (Socket.OSSupportsIPv6)
                    socket.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.MulticastInterface, ni.getIndex());
                else
                    socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastInterface, ni.getIndex());

                return;
            }

            return;
        }

#endif

        /// <summary>
        /// Implements the native method for 'socketSetOption'.
        /// </summary>
        /// <param name="this_"></param>
        /// <param name="opt"></param>
        /// <param name="value"></param>
        /// <exception cref="global::java.lang.NullPointerException"></exception>
        /// <exception cref="global::java.net.SocketException"></exception>
        public static void socketSetOption0(object this_, int opt, object value)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            InvokeAction<global::java.net.PlainDatagramSocketImpl>(this_, impl =>
            {
                InvokeWithSocket(impl, socket =>
                {
                    if (value == null)
                        throw new global::java.lang.NullPointerException(nameof(value));

                    // multicast options may receive InetAddress or NetworkInterface
                    if (opt == global::java.net.SocketOptions.IP_MULTICAST_IF ||
                        opt == global::java.net.SocketOptions.IP_MULTICAST_IF2)
                    {
                        SetMulticastInterface(socket, opt, value);
                        return;
                    }

                    // implementation changes based on IP/IPv6
                    if (opt == global::java.net.SocketOptions.IP_MULTICAST_LOOP)
                    {
                        var val = (global::java.lang.Boolean)value;
                        socket.MulticastLoopback = val.booleanValue();
                        return;
                    }
                    // .NET provides property
                    if (opt == global::java.net.SocketOptions.SO_TIMEOUT)
                    {
                        var val = (global::java.lang.Integer)value;
                        socket.ReceiveTimeout = val.intValue();
                        return;
                    }

                    // .NET provides a property
                    if (opt == global::java.net.SocketOptions.SO_RCVBUF)
                    {
                        var val = (global::java.lang.Integer)value;
                        socket.ReceiveBufferSize = val.intValue();
                        return;
                    }

                    // .NET provides a property
                    if (opt == global::java.net.SocketOptions.SO_SNDBUF)
                    {
                        var val = (global::java.lang.Integer)value;
                        socket.SendBufferSize = val.intValue();
                        return;
                    }

                    // .NET provides a property
                    if (opt == global::java.net.SocketOptions.TCP_NODELAY)
                    {
                        var val = (global::java.lang.Integer)value;
                        socket.NoDelay = val.intValue() != -1;
                        return;
                    }

                    // lookup option options
                    if (SocketOptionUtil.TryGetDotNetSocketOption(opt, out var options) == false)
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
        static object GetSocketOptionGetValue(SocketOptionUtil.SocketOptionType type, bool value) => type switch
        {
            SocketOptionUtil.SocketOptionType.Boolean => new global::java.lang.Boolean(value),
            _ => throw new global::java.net.SocketException("Invalid option value."),
        };

        /// <summary>
        /// Handles socket options stored as an integer.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="global::java.net.SocketException"></exception>
        static object GetSocketOptionGetValue(SocketOptionUtil.SocketOptionType type, int value) => type switch
        {
            SocketOptionUtil.SocketOptionType.Boolean => value == 0 ? global::java.lang.Boolean.FALSE : global::java.lang.Boolean.TRUE,
            SocketOptionUtil.SocketOptionType.Integer => new global::java.lang.Integer(value),
            SocketOptionUtil.SocketOptionType.Unknown => value,
            _ => throw new global::java.net.SocketException("Invalid option value."),
        };

#endif

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
        static void InvokeWithSocket(global::java.net.PlainDatagramSocketImpl impl, Action<Socket> action)
        {
            var socket = impl.fd?.getSocket();
            if (socket == null)
                throw new global::java.net.SocketException("Socket closed");

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
        static TResult InvokeWithSocket<TResult>(global::java.net.PlainDatagramSocketImpl impl, Func<Socket, TResult> func)
        {
            var socket = impl.fd?.getSocket();
            if (socket == null)
                throw new global::java.net.SocketException("Socket closed");

            return func(socket);
        }

#endif

    }

}
