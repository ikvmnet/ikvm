/*
  Copyright (C) 2011 Jeroen Frijters

  This software is provided 'as-is', without any express or implied
  warranty.  In no event will the authors be held liable for any damages
  arising from the use of this software.

  Permission is granted to anyone to use this software for any purpose,
  including commercial applications, and to alter it and redistribute it
  freely, subject to the following restrictions:

  1. The origin of this software must not be misrepresented; you must not
     claim that you wrote the original software. If you use this software
     in a product, an acknowledgment in the product documentation would be
     appreciated but is not required.
  2. Altered source versions must be plainly marked as such, and must not be
     misrepresented as being the original software.
  3. This notice may not be removed or altered from any source distribution.

  Jeroen Frijters
  jeroen@frijters.net
  
*/

using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;

using IKVM.Runtime.Java.Externs.java.net;

using static IKVM.Java.Externs.java.net.SocketImplUtil;

namespace IKVM.Java.Externs.sun.nio.ch
{

    static class Net
    {

#if !FIRST_PASS

#if  NETCOREAPP3_1_OR_GREATER

        [DllImport("libc", SetLastError = true)]
        static unsafe extern int setsockopt(SafeHandle sockfd, int level, int optname, void* optval, int optlen);

        [DllImport("libc", SetLastError = true)]
        static unsafe extern int getsockopt(SafeHandle sockfd, int level, int optname, void* optval, int* optlen);

        const int IPPROTO_IP = 0;
        const int IPPROTO_IPV6 = 41;
        const int IP_ADD_SOURCE_MEMBERSHIP = 39;
        const int IP_DROP_SOURCE_MEMBERSHIP = 40;
        const int MCAST_JOIN_SOURCE_GROUP = 46;
        const int MCAST_LEAVE_SOURCE_GROUP = 47;

#endif

#endif

        /// <summary>
        /// Implements the native method for 'initIDs'.
        /// </summary>
        /// <returns></returns>
        public static void initIDs()
        {

        }

        /// <summary>
        /// Implements the native method for 'isIPv6Available0'.
        /// </summary>
        /// <returns></returns>
        public static bool isIPv6Available0()
        {
            return Socket.OSSupportsIPv6;
        }

        /// <summary>
        /// Implements the native method for 'isExclusiveBindAvailable'.
        /// </summary>
        /// <returns></returns>
        public static int isExclusiveBindAvailable()
        {
            return RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? 1 : -1;
        }

        /// <summary>
        /// Implements the native method for 'canIPv6SocketJoinIPv4Group0'.
        /// </summary>
        /// <returns></returns>
        public static bool canIPv6SocketJoinIPv4Group0()
        {
            return false;
        }

        /// <summary>
        /// Implements the native method for 'canJoin6WithIPv4Group0'.
        /// </summary>
        /// <returns></returns>
        public static bool canJoin6WithIPv4Group0()
        {
            return false;
        }

        /// <summary>
        /// Implements the native method for 'shutdown'.
        /// </summary>
        /// <param name="fd"></param>
        /// <param name="how"></param>
        public static void shutdown(global::java.io.FileDescriptor fd, int how)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            InvokeAction(() =>
            {
                InvokeWithSocket(fd, socket =>
                {
                    socket.Shutdown((SocketShutdown)how);
                });
            });
#endif
        }

        /// <summary>
        /// Implements the native method for 'localInetAddress'.
        /// </summary>
        /// <param name="fd"></param>
        /// <returns></returns>
        public static int localPort(global::java.io.FileDescriptor fd)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            return InvokeFunc(() =>
            {
                return InvokeWithSocket(fd, socket =>
                {
                    return ((IPEndPoint)socket.LocalEndPoint).Port;
                });
            });
#endif
        }

        /// <summary>
        /// Implements the native method for 'localInetAddress'.
        /// </summary>
        /// <param name="fd"></param>
        /// <returns></returns>
        /// <exception cref="global::java.net.SocketException"></exception>
        public static global::java.net.InetAddress localInetAddress(global::java.io.FileDescriptor fd)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            return InvokeFunc(() =>
            {
                return InvokeWithSocket(fd, socket =>
                {
                    return ((IPEndPoint)socket.LocalEndPoint).ToInetAddress();
                });
            });
#endif
        }

        /// <summary>
        /// <summary>
        /// Implements the native method for 'remotePort'.
        /// </summary>
        /// <param name="fd"></param>
        /// <returns></returns>
        /// <exception cref="global::java.net.SocketException"></exception>
        public static int remotePort(global::java.io.FileDescriptor fd)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            return InvokeFunc(() =>
            {
                return InvokeWithSocket(fd, socket =>
                {
                    return ((IPEndPoint)socket.RemoteEndPoint).Port;
                });
            });
#endif
        }

        /// <summary>
        /// Implements the native method for 'remoteInetAddress'.
        /// </summary>
        /// <param name="fd"></param>
        /// <returns></returns>
        public static global::java.net.InetAddress remoteInetAddress(global::java.io.FileDescriptor fd)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            return InvokeFunc(() =>
            {
                return InvokeWithSocket(fd, socket =>
                {
                    return global::java.net.SocketUtil.getInetAddressFromIPEndPoint((System.Net.IPEndPoint)socket.RemoteEndPoint);
                });
            });
#endif
        }

        /// <summary>
        /// Implements the native method for 'setIntOption0'.
        /// </summary>
        /// <param name="fd"></param>
        /// <param name="mayNeedConversion"></param>
        /// <param name="level"></param>
        /// <param name="opt"></param>
        /// <returns></returns>
        /// <exception cref="global::java.net.SocketException"></exception>
        public static int getIntOption0(global::java.io.FileDescriptor fd, bool mayNeedConversion, int level, int opt)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            return InvokeFunc(() =>
            {
                return InvokeWithSocket(fd, socket =>
                {
                    var sol = (SocketOptionLevel)level;
                    var son = (SocketOptionName)opt;

                    if (sol == SocketOptionLevel.IPv6 && opt == global::ikvm.@internal.Winsock.IPV6_TCLASS)
                        return 0;

                    try
                    {
                        object obj = socket.GetSocketOption(sol, son);
                        return obj is LingerOption linger ? linger.Enabled ? linger.LingerTime : -1 : (int)obj;
                    }
                    catch (SocketException e) when (mayNeedConversion && e.SocketErrorCode == SocketError.ProtocolOption && sol == SocketOptionLevel.IP && son == SocketOptionName.TypeOfService)
                    {
                        return 0;
                    }
                });
            });
#endif
        }

        /// <summary>
        /// Implements the native method for 'setIntOption0'.
        /// </summary>
        /// <param name="fd"></param>
        /// <param name="mayNeedConversion"></param>
        /// <param name="level"></param>
        /// <param name="opt"></param>
        /// <param name="arg"></param>
        /// <param name="isIPv6"></param>
        /// <exception cref="global::java.net.SocketException"></exception>
        public static void setIntOption0(global::java.io.FileDescriptor fd, bool mayNeedConversion, int level, int opt, int arg, bool isIPv6)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            InvokeAction(() =>
            {
                InvokeWithSocket(fd, socket =>
                {
                    var sol = (SocketOptionLevel)level;
                    var son = (SocketOptionName)opt;

                    if (sol == SocketOptionLevel.IPv6 && opt == global::ikvm.@internal.Winsock.IPV6_TCLASS)
                        return;

                    if (mayNeedConversion)
                    {
                        const int IPTOS_TOS_MASK = 0x1e;
                        const int IPTOS_PREC_MASK = 0xe0;
                        if (sol == SocketOptionLevel.IP && son == SocketOptionName.TypeOfService)
                            arg &= IPTOS_TOS_MASK | IPTOS_PREC_MASK;
                    }

                    try
                    {
                        socket.SetSocketOption(sol, son, arg);
                    }
                    catch (SocketException e) when (mayNeedConversion && e.SocketErrorCode == SocketError.ProtocolOption && sol == SocketOptionLevel.IP && (son == SocketOptionName.TypeOfService || son == SocketOptionName.MulticastLoopback))
                    {
                        return;
                    }
                    catch (SocketException e) when (mayNeedConversion && e.SocketErrorCode == SocketError.InvalidArgument && sol == SocketOptionLevel.IP && son == SocketOptionName.TypeOfService)
                    {
                        return;
                    }
                });
            });
#endif
        }

        static void PutInt(byte[] buf, int pos, int value)
        {
            buf[pos + 0] = (byte)(value >> 24);
            buf[pos + 1] = (byte)(value >> 16);
            buf[pos + 2] = (byte)(value >> 8);
            buf[pos + 3] = (byte)(value >> 0);
        }

        /// <summary>
        /// Implements the native method for 'joinOrDrop4'.
        /// </summary>
        /// <param name="join"></param>
        /// <param name="fd"></param>
        /// <param name="group"></param>
        /// <param name="interf"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        /// <exception cref="global::java.net.SocketException"></exception>
        public static int joinOrDrop4(bool join, global::java.io.FileDescriptor fd, int group, int interf, int source)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            return InvokeFunc(() =>
            {
                return InvokeWithSocket(fd, socket =>
                {
                    if (source == 0)
                    {
                        socket.SetSocketOption(SocketOptionLevel.IP, join ? SocketOptionName.AddMembership : SocketOptionName.DropMembership, new MulticastOption(new IPAddress(IPAddress.HostToNetworkOrder(group) & 0xFFFFFFFFL), new IPAddress(IPAddress.HostToNetworkOrder(interf) & 0xFFFFFFFFL)));
                    }
                    else
                    {
                        // ip_mreq_source
                        var optionValue = new byte[12];
                        PutInt(optionValue, 0, group);
                        PutInt(optionValue, 4, source);
                        PutInt(optionValue, 8, interf);

#if NETCOREAPP3_1_OR_GREATER
                        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                        {
                            socket.SetSocketOption(SocketOptionLevel.IP, join ? SocketOptionName.AddSourceMembership : SocketOptionName.DropSourceMembership, optionValue);
                        }
                        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                        {
                            unsafe
                            {
                                fixed (void* v = optionValue)
                                    if (setsockopt(socket.SafeHandle, IPPROTO_IP, join ? IP_ADD_SOURCE_MEMBERSHIP : IP_DROP_SOURCE_MEMBERSHIP, v, optionValue.Length) != 0)
                                        throw new SocketException(Marshal.GetLastWin32Error());
                            }
                        }
                        else
                        {
                            throw new global::java.net.SocketException("Invalid option.");
                        }
#else
                        socket.SetSocketOption(SocketOptionLevel.IP, join ? SocketOptionName.AddSourceMembership : SocketOptionName.DropSourceMembership, optionValue);
#endif
                    }

                    return 0;
                });
            });
#endif
        }

        /// <summary>
        /// Implements the native method for 'blockOrUnblock4'.
        /// </summary>
        /// <param name="block"></param>
        /// <param name="fd"></param>
        /// <param name="group"></param>
        /// <param name="interf"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        /// <exception cref="global::java.net.SocketException"></exception>
        public static int blockOrUnblock4(bool block, global::java.io.FileDescriptor fd, int group, int interf, int source)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            return InvokeFunc(() =>
            {
                return InvokeWithSocket(fd, socket =>
                {
                    // ip_mreq_source
                    var optionValue = new byte[12];
                    PutInt(optionValue, 0, group);
                    PutInt(optionValue, 4, source);
                    PutInt(optionValue, 8, interf);
                    socket.SetSocketOption(SocketOptionLevel.IP, block ? SocketOptionName.BlockSource : SocketOptionName.UnblockSource, optionValue);
                    return 0;
                });
            });
#endif
        }

        /// <summary>
        /// Write a sockaddr_in6 into optionValue at offset pos.
        /// </summary>
        /// <param name="optionValue"></param>
        /// <param name="pos"></param>
        /// <param name="addr"></param>
        static void PutSockAddrIn6(byte[] optionValue, int pos, byte[] addr)
        {
            // sin6_family
            optionValue[pos] = 23; // AF_INET6

            // sin6_addr
            Buffer.BlockCopy(addr, 0, optionValue, pos + 8, addr.Length);
        }

        /// <summary>
        /// Implements the native method for 'joinOrDrop6'.
        /// </summary>
        /// <param name="join"></param>
        /// <param name="fd"></param>
        /// <param name="group"></param>
        /// <param name="index"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        /// <exception cref="global::java.net.SocketException"></exception>
        public static int joinOrDrop6(bool join, global::java.io.FileDescriptor fd, byte[] group, int index, byte[] source)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            return InvokeFunc(() =>
            {
                return InvokeWithSocket(fd, socket =>
                {
                    if (source == null)
                    {
                        socket.SetSocketOption(SocketOptionLevel.IPv6, join ? SocketOptionName.AddMembership : SocketOptionName.DropMembership, new IPv6MulticastOption(new IPAddress(group), index));
                    }
                    else
                    {
                        // group_source_req
                        var optionValue = new byte[264];
                        optionValue[0] = (byte)index;
                        optionValue[1] = (byte)(index >> 8);
                        optionValue[2] = (byte)(index >> 16);
                        optionValue[3] = (byte)(index >> 24);
                        PutSockAddrIn6(optionValue, 8, group);
                        PutSockAddrIn6(optionValue, 136, source);

#if NETCOREAPP3_1_OR_GREATER
                        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                        {
                            socket.SetSocketOption(SocketOptionLevel.IPv6, join ? SocketOptionName.AddSourceMembership : SocketOptionName.DropSourceMembership, optionValue);
                        }
                        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                        {
                            unsafe
                            {
                                fixed (void* v = optionValue)
                                    if (setsockopt(socket.SafeHandle, IPPROTO_IPV6, join ? MCAST_JOIN_SOURCE_GROUP : MCAST_LEAVE_SOURCE_GROUP, v, optionValue.Length) != 0)
                                        throw new SocketException(Marshal.GetLastWin32Error());
                            }
                        }
                        else
                        {
                            throw new global::java.net.SocketException("Invalid option.");
                        }
#else
                        socket.SetSocketOption(SocketOptionLevel.IPv6, join ? SocketOptionName.AddSourceMembership : SocketOptionName.DropSourceMembership, optionValue);
#endif
                    }

                    return 0;
                });
            });
#endif
        }

        /// <summary>
        /// Implements the native method for 'blockOrUnblock6'.
        /// </summary>
        /// <param name="block"></param>
        /// <param name="fd"></param>
        /// <param name="group"></param>
        /// <param name="index"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        /// <exception cref="global::java.net.SocketException"></exception>
        public static int blockOrUnblock6(bool block, global::java.io.FileDescriptor fd, byte[] group, int index, byte[] source)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            return InvokeFunc(() =>
            {
                return InvokeWithSocket(fd, socket =>
                {
                    // group_source_req
                    var optionValue = new byte[264];
                    optionValue[0] = (byte)index;
                    optionValue[1] = (byte)(index >> 8);
                    optionValue[2] = (byte)(index >> 16);
                    optionValue[3] = (byte)(index >> 24);
                    PutSockAddrIn6(optionValue, 8, group);
                    PutSockAddrIn6(optionValue, 136, source);
                    socket.SetSocketOption(SocketOptionLevel.IPv6, block ? SocketOptionName.BlockSource : SocketOptionName.UnblockSource, optionValue);
                    return 0;
                });
            });
#endif
        }

        /// <summary>
        /// Implements the native method for 'setInterface4'.
        /// </summary>
        /// <param name="fd"></param>
        /// <param name="interf"></param>
        /// <exception cref="global::java.net.SocketException"></exception>
        public static void setInterface4(global::java.io.FileDescriptor fd, int interf)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            InvokeAction(() =>
            {
                InvokeWithSocket(fd, socket =>
                {
                    socket.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.MulticastInterface, IPAddress.HostToNetworkOrder(interf));
                });
            });
#endif
        }

        /// <summary>
        /// Implements the native method for 'getInterface4'.
        /// </summary>
        /// <param name="fd"></param>
        /// <returns></returns>
        /// <exception cref="global::java.net.SocketException"></exception>
        public static int getInterface4(global::java.io.FileDescriptor fd)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            return InvokeFunc(() =>
            {
                return InvokeWithSocket(fd, socket =>
                {
                    return IPAddress.NetworkToHostOrder((int)socket.GetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.MulticastInterface));
                });
            });
#endif
        }

        /// <summary>
        /// Implements the native method for 'setInterface6'.
        /// </summary>
        /// <param name="fd"></param>
        /// <param name="index"></param>
        /// <exception cref="global::java.net.SocketException"></exception>
        public static void setInterface6(global::java.io.FileDescriptor fd, int index)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            InvokeAction(() =>
            {
                InvokeWithSocket(fd, socket =>
                {
                    socket.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.MulticastInterface, index);
                });
            });
#endif
        }

        /// <summary>
        /// Implements the native method for 'getInterface6'.
        /// </summary>
        /// <param name="fd"></param>
        /// <returns></returns>
        /// <exception cref="global::java.net.SocketException"></exception>
        public static int getInterface6(global::java.io.FileDescriptor fd)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            return InvokeFunc(() =>
            {
                return InvokeWithSocket(fd, socket =>
                {
                    return (int)socket.GetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.MulticastInterface);
                });
            });
#endif
        }

        /// <summary>
        /// Implements the native method for 'socket0'.
        /// </summary>
        /// <param name="preferIPv6"></param>
        /// <param name="stream"></param>
        /// <param name="reuse"></param>
        /// <returns></returns>
        public static global::java.io.FileDescriptor socket0(bool preferIPv6, bool stream, bool reuse)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            return InvokeFunc(() =>
            {
                var addressFamily = preferIPv6 ? AddressFamily.InterNetworkV6 : AddressFamily.InterNetwork;
                var socketType = stream ? SocketType.Stream : SocketType.Dgram;
                var protocolType = stream ? ProtocolType.Tcp : ProtocolType.Udp;
                var socket = new Socket(addressFamily, socketType, protocolType);

                if (preferIPv6)
                    socket.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.IPv6Only, 0);

                if (!stream)
                    setConnectionReset(socket, false);

                var fd = new global::java.io.FileDescriptor();
                fd.setSocket(socket);
                return fd;
            });
#endif
        }

        /// <summary>
        /// Implements the native method for 'bind0'.
        /// </summary>
        /// <param name="fd"></param>
        /// <param name="preferIPv6"></param>
        /// <param name="useExclBind"></param>
        /// <param name="addr"></param>
        /// <param name="port"></param>
        /// <exception cref="global::java.net.SocketException"></exception>
        public static void bind0(global::java.io.FileDescriptor fd, bool preferIPv6, bool useExclBind, global::java.net.InetAddress addr, int port)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            InvokeAction(() =>
            {
                InvokeWithSocket(fd, socket =>
                {
                    if (useExclBind && (int)socket.GetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress) != 1)
                        socket.ExclusiveAddressUse = true;

                    socket.Bind(new System.Net.IPEndPoint(addr.ToIPAddress(), port));
                });
            });
#endif
        }

        /// <summary>
        /// Implements the native method for 'listen'.
        /// </summary>
        /// <param name="fd"></param>
        /// <param name="backlog"></param>
        /// <exception cref="global::java.net.SocketException"></exception>
        public static void listen(global::java.io.FileDescriptor fd, int backlog)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            InvokeAction(() =>
            {
                InvokeWithSocket(fd, socket =>
                {
                    socket.Listen(backlog);
                });
            });
#endif
        }

        internal static void setConnectionReset(Socket socket, bool enable)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                // Windows 2000 introduced a "feature" that causes it to return WSAECONNRESET from receive,
                // if a previous send resulted in an ICMP port unreachable. For unconnected datagram sockets,
                // we disable this feature by using this ioctl.
                const int IOC_IN = unchecked((int)0x80000000);
                const int IOC_VENDOR = 0x18000000;
                const int SIO_UDP_CONNRESET = IOC_IN | IOC_VENDOR | 12;
                socket.IOControl(SIO_UDP_CONNRESET, new byte[] { enable ? (byte)1 : (byte)0 }, null);
            }
        }

        /// <summary>
        /// Implements the native method for 'connect0'.
        /// </summary>
        /// <param name="preferIPv6"></param>
        /// <param name="fd"></param>
        /// <param name="remote"></param>
        /// <param name="remotePort"></param>
        /// <returns></returns>
        public static int connect0(bool preferIPv6, global::java.io.FileDescriptor fd, global::java.net.InetAddress remote, int remotePort)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            return InvokeFunc<int>(() =>
            {
                return InvokeWithSocket(fd, socket =>
                {
                    var ep = new System.Net.IPEndPoint(global::java.net.SocketUtil.getAddressFromInetAddress(remote, preferIPv6), remotePort);
                    var datagram = socket.SocketType == SocketType.Dgram;
                    if (datagram || fd.isSocketBlocking())
                    {
                        socket.Connect(ep);
                        if (datagram)
                            setConnectionReset(socket, true);

                        return 1;
                    }
                    else
                    {
                        fd.setAsyncResult(socket.BeginConnect(ep, null, null));
                        return global::sun.nio.ch.IOStatus.UNAVAILABLE;
                    }
                });
            });
#endif
        }

        /// <summary>
        /// Implements the native method for 'poll'.
        /// </summary>
        /// <param name="fd"></param>
        /// <param name="events"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        public static int poll(global::java.io.FileDescriptor fd, int events, long timeout)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            return InvokeFunc(() =>
            {
                return InvokeWithSocket(fd, socket =>
                {
                    var selectMode = events switch
                    {
                        int e when e == global::sun.nio.ch.Net.POLLCONN => SelectMode.SelectWrite,
                        int e when e == global::sun.nio.ch.Net.POLLOUT => SelectMode.SelectWrite,
                        int e when e == global::sun.nio.ch.Net.POLLIN => SelectMode.SelectRead,
                        _ => throw new NotSupportedException(),
                    };

                    if (socket.Poll(timeout * 1000 > int.MaxValue ? int.MaxValue : (int)timeout * 1000, selectMode))
                        return events;

                    return 0;
                });
            });
#endif
        }

        public static short pollinValue() => 0x0001;

        public static short polloutValue() => 0x0004;

        public static short pollerrValue() => 0x0008;

        public static short pollhupValue() => 0x0010;

        public static short pollnvalValue() => 0x0020;

        public static short pollconnValue() => 0x0002;

#if !FIRST_PASS

        /// <summary>
        /// Invokes the given action with the current socket, catching and mapping any resulting .NET exceptions.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fd"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        /// <exception cref="global::java.lang.NullPointerException"></exception>
        /// <exception cref="global::java.net.SocketException"></exception>
        static void InvokeWithSocket(global::java.io.FileDescriptor fd, Action<Socket> action)
        {
            var socket = fd?.getSocket();
            if (socket == null)
                throw new global::java.net.SocketException("Socket closed");

            action(socket);
        }

        /// <summary>
        /// Invokes the given function with the current socket, catching and mapping any resulting .NET exceptions.
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="fd"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        /// <exception cref="global::java.lang.NullPointerException"></exception>
        /// <exception cref="global::java.net.SocketException"></exception>
        static TResult InvokeWithSocket<TResult>(global::java.io.FileDescriptor fd, Func<Socket, TResult> func)
        {
            var socket = fd?.getSocket();
            if (socket == null)
                throw new global::java.net.SocketException("Socket closed");

            return func(socket);
        }

#endif

    }

}
