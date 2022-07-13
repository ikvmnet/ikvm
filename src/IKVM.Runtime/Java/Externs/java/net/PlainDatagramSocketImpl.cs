using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Reflection;

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

        static readonly byte[] PeekBuffer = new byte[1];
        static readonly MethodInfo InetAddressHolderMethod = typeof(global::java.net.InetAddress).GetMethod("holder", BindingFlags.NonPublic | BindingFlags.Instance);
        static readonly FieldInfo InetAddressHolderAddressField = typeof(global::java.net.InetAddress).GetNestedType("InetAddressHolder", BindingFlags.NonPublic).GetField("address", BindingFlags.NonPublic);
        static readonly MethodInfo Inet6AddressHolderMethod = typeof(global::java.net.Inet6Address).GetMethod("holder6", BindingFlags.NonPublic | BindingFlags.Instance);
        static readonly MethodInfo Inet6AddressHolderSetIpAddressMethod = typeof(global::java.net.Inet6Address).GetNestedType("Inet6AddressHolder", BindingFlags.NonPublic).GetMethod("setAddr", BindingFlags.NonPublic);
        static readonly FieldInfo NetworkInterfaceIndexField = typeof(global::java.net.NetworkInterface).GetField("index", BindingFlags.NonPublic | BindingFlags.Instance);
        static readonly FieldInfo NetworkInterfaceAddrsField = typeof(global::java.net.NetworkInterface).GetField("addrs", BindingFlags.NonPublic | BindingFlags.Instance);
        static readonly FieldInfo NetworkInterfaceNameField = typeof(global::java.net.NetworkInterface).GetField("name", BindingFlags.NonPublic | BindingFlags.Instance);

#endif

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
            throw new NotSupportedException();
#else
            SocketInvokeFunc<global::java.net.PlainDatagramSocketImpl>(this_, impl =>
            {
                var socket = new Socket(SocketType.Dgram, ProtocolType.Udp);
                if (Socket.OSSupportsIPv6)
                    socket.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.IPv6Only, false);
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
            throw new NotSupportedException();
#else
            SocketInvokeFunc<global::java.net.PlainDatagramSocketImpl>(this_, (impl) =>
            {
                var socket = (Socket)impl.fd?.getSocket();
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
            throw new NotSupportedException();
#else
            SafeInvoke<global::java.net.PlainDatagramSocketImpl, global::java.net.InetAddress>(this_, localAddress_, (impl, localAddress) =>
            {
                InvokeWithSocket(impl, socket =>
                {
                    if (localAddress == null)
                        throw new global::java.lang.NullPointerException(nameof(localAddress));

                    socket.Bind(new IPEndPoint(localAddress.ToIPAddress(), localPort));

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
            throw new NotSupportedException();
#else
            SafeInvoke<global::java.net.PlainDatagramSocketImpl, global::java.net.DatagramPacket>(this_, packet_, (impl, packet) =>
            {
                InvokeWithSocket(impl, socket =>
                {
                    if (packet == null)
                        throw new global::java.lang.NullPointerException(nameof(packet));

                    socket.SendTimeout = impl.timeout;
                    var result = socket.BeginSendTo(packet.getData(), packet.getOffset(), packet.getLength(), SocketFlags.None, new IPEndPoint(packet.getAddress().ToIPAddress(), packet.getPort()), null, null);
                    if (impl.timeout > 0 && result.AsyncWaitHandle.WaitOne(impl.timeout, true) == false)
                        throw new global::java.net.SocketTimeoutException("Send timed out.");

                    var n = socket.EndSendTo(result);
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
            throw new NotSupportedException();
#else
            return SafeInvoke<global::java.net.PlainDatagramSocketImpl, global::java.net.InetAddress, int>(this_, address_, (impl, address) =>
            {
                return InvokeWithSocket(impl, socket =>
                {
                    if (address == null)
                        throw new global::java.lang.NullPointerException(nameof(address));

                    // use asynchronous completion to allow cancelation through Close()
                    var remoteEndpoint = (EndPoint)new IPEndPoint(IPAddress.Any, 0);
                    socket.ReceiveTimeout = impl.timeout;
                    var result = socket.BeginReceiveFrom(PeekBuffer, 0, 1, SocketFlags.Peek, ref remoteEndpoint, null, null);
                    if (impl.timeout > 0 && result.AsyncWaitHandle.WaitOne(impl.timeout, true) == false)
                        throw new global::java.net.SocketTimeoutException("Peek data timed out.");

                    var n = socket.EndReceiveFrom(result, ref remoteEndpoint);

                    // check that we received an IP endpoint
                    if (remoteEndpoint is not IPEndPoint ipRemoteEndpoint)
                        throw new global::java.net.SocketException("Unexpected resulting endpoint type.");

                    if (ipRemoteEndpoint.AddressFamily == AddressFamily.InterNetworkV6)
                    {
                        if (address is global::java.net.Inet6Address)
                        {
                            // InetAddress stores its data on a nested 'holder' instance
                            var holder = Inet6AddressHolderMethod.Invoke(address, Array.Empty<object>());
                            if (holder == null)
                                throw new global::java.net.SocketException("Could not get Inet6Address holder.");

                            // set the address directly onto the holder
                            var bytes = ipRemoteEndpoint.Address.GetAddressBytes();
                            Inet6AddressHolderSetIpAddressMethod.Invoke(holder, new[] { bytes });
                        }
                        else if (ipRemoteEndpoint.Address.IsIPv4MappedToIPv6)
                        {
                            // InetAddress stores its data on a nested 'holder' instance
                            var holder = InetAddressHolderMethod.Invoke(address, Array.Empty<object>());
                            if (holder == null)
                                throw new global::java.net.SocketException("Could not get InetAddress holder.");

                            // set the address directly onto the holder
                            var bytes = BinaryPrimitives.ReadInt32BigEndian(ipRemoteEndpoint.Address.MapToIPv4().GetAddressBytes());
                            InetAddressHolderAddressField.SetValue(holder, bytes);
                        }
                    }
                    else if (ipRemoteEndpoint.AddressFamily == AddressFamily.InterNetwork)
                    {
                        if (address is global::java.net.Inet6Address)
                        {
                            // InetAddress stores its data on a nested 'holder' instance
                            var holder = Inet6AddressHolderMethod.Invoke(address, Array.Empty<object>());
                            if (holder == null)
                                throw new global::java.net.SocketException("Could not get Inet6Address holder.");

                            // set the address directly onto the holder
                            var bytes = ipRemoteEndpoint.Address.MapToIPv6().GetAddressBytes();
                            Inet6AddressHolderSetIpAddressMethod.Invoke(holder, new[] { bytes });
                        }
                        else
                        {
                            // InetAddress stores its data on a nested 'holder' instance
                            var holder = InetAddressHolderMethod.Invoke(address, Array.Empty<object>());
                            if (holder == null)
                                throw new global::java.net.SocketException("Could not get InetAddress holder.");

                            // set the address directly onto the holder
                            var bytes = BinaryPrimitives.ReadInt32BigEndian(ipRemoteEndpoint.Address.GetAddressBytes());
                            InetAddressHolderAddressField.SetValue(holder, bytes);
                        }
                    }

                    return ipRemoteEndpoint.Port;
                });
            });
#endif
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
            throw new NotSupportedException();
#else
            return SafeInvoke<global::java.net.PlainDatagramSocketImpl, global::java.net.DatagramPacket, int>(this_, packet_, (impl, packet) =>
            {
                return InvokeWithSocket(impl, socket =>
                {
                    if (packet == null)
                        throw new global::java.lang.NullPointerException(nameof(packet));

                    // use asynchronous completion to allow cancelation through Close()
                    var remoteEndpoint = (EndPoint)new IPEndPoint(IPAddress.Any, 0);
                    socket.ReceiveTimeout = impl.timeout;
                    var result = socket.BeginReceiveFrom(packet.getData(), packet.getOffset(), packet.getLength(), SocketFlags.Peek, ref remoteEndpoint, null, null);
                    if (impl.timeout > 0 && result.AsyncWaitHandle.WaitOne(impl.timeout, true) == false)
                        throw new global::java.net.SocketTimeoutException("Peek data timed out.");

                    var n = socket.EndReceiveFrom(result, ref remoteEndpoint);

                    // check that we received an IP endpoint
                    if (remoteEndpoint is not IPEndPoint ipRemoteEndpoint)
                        throw new global::java.net.SocketException("Unexpected resulting endpoint type.");

                    var remoteAddress = ipRemoteEndpoint.ToInetAddress();
                    var packetAddress = packet.getAddress();
                    if (packetAddress == null || packetAddress.equals(remoteAddress) == false)
                        packet.setAddress(ipRemoteEndpoint.ToInetAddress());

                    packet.setPort(ipRemoteEndpoint.Port);
                    packet.setLength(n);
                    return ipRemoteEndpoint.Port;
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
            throw new NotSupportedException();
#else
            SafeInvoke<global::java.net.PlainDatagramSocketImpl, global::java.net.DatagramPacket>(this_, packet_, (impl, packet) =>
            {
                InvokeWithSocket(impl, socket =>
                {
                    if (packet == null)
                        throw new global::java.lang.NullPointerException(nameof(packet));

                    // use asynchronous completion to allow cancelation through Close()
                    var remoteEndpoint = (EndPoint)new IPEndPoint(IPAddress.Any, 0);
                    socket.ReceiveTimeout = impl.timeout;
                    var result = socket.BeginReceiveFrom(packet.getData(), packet.getOffset(), packet.getLength(), SocketFlags.None, ref remoteEndpoint, null, null);
                    if (impl.timeout > 0 && result.AsyncWaitHandle.WaitOne(impl.timeout, true) == false)
                        throw new global::java.net.SocketTimeoutException("Receive timed out.");

                    var n = socket.EndReceiveFrom(result, ref remoteEndpoint);

                    // check that we received an IP endpoint
                    if (remoteEndpoint is not IPEndPoint ipRemoteEndpoint)
                        throw new global::java.net.SocketException("Unexpected resulting endpoint type.");

                    var remoteAddress = ipRemoteEndpoint.ToInetAddress();
                    var packetAddress = packet.getAddress();
                    if (packetAddress == null || packetAddress.equals(remoteAddress) == false)
                        packet.setAddress(ipRemoteEndpoint.ToInetAddress());

                    packet.setPort(ipRemoteEndpoint.Port);
                    packet.setLength(n);
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
            throw new NotSupportedException();
#else
            SocketInvokeFunc<global::java.net.PlainDatagramSocketImpl>(this_, (impl) =>
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
            throw new NotSupportedException();
#else
            return SafeInvoke<global::java.net.PlainDatagramSocketImpl, int>(this_, (impl) =>
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

            // we start by favoring IPv6
            var ipv6 = Socket.OSSupportsIPv6;

            // attempt to join IPv6 multicast
            if (ipv6)
            {
                var o = new IPv6MulticastOption(inetaddr.ToIPAddress());
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
                var o = new MulticastOption(inetaddr.ToIPAddress());
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
            throw new NotSupportedException();
#else
            SocketInvokeFunc<global::java.net.PlainDatagramSocketImpl, global::java.net.InetAddress, global::java.net.NetworkInterface>(this_, inetaddr_, netIf_, (impl, inetaddr, netIf) =>
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
            throw new NotSupportedException();
#else
            SocketInvokeFunc<global::java.net.PlainDatagramSocketImpl, global::java.net.InetAddress, global::java.net.NetworkInterface>(this_, inetaddr_, netIf_, (impl, inetaddr, netIf) =>
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
            NetworkInterfaceIndexField.SetValue(anon, -1);
            NetworkInterfaceAddrsField.SetValue(anon, new global::java.net.InetAddress[0]);
            NetworkInterfaceNameField.SetValue(anon, "");
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
            throw new NotSupportedException();
#else
            return SafeInvoke<global::java.net.PlainDatagramSocketImpl, object>(this_, impl =>
            {
                return InvokeWithSocket<object>(impl, socket =>
                {
                    // IP_MULTICAST_IF returns an InetAddress while IP_MULTICAST_IF2 returns a NetworkInterface
                    if (opt == global::java.net.SocketOptions.IP_MULTICAST_IF ||
                        opt == global::java.net.SocketOptions.IP_MULTICAST_IF2)
                        return GetMulticastInterfaceOption(socket, opt);

                    // .NET provides property
                    if (opt == global::java.net.SocketOptions.SO_BINDADDR)
                        return ((IPEndPoint)socket.LocalEndPoint).ToInetAddress();

                    if (SocketOptionUtil.TryGetDotNetSocketOption(opt, out var options) == false)
                        throw new global::java.net.SocketException("Invalid option.");

                    return socket.GetSocketOption(options.Level, options.Name) switch
                    {
                        bool b => new global::java.lang.Boolean(b),
                        int i => new global::java.lang.Integer(i),
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

        /// <summary>
        /// Implements the setter of the socket options IP_MULTICAST_LOOP.
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="opt"></param>
        /// <param name="val"></param>
        /// <exception cref="global::java.lang.NullPointerException"></exception>
        /// <exception cref="global::java.net.SocketException"></exception>
        static void SetMulticastLoopbackMode(Socket socket, int opt, object val)
        {
            if (val is not global::java.lang.Boolean b)
                throw new global::java.net.SocketException("Bad argument for IP_MULTICAST_LOOP: value is not a boolean");

            if (Socket.OSSupportsIPv6)
                socket.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.MulticastLoopback, b);
            else
                socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastLoopback, b);
        }

#endif

        /// <summary>
        /// Implements the native method for 'socketSetOption'.
        /// </summary>
        /// <param name="this_"></param>
        /// <param name="opt"></param>
        /// <param name="val"></param>
        /// <exception cref="global::java.lang.NullPointerException"></exception>
        /// <exception cref="global::java.net.SocketException"></exception>
        public static void socketSetOption(object this_, int opt, object val)
        {
#if FIRST_PASS
            throw new NotSupportedException();
#else
            SafeInvoke<global::java.net.PlainDatagramSocketImpl>(this_, impl =>
            {
                InvokeWithSocket(impl, socket =>
                {
                    if (val == null)
                        throw new global::java.lang.NullPointerException(nameof(val));

                    // multicast options may receive InetAddress or NetworkInterface
                    if (opt == global::java.net.SocketOptions.IP_MULTICAST_IF ||
                        opt == global::java.net.SocketOptions.IP_MULTICAST_IF2)
                    {
                        SetMulticastInterface(socket, opt, val);
                        return;
                    }

                    // implementation changes based on IP/IPv6
                    if (opt == global::java.net.SocketOptions.IP_MULTICAST_LOOP)
                    {
                        SetMulticastLoopbackMode(socket, opt, val);
                        return;
                    }

                    if (SocketOptionUtil.TryGetDotNetSocketOption(opt, out var options) == false)
                        throw new global::java.net.SocketException("Invalid option.");

                    socket.SetSocketOption(options.Level, options.Name, val switch
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
        /// Implements the native method for 'connect0'.
        /// </summary>
        /// <param name="this_"></param>
        /// <param name="address"></param>
        /// <param name="port"></param>
        /// <exception cref="global::java.lang.NullPointerException"></exception>
        /// <exception cref="global::java.net.SocketException"></exception>
        public static void connect0(object this_, object address_, int port)
        {
#if FIRST_PASS
            throw new NotSupportedException();
#else
            SafeInvoke<global::java.net.PlainDatagramSocketImpl, global::java.net.InetAddress>(this_, address_, (impl, address) =>
            {
                InvokeWithSocket(impl, socket =>
                {
                    if (address == null)
                        throw new global::java.lang.NullPointerException(nameof(address));

                    socket.Connect(address.ToIPAddress(), port);
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
            throw new NotSupportedException();
#else
            SocketInvokeFunc<global::java.net.PlainDatagramSocketImpl>(this_, impl =>
            {
                InvokeWithSocket(impl, socket =>
                {
                    socket.Disconnect(false);
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
            throw new NotSupportedException();
#else
            return SafeInvoke<global::java.net.PlainDatagramSocketImpl, int>(this_, impl =>
            {
                return InvokeWithSocket(impl, socket =>
                {
                    return socket.Available;
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
        static void InvokeWithSocket(global::java.net.PlainDatagramSocketImpl impl, Action<Socket> action)
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
        static TResult InvokeWithSocket<TResult>(global::java.net.PlainDatagramSocketImpl impl, Func<Socket, TResult> func)
        {
            var socket = (Socket)impl.fd?.getSocket();
            if (socket == null)
                throw new global::java.net.SocketException("Socket closed.");

            return func(socket);
        }

#endif

    }

}
